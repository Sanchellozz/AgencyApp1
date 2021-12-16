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

namespace AgencyApp.Controllers
{
    public class ContractsController : Controller
    {
        private readonly AgencyDBContext _context;

        public ContractsController(AgencyDBContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.ClientNameSortParm = sortOrder == "ClientName" ? "ClientName_desc" : "ClientName";
            ViewBag.AgentNameSortParm = sortOrder == "AgentName" ? "AgentName_desc" : "AgentName";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.IdSortParm = sortOrder == "Id" ? "Id_desc" : "Id";
            ViewBag.DicSortParm = sortOrder == "Dic" ? "Dic_desc" : "Dic";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            var contracts = from s in _context.Contract.Include(c => c.Agent).Include(c => c.Client).Include(c => c.Dictionary)
            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                contracts = contracts.Where(s => s.Client.Name.Contains(searchString)
                                       || s.Agent.Name.Contains(searchString)
                                       || s.Dictionary.Price.Equals(searchString)
                                       || s.Dictionary.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "ClientName":
                    contracts = contracts.OrderBy(s => s.Client.Name);
                    break;
                case "ClientName_desc":
                    contracts = contracts.OrderByDescending(s => s.Client.Name);
                    break;
                case "AgentName":
                    contracts = contracts.OrderBy(s => s.Agent.Name);
                    break;
                case "AgentName_desc":
                    contracts = contracts.OrderByDescending(s => s.Agent.Name);
                    break;
                case "Date":
                    contracts = contracts.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    contracts = contracts.OrderByDescending(s => s.Date);
                    break;
                case "Id":
                    contracts = contracts.OrderBy(s => s.ContractId);
                    break;
                case "Id_desc":
                    contracts = contracts.OrderByDescending(s => s.ContractId);
                    break;
                case "Dic":
                    contracts = contracts.OrderBy(s => s.Dictionary.Name);
                    break;
                case "Dic_desc":
                    contracts = contracts.OrderByDescending(s => s.Dictionary.Name);
                    break;
                case "Price":
                    contracts = contracts.OrderBy(s => s.Dictionary.Price);
                    break;
                case "Price_desc":
                    contracts = contracts.OrderByDescending(s => s.Dictionary.Price);
                    break;
                default:
                    contracts = contracts.OrderBy(s => s.ContractId);
                    break;
            }
            return View(contracts.ToList());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .Include(c => c.Agent)
                .Include(c => c.Client)
                .Include(c => c.Dictionary)
                .FirstOrDefaultAsync(m => m.ContractId == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewData["AgentName"] = new SelectList(_context.Agents, "Id", "Name");
            ViewData["ClientName"] = new SelectList(_context.Clients, "Id", "Name");
            ViewData["DictionaryName"] = new SelectList(_context.Dictionary, "Id", "Name");
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,ClientId,AgentId,DictionaryId,Date")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AgentId"] = new SelectList(_context.Agents, "Id", "Id", contract.AgentId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", contract.ClientId);
            ViewData["DictionaryId"] = new SelectList(_context.Dictionary, "Id", "Id", contract.DictionaryId);
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewData["AgentId"] = new SelectList(_context.Agents, "Id", "Id", contract.AgentId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", contract.ClientId);
            ViewData["DictionaryId"] = new SelectList(_context.Dictionary, "Id", "Id", contract.DictionaryId);
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContractId,ClientId,AgentId,DictionaryId,Date")] Contract contract)
        {
            if (id != contract.ContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.ContractId))
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
            ViewData["AgentId"] = new SelectList(_context.Agents, "Id", "Id", contract.AgentId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", contract.ClientId);
            ViewData["DictionaryId"] = new SelectList(_context.Dictionary, "Id", "Id", contract.DictionaryId);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .Include(c => c.Agent)
                .Include(c => c.Client)
                .Include(c => c.Dictionary)
                .FirstOrDefaultAsync(m => m.ContractId == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contract.FindAsync(id);
            _context.Contract.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
            return _context.Contract.Any(e => e.ContractId == id);
        }
    }
}
