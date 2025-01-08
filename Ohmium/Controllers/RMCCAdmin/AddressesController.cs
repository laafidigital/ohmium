using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class AddressesController : Controller
    {
        private readonly SensorContext _context;

        public AddressesController(SensorContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var sensorContext = _context.address.Include(a => a.site);
            return View(await sensorContext.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.address
                .Include(a => a.site)
                .FirstOrDefaultAsync(m => m.id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create(int? id)
        {

            ViewData["sid"] = new SelectList(_context.site, "id", "name");
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,sid,address1,address2,address3,postalCode,city,state,country")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["sid"] = new SelectList(_context.site, "id", "name", address.sid);
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            ViewData["sid"] = new SelectList(_context.site, "id", "email", address.sid);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,sid,address1,address2,address3,postalCode,city,state,country")] Address address)
        {
            if (id != address.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.id))
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
            ViewData["sid"] = new SelectList(_context.site, "id", "email", address.sid);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.address
                .Include(a => a.site)
                .FirstOrDefaultAsync(m => m.id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.address.FindAsync(id);
            _context.address.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(Guid id)
        {
            return _context.address.Any(e => e.id == id);
        }
    }
}
