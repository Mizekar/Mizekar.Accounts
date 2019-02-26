using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mizekar.Accounts.Data;
using Mizekar.Accounts.Data.Entities;

namespace Mizekar.Accounts.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class RolesController : Controller
    {
        private readonly AccountsDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILoggerFactory _loggerFactory;

        public RolesController(AccountsDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _loggerFactory = loggerFactory;
        }

        // GET: Manage/Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        // GET: Manage/Roles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // GET: Manage/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.RoleExistsAsync(applicationRole.Name) == false)
                {
                    await _roleManager.CreateAsync(new ApplicationRole() { Name = applicationRole.Name });
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Name already Exist");
                }
            }
            return View(applicationRole);
        }

        // GET: Manage/Roles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }
            return View(applicationRole);
        }

        // POST: Manage/Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] ApplicationRole applicationRole)
        {
            if (id != applicationRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                role.Name = applicationRole.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Name", result.Errors.First().ToString());
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationRole);
        }

        // GET: Manage/Roles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // POST: Manage/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicationRole = await _context.Roles.FindAsync(id);
            await _roleManager.DeleteAsync(applicationRole);
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationRoleExists(Guid id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
