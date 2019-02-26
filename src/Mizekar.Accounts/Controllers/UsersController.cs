using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Accounts.Data;
using Mizekar.Accounts.Models.Users;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mizekar.Accounts.Controllers
{
    [Route("v1/api/users")]
    public class UsersController : Controller
    {
        private readonly AccountsDbContext _accountsDbContext;

        public UsersController(AccountsDbContext accountsDbContext)
        {
            _accountsDbContext = accountsDbContext;
        }

        /// <summary>
        /// Get Profile by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<ProfileView>> GetProfile([FromRoute]Guid userId)
        {
            var user = await _accountsDbContext.Users.FindAsync(userId);
            var model = new ProfileView()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return Ok(model);
        }

        /// <summary>
        /// Get Profiles by Ids
        /// </summary>
        /// <param name="userIds">user id's</param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<List<ProfileView>>> GetProfile(List<Guid> userIds)
        {
            if (userIds == null)
            {
                var users = await _accountsDbContext.Users.Where(w => userIds.Contains(w.Id)).ToListAsync();
                var usersModel = new List<ProfileView>();
                foreach (var user in users)
                {
                    var model = new ProfileView()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    };
                    usersModel.Add(model);
                }

                return Ok(usersModel);
            }

            return BadRequest("user Id's null");
        }

        /// <summary>
        /// Get Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ProfileView>> GetProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _accountsDbContext.Users.FindAsync(userId);
                var model = new ProfileView()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                return Ok(model);
            }

            return Forbid("no user logged in");
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="updateProfile"></param>
        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult> UpdateProfile([FromBody]UpdateProfile updateProfile)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _accountsDbContext.Users.FindAsync(userId);
                user.FirstName = updateProfile.FirstName;
                user.LastName = updateProfile.LastName;
                await _accountsDbContext.SaveChangesAsync();
                return Ok();
            }

            return Forbid("no user logged in");
        }

        /// <summary>
        /// Update Profile by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateProfile"></param>
        [HttpPost("{id}")]
        public async Task<ActionResult> UpdateProfile(Guid userId, [FromBody]UpdateProfile updateProfile)
        {
            var user = await _accountsDbContext.Users.FindAsync(userId);
            user.FirstName = updateProfile.FirstName;
            user.LastName = updateProfile.LastName;
            await _accountsDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
