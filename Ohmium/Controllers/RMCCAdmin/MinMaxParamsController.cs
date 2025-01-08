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
    public class MinMaxParamsController : Controller
    {
        private readonly SensorContext _context;

        public MinMaxParamsController(SensorContext context)
        {
            _context = context;
        }

        // GET: MinMaxParams
        public async Task<IActionResult> Index()
        {
            return View(await _context.mmp.ToListAsync());
        }

        // GET: MinMaxParams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var minMaxParams = await _context.mmp
                .FirstOrDefaultAsync(m => m.id == id);
            if (minMaxParams == null)
            {
                return NotFound();
            }

            return View(minMaxParams);
        }

        // GET: MinMaxParams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MinMaxParams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,equipment,sensorName,min,max")] MinMaxParams minMaxParams)
        {
            if (ModelState.IsValid)
            {
                _context.Add(minMaxParams);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(minMaxParams);
        }

        // GET: MinMaxParams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var minMaxParams = await _context.mmp.FindAsync(id);
            if (minMaxParams == null)
            {
                return NotFound();
            }
            return View(minMaxParams);
        }

        // POST: MinMaxParams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,equipment,sensorName,min,max")] MinMaxParams minMaxParams)
        {
            if (id != minMaxParams.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(minMaxParams);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MinMaxParamsExists(minMaxParams.id))
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
            return View(minMaxParams);
        }

        // GET: MinMaxParams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var minMaxParams = await _context.mmp
                .FirstOrDefaultAsync(m => m.id == id);
            if (minMaxParams == null)
            {
                return NotFound();
            }

            return View(minMaxParams);
        }

        // POST: MinMaxParams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var minMaxParams = await _context.mmp.FindAsync(id);
            _context.mmp.Remove(minMaxParams);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MinMaxParamsExists(int id)
        {
            return _context.mmp.Any(e => e.id == id);
        }
    }
}
