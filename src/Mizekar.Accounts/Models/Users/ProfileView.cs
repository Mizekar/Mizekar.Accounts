using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mizekar.Accounts.Models.Users
{
    public class ProfileView
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileImageThumb { get; set; }
    }
}
