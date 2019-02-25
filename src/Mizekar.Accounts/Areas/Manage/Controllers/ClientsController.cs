using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mizekar.Accounts.Areas.Manage.Models.Clients;
using Mizekar.Accounts.Data;

namespace Mizekar.Accounts.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ClientsController : Controller
    {
        private readonly ConfigurationDbContext _context;

        public ClientsController(ConfigurationDbContext configurationDbContext)
        {
            _context = configurationDbContext;
        }

        // GET: Manage/Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        //// GET: Manage/Clients/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var clientViewModel = await _context.ClientViewModel
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (clientViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(clientViewModel);
        //}

        //// GET: Manage/Clients/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Manage/Clients/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Enabled,ClientId,ProtocolType,RequireClientSecret,ClientName,Description,ClientUri,LogoUri,RequireConsent,AllowRememberConsent,AlwaysIncludeUserClaimsInIdToken,RequirePkce,AllowPlainTextPkce,AllowAccessTokensViaBrowser,FrontChannelLogoutUri,FrontChannelLogoutSessionRequired,BackChannelLogoutUri,BackChannelLogoutSessionRequired,AllowOfflineAccess,IdentityTokenLifetime,AccessTokenLifetime,AuthorizationCodeLifetime,ConsentLifetime,AbsoluteRefreshTokenLifetime,SlidingRefreshTokenLifetime,RefreshTokenUsage,UpdateAccessTokenClaimsOnRefresh,RefreshTokenExpiration,AccessTokenType,EnableLocalLogin,IncludeJwtId,AlwaysSendClientClaims,ClientClaimsPrefix,PairWiseSubjectSalt,Created,Updated,LastAccessed,UserSsoLifetime,UserCodeType,DeviceCodeLifetime,NonEditable")] ClientViewModel clientViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(clientViewModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(clientViewModel);
        //}

        //// GET: Manage/Clients/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var clientViewModel = await _context.ClientViewModel.FindAsync(id);
        //    if (clientViewModel == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(clientViewModel);
        //}

        //// POST: Manage/Clients/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Enabled,ClientId,ProtocolType,RequireClientSecret,ClientName,Description,ClientUri,LogoUri,RequireConsent,AllowRememberConsent,AlwaysIncludeUserClaimsInIdToken,RequirePkce,AllowPlainTextPkce,AllowAccessTokensViaBrowser,FrontChannelLogoutUri,FrontChannelLogoutSessionRequired,BackChannelLogoutUri,BackChannelLogoutSessionRequired,AllowOfflineAccess,IdentityTokenLifetime,AccessTokenLifetime,AuthorizationCodeLifetime,ConsentLifetime,AbsoluteRefreshTokenLifetime,SlidingRefreshTokenLifetime,RefreshTokenUsage,UpdateAccessTokenClaimsOnRefresh,RefreshTokenExpiration,AccessTokenType,EnableLocalLogin,IncludeJwtId,AlwaysSendClientClaims,ClientClaimsPrefix,PairWiseSubjectSalt,Created,Updated,LastAccessed,UserSsoLifetime,UserCodeType,DeviceCodeLifetime,NonEditable")] ClientViewModel clientViewModel)
        //{
        //    if (id != clientViewModel.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(clientViewModel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ClientViewModelExists(clientViewModel.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(clientViewModel);
        //}

        //// GET: Manage/Clients/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var clientViewModel = await _context.ClientViewModel
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (clientViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(clientViewModel);
        //}

        //// POST: Manage/Clients/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var clientViewModel = await _context.ClientViewModel.FindAsync(id);
        //    _context.ClientViewModel.Remove(clientViewModel);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ClientViewModelExists(int id)
        //{
        //    return _context.ClientViewModel.Any(e => e.Id == id);
        //}
    }
}
