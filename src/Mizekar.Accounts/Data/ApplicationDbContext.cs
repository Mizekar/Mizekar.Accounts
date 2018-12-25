using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mizekar.Accounts.Data.Entities;

namespace Mizekar.Accounts.Data
{
    public class AccountsDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
            : base(options)
        {
        }
    }
}
