using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Mizekar.Accounts.Areas.Manage.Models;
using Mizekar.Accounts.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mizekar.Accounts.Areas.Manage.Controllers
{
    [Area("manage")]
    public class HomeController : Controller
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;

        public HomeController(AccountsDbContext accountsDbContext, ConfigurationDbContext configurationDbContext, IClientStore clientStore, IResourceStore resourceStore)
        {
            _accountsDbContext = accountsDbContext;
            _configurationDbContext = configurationDbContext;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new Dashboard()
            {
                Users = _accountsDbContext.Users.Count(),
                Roles = _accountsDbContext.Roles.Count(),
                Clients = _configurationDbContext.Clients.Count(),
                ApiResources = _configurationDbContext.ApiResources.Count(),
                IdentityResources = _configurationDbContext.IdentityResources.Count(),
            };
            return View(model);
        }
    }
}
