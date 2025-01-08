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
    public class SegmentsController : Controller
    {
        private readonly SensorContext _context;

        public SegmentsController(SensorContext context)
        {
            _context = context;
        }

        // GET: Segments
        public async Task<IActionResult> Index()
        {
            return View(await _context.segment.ToListAsync());
        }

        // GET: Segments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segment = await _context.segment
                .FirstOrDefaultAsync(m => m.id == id);
            if (segment == null)
            {
                return NotFound();
            }

            return View(segment);
        }

        // GET: Segments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Segments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,type")] Segment segment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(segment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(segment);
        }

        // GET: Segments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segment = await _context.segment.FindAsync(id);
            if (segment == null)
            {
                return NotFound();
            }
            return View(segment);
        }

        // POST: Segments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,name,type")] Segment segment)
        {
            if (id != segment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(segment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SegmentExists(segment.id))
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
            return View(segment);
        }

        // GET: Segments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segment = await _context.segment
                .FirstOrDefaultAsync(m => m.id == id);
            if (segment == null)
            {
                return NotFound();
            }

            return View(segment);
        }

        // POST: Segments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var segment = await _context.segment.FindAsync(id);
            _context.segment.Remove(segment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SegmentExists(Guid id)
        {
            return _context.segment.Any(e => e.id == id);
        }
    }
}
