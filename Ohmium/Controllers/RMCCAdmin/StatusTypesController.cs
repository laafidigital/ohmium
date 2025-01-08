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
    public class StatusTypesController : Controller
    {
        private readonly SensorContext _context;

        public StatusTypesController(SensorContext context)
        {
            _context = context;
        }

        // GET: StatusTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.statusType.ToListAsync());
        }

        // GET: StatusTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusType = await _context.statusType
                .FirstOrDefaultAsync(m => m.id == id);
            if (statusType == null)
            {
                return NotFound();
            }

            return View(statusType);
        }

        // GET: StatusTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name")] StatusType statusType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statusType);
        }

        // GET: StatusTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusType = await _context.statusType.FindAsync(id);
            if (statusType == null)
            {
                return NotFound();
            }
            return View(statusType);
        }

        // POST: StatusTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name")] StatusType statusType)
        {
            if (id != statusType.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusTypeExists(statusType.id))
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
            return View(statusType);
        }

        // GET: StatusTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusType = await _context.statusType
                .FirstOrDefaultAsync(m => m.id == id);
            if (statusType == null)
            {
                return NotFound();
            }

            return View(statusType);
        }

        // POST: StatusTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusType = await _context.statusType.FindAsync(id);
            _context.statusType.Remove(statusType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusTypeExists(int id)
        {
            return _context.statusType.Any(e => e.id == id);
        }
    }
}
