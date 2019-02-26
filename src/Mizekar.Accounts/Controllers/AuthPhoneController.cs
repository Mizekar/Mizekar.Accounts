using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mizekar.Accounts.Data.Entities;
using Mizekar.Accounts.Helper;
using Mizekar.Accounts.Services;
using Mizekar.Accounts.ViewModels;
using Mizekar.Core.Extensions;

namespace Mizekar.Accounts.Controllers
{
    /// <summary>
    /// Auth System By SMS
    /// </summary>
    [Route("api/auth/phone")]
    public class AuthPhoneController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISmsService _smsService;
        private readonly DataProtectorTokenProvider<ApplicationUser> _dataProtectorTokenProvider;
        private readonly PhoneNumberTokenProvider<ApplicationUser> _phoneNumberTokenProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthPhoneController(
            IConfiguration configuration,
            ISmsService smsService,
            DataProtectorTokenProvider<ApplicationUser> dataProtectorTokenProvider,
            PhoneNumberTokenProvider<ApplicationUser> phoneNumberTokenProvider,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
            _dataProtectorTokenProvider = dataProtectorTokenProvider ?? throw new ArgumentNullException(nameof(dataProtectorTokenProvider));
            _phoneNumberTokenProvider = phoneNumberTokenProvider ?? throw new ArgumentNullException(nameof(phoneNumberTokenProvider));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// Send Verify Code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PhoneLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!PhoneNumbers.ValidatePhoneNumber(model.PhoneNumber))
            {
                return BadRequest("phone invalid");
            }
            var phoneNumber = PhoneNumbers.NormalizePhoneNumber(model.PhoneNumber);
            phoneNumber = _userManager.NormalizeKey(phoneNumber);
            var user = await GetUser(phoneNumber);
            var response = await SendSmsRequet(phoneNumber, user);

            if (!response.Result)
            {
                return BadRequest("Sending sms failed");
            }

            var resendToken = await _dataProtectorTokenProvider.GenerateAsync("resend_token", _userManager, user);
            var body = GetBody(response.VerifyToken, resendToken);

            return Accepted(body);
        }

        /// <summary>
        /// reSend Verify Code
        /// </summary>
        /// <param name="resendToken"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(string resendToken, [FromBody]PhoneLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!PhoneNumbers.ValidatePhoneNumber(model.PhoneNumber))
            {
                return BadRequest("phone invalid");
            }

            var phoneNumber = PhoneNumbers.NormalizePhoneNumber(model.PhoneNumber);
            phoneNumber = _userManager.NormalizeKey(phoneNumber);
            var user = await GetUser(phoneNumber);
            if (!await _dataProtectorTokenProvider.ValidateAsync("resend_token", resendToken, _userManager, user))
            {
                return BadRequest("Invalid resend token");
            }

            var response = await SendSmsRequet(phoneNumber, user);

            if (!response.Result)
            {
                return BadRequest("Sending sms failed");
            }

            var newResendToken = await _dataProtectorTokenProvider.GenerateAsync("resend_token", _userManager, user);
            var body = GetBody(response.VerifyToken, newResendToken);
            return Accepted(body);
        }

        private async Task<ApplicationUser> GetUser(string phoneNumber)
        {
            var applicationUser = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            if (applicationUser == null)
            {
                applicationUser = new ApplicationUser
                {
                    PhoneNumber = phoneNumber,
                    SecurityStamp = phoneNumber.Sha256(),
                    UserName = phoneNumber,
                };
                await _userManager.CreateAsync(applicationUser);
            }

            return applicationUser;
        }

        private async Task<(string VerifyToken, bool Result)> SendSmsRequet(string phoneNumber, ApplicationUser user)
        {
            var verifyToken = await _phoneNumberTokenProvider.GenerateAsync("verify_number", _userManager, user);
            var result = await _smsService.SendAsync(phoneNumber, $"login verification code : {verifyToken}");
            return (verifyToken, result);
        }

        private Dictionary<string, string> GetBody(string verifyToken, string resendToken)
        {
            var body = new Dictionary<string, string> { { "resend_token", resendToken } };
            if (_configuration["ReturnVerifyTokenForTesting"] == bool.TrueString)
            {
                body.Add("verify_token", verifyToken);
            }
            return body;
        }
    }
}
