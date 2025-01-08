using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models;
using Ohmium.Models.TemplateModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.Templates
{
    public class SequenceLibrariesController : Controller
    {
        private readonly SensorContext _context;

        public SequenceLibrariesController(SensorContext context)
        {
            _context = context;
        }

        // GET: SequenceLibraries
        public IActionResult Index()
        {
           return View (_context.sequencyLibrary.ToList());
        }

        // GET: SequenceLibraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sequenceLibrary = await _context.sequencyLibrary.FindAsync(id);
                
            if(sequenceLibrary == null)
            {
                return NotFound();
            }

            return View(sequenceLibrary);
        }

        // GET: SequenceLibraries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SequenceLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("id,sequenceName")] SequenceLibrary sequenceLibrary)
        {
            if (ModelState.IsValid)
            {
                _context.sequencyLibrary.Add(sequenceLibrary);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(sequenceLibrary);
        }

        // GET: SequenceLibraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sequenceLibrary = await _context.sequencyLibrary.FindAsync(id);
            if (sequenceLibrary == null)
            {
                return NotFound();
            }
            return View(sequenceLibrary);
        }

        // POST: SequenceLibraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,sequenceName")] SequenceLibrary sequenceLibrary)
        {
            if (id != sequenceLibrary.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  _context.Update(sequenceLibrary);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SequenceLibraryExists(sequenceLibrary.id))
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
            return View(sequenceLibrary);
        }

        // GET: SequenceLibraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sequenceLibrary = await _context.sequencyLibrary.FindAsync(id);
            if (sequenceLibrary == null)
            {
                return NotFound();
            }

            return View(sequenceLibrary);
        }

        // POST: SequenceLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(SequenceLibrary sq)
        {
            var sequenceLibrary = _context.sequencyLibrary.Remove(sq);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SequenceLibraryExists(int? id)
        {
            SequenceLibrary sq = await _context.sequencyLibrary.FindAsync(id);
            if (sq == null)
                return false;
            else
                return true;
        }
    }
}
