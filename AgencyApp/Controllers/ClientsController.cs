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
    [Authorize(Roles = "agent")]
    public class ClientsController : Controller
    {
        private readonly AgencyDBContext _context;
        private readonly ILogger _logger;
        UserManager<User> _userManager;

        public ClientsController(AgencyDBContext context, ILogger<ClientsController> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var clients = _context.Clients.Include(c => c.License);
            return View(await clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id, string? licensename)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            var group = await _context.Licenses
                .FirstOrDefaultAsync(m => m.Id == client.LicenseId);
            if (group == null)
            { return NotFound(); }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name");
            ViewData["UserID"] = new SelectList(_userManager.Users, "Id", "UserName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LicenseId,Passport,Id,Name,UserID")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Был создан клиент");
                return RedirectToAction(nameof(Index));

            }
            ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Id", client.LicenseId);
            ViewData["UserID"] = new SelectList(_userManager.Users, "Id", "Id", client.UserID);
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name");
            ViewData["UserID"] = new SelectList(_userManager.Users, "Id", "UserName");
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Id", client.LicenseId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LicenseId,Passport,Id,Name,UserID")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
