using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mizekar.Accounts.Areas.Manage.Models
{
    public class Dashboard
    {
        public int Users { get; set; }
        public int Roles { get; set; }
        public int Clients { get; set; }
        public int PersistedGrants { get; set; }
        public int ApiResources { get; set; }
        public int IdentityResources { get; set; }
        
    }
}
