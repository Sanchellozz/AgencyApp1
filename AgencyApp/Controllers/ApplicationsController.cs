#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgencyApp.Data;
using AgencyApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AgencyApp.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly AgencyDBContext _context;
        UserManager<User> _userManager;


        public ApplicationsController(AgencyDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;

        }


        // GET: Applications
        public async Task<IActionResult> Index()
        {
            if (@User.IsInRole("agent"))
            {
                var agencyDBContext = _context.Application.Include(a => a.Client).Include(a => a.Dictionary);
                return View(await agencyDBContext.ToListAsync());
            }
            else {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                string userId = currentUser.Id.ToString();
                var applicationsview = _context.Application.Include(a => a.Client).Include(a => a.Dictionary).Where(c => c.Client.UserID == userId);
                return View(applicationsview);
            }
        }

        // GET: Applications/Details/5
        [Authorize(Roles = "agent")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.Client)
                .Include(a => a.Dictionary)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create

        public async Task<IActionResult> CreateAsync()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            string userId = currentUser.Id.ToString();
            ViewData["ClientName"] = new SelectList(_context.Clients.Where(c => c.UserID == userId), "Id", "Name");
            ViewData["DictionaryName"] = new SelectList(_context.Dictionary, "Id", "Name");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplicationId,DictionaryId,ClientId,Telephone")] Application application)
        {
            application.Status = "Рассматривается";
            _context.Add(application);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["ClientName"] = new SelectList(user.Email);
            ViewData["DictionaryId"] = new SelectList(_context.Dictionary, "Id", "Id", application.DictionaryId);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Applications/Edit/5
        [Authorize(Roles = "agent")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Application.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "agent")]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,DictionaryId,ClientId,Telephone,Status")] Application application)
        {

            if (id != application.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.ApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        // GET: Applications/Delete/5
        [Authorize(Roles = "agent")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.Client)
                .Include(a => a.Dictionary)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "agent")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Application.FindAsync(id);
            _context.Application.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
            return _context.Application.Any(e => e.ApplicationId == id);
        }
    }
}
