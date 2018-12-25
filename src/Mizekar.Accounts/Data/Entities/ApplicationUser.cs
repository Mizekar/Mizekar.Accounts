using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Mizekar.Accounts.Data.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string Title => $"{FirstName} {LastName}";
    }
}
