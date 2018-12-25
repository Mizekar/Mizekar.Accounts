﻿using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mizekar.Accounts.Data.Entities;
using Mizekar.Accounts.Models;

namespace Mizekar.Accounts.Validation
{
	public class PhoneNumberTokenGrantValidator : IExtensionGrantValidator
	{
		private readonly PhoneNumberTokenProvider<ApplicationUser> _phoneNumberTokenProvider;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEventService _events;
		private readonly ILogger<PhoneNumberTokenGrantValidator> _logger;

		public PhoneNumberTokenGrantValidator(PhoneNumberTokenProvider<ApplicationUser> phoneNumberTokenProvider,
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEventService events,
			ILogger<PhoneNumberTokenGrantValidator> logger)
		{
			_phoneNumberTokenProvider = phoneNumberTokenProvider;
			_userManager = userManager;
			_signInManager = signInManager;
			_events = events;
			_logger = logger;
		}

		public async Task ValidateAsync(ExtensionGrantValidationContext context)
		{
			//var createUser = false;
			var raw = context.Request.Raw;
			var credential = raw.Get(OidcConstants.TokenRequest.GrantType);
			if (credential != null && credential == Constants.AuthConstants.GrantType.PhoneNumberToken)
			{
				var phoneNumber = raw.Get(Constants.AuthConstants.TokenRequest.PhoneNumber);
				var verificationToken = raw.Get(Constants.AuthConstants.TokenRequest.Token);

				var user = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == _userManager.NormalizeKey(phoneNumber));
				//if (user == null)
				//{
				//	user = new ApplicationUser
				//	{
				//		UserName = phoneNumber,
				//		PhoneNumber = phoneNumber,
				//		SecurityStamp = phoneNumber.Sha256()
				//	};
				//	createUser = true;
				//}

				var result = await _phoneNumberTokenProvider.ValidateAsync("verify_number", verificationToken, _userManager, user);
				if (result)
				{
				    user.PhoneNumberConfirmed = true;
				    await _userManager.UpdateAsync(user);
                    //if (createUser)
                    //{
                    //	user.PhoneNumberConfirmed = true;
                    //	var resultCreation = await _userManager.CreateAsync(user);
                    //	if (resultCreation != IdentityResult.Success)
                    //	{
                    //		_logger.LogInformation("User creation failed: {username}, reason: invalid user", phoneNumber);
                    //		await _events.RaiseAsync(new UserLoginFailureEvent(phoneNumber, resultCreation.Errors.Select(x => x.Description).Aggregate((a, b) => a + ", " + b), false));
                    //		return;
                    //	}
                    //}

                    _logger.LogInformation("Credentials validated for username: {phoneNumber}", phoneNumber);
					await _events.RaiseAsync(new UserLoginSuccessEvent(phoneNumber, user.Id.ToString(), phoneNumber, false));
					await _signInManager.SignInAsync(user, true);
					context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.ConfirmationBySms);
				}
				else
				{
					_logger.LogInformation("Authentication failed for token: {token}, reason: invalid token", verificationToken);
					await _events.RaiseAsync(new UserLoginFailureEvent(verificationToken, "invalid token or verification id", false));
				}
			}
			else
			{
				context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid verify_phone_number_token credential");
			}
		}

		public string GrantType => Constants.AuthConstants.GrantType.PhoneNumberToken;
	}
}

