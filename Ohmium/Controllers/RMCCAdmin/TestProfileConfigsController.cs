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
    public class TestProfileConfigsController : Controller
    {
        private readonly SensorContext _context;

        public TestProfileConfigsController(SensorContext context)
        {
            _context = context;
        }

        // GET: TestProfileConfigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.testProfileConfig.ToListAsync());
        }

        // GET: TestProfileConfigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testProfileConfig = await _context.testProfileConfig
                .FirstOrDefaultAsync(m => m.id == id);
            if (testProfileConfig == null)
            {
                return NotFound();
            }

            return View(testProfileConfig);
        }

        // GET: TestProfileConfigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestProfileConfigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Config")] TestProfileConfig testProfileConfig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testProfileConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testProfileConfig);
        }

        // GET: TestProfileConfigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testProfileConfig = await _context.testProfileConfig.FindAsync(id);
            if (testProfileConfig == null)
            {
                return NotFound();
            }
            return View(testProfileConfig);
        }

        // POST: TestProfileConfigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Config")] TestProfileConfig testProfileConfig)
        {
            if (id != testProfileConfig.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testProfileConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestProfileConfigExists(testProfileConfig.id))
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
            return View(testProfileConfig);
        }

        // GET: TestProfileConfigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testProfileConfig = await _context.testProfileConfig
                .FirstOrDefaultAsync(m => m.id == id);
            if (testProfileConfig == null)
            {
                return NotFound();
            }

            return View(testProfileConfig);
        }

        // POST: TestProfileConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testProfileConfig = await _context.testProfileConfig.FindAsync(id);
            _context.testProfileConfig.Remove(testProfileConfig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestProfileConfigExists(int id)
        {
            return _context.testProfileConfig.Any(e => e.id == id);
        }
    }
}
