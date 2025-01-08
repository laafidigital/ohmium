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
    public class ColorConfigsController : Controller
    {
        private readonly SensorContext _context;

        public ColorConfigsController(SensorContext context)
        {
            _context = context;
        }

        // GET: ColorConfigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.colorConfig.ToListAsync());
        }

        // GET: ColorConfigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colorConfig = await _context.colorConfig
                .FirstOrDefaultAsync(m => m.id == id);
            if (colorConfig == null)
            {
                return NotFound();
            }

            return View(colorConfig);
        }

        // GET: ColorConfigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ColorConfigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,sensorName,colorCode")] ColorConfig colorConfig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colorConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colorConfig);
        }

        // GET: ColorConfigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colorConfig = await _context.colorConfig.FindAsync(id);
            if (colorConfig == null)
            {
                return NotFound();
            }
            return View(colorConfig);
        }

        // POST: ColorConfigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,sensorName,colorCode")] ColorConfig colorConfig)
        {
            if (id != colorConfig.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colorConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorConfigExists(colorConfig.id))
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
            return View(colorConfig);
        }

        // GET: ColorConfigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colorConfig = await _context.colorConfig
                .FirstOrDefaultAsync(m => m.id == id);
            if (colorConfig == null)
            {
                return NotFound();
            }

            return View(colorConfig);
        }

        // POST: ColorConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colorConfig = await _context.colorConfig.FindAsync(id);
            _context.colorConfig.Remove(colorConfig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorConfigExists(int id)
        {
            return _context.colorConfig.Any(e => e.id == id);
        }
    }
}
