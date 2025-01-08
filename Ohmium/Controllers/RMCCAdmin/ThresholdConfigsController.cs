using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class ThresholdConfigsController : Controller
    {
        private readonly SensorContext _context;

        public ThresholdConfigsController(SensorContext context)
        {
            _context = context;
        }

        // GET: ThresholdConfigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.thresholdconfigs.ToListAsync());
        }

        // GET: ThresholdConfigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thresholdConfig = await _context.thresholdconfigs
                .FirstOrDefaultAsync(m => m.id == id);
            if (thresholdConfig == null)
            {
                return NotFound();
            }

            return View(thresholdConfig);
        }

        // GET: ThresholdConfigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ThresholdConfigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,paramName,minVal,maxVal,colorSortOrder")] ThresholdConfig thresholdConfig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thresholdConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thresholdConfig);
        }

        // GET: ThresholdConfigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thresholdConfig = await _context.thresholdconfigs.FindAsync(id);
            if (thresholdConfig == null)
            {
                return NotFound();
            }
            return View(thresholdConfig);
        }

        // POST: ThresholdConfigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,paramName,minVal,maxVal,colorSortOrder")] ThresholdConfig thresholdConfig)
        {
            if (id != thresholdConfig.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thresholdConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThresholdConfigExists(thresholdConfig.id))
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
            return View(thresholdConfig);
        }

        // GET: ThresholdConfigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thresholdConfig = await _context.thresholdconfigs
                .FirstOrDefaultAsync(m => m.id == id);
            if (thresholdConfig == null)
            {
                return NotFound();
            }

            return View(thresholdConfig);
        }

        // POST: ThresholdConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thresholdConfig = await _context.thresholdconfigs.FindAsync(id);
            _context.thresholdconfigs.Remove(thresholdConfig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThresholdConfigExists(int id)
        {
            return _context.thresholdconfigs.Any(e => e.id == id);
        }
    }
}
