using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models;
using Ohmium.Models.EFModels;

namespace Ohmium.Controllers.SQLite
{
    public class SQRegionsController : Controller
    {
        private readonly CacheContext _context;

        public SQRegionsController(CacheContext context)
        {
            _context = context;
        }

        // GET: SQRegions
        public async Task<IActionResult> Index()
        {
            return View(await _context.region.ToListAsync());
        }

        // GET: SQRegions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.region
                .FirstOrDefaultAsync(m => m.name == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // GET: SQRegions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SQRegions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,desc")] Region region)
        {
            if (ModelState.IsValid)
            {
                _context.Add(region);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(region);
        }

        // GET: SQRegions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.region.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            return View(region);
        }

        // POST: SQRegions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("name,desc")] Region region)
        {
            if (id != region.name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(region);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegionExists(region.name))
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
            return View(region);
        }

        // GET: SQRegions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.region
                .FirstOrDefaultAsync(m => m.name == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // POST: SQRegions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var region = await _context.region.FindAsync(id);
            _context.region.Remove(region);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegionExists(string id)
        {
            return _context.region.Any(e => e.name == id);
        }
    }
}
