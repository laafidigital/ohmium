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
    public class TestStatesController : Controller
    {
        private readonly SensorContext _context;

        public TestStatesController(SensorContext context)
        {
            _context = context;
        }

        // GET: TestStates
        public async Task<IActionResult> Index()
        {
            return View(await _context.testStates.ToListAsync());
        }

        // GET: TestStates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testState = await _context.testStates
                .FirstOrDefaultAsync(m => m.id == id);
            if (testState == null)
            {
                return NotFound();
            }

            return View(testState);
        }

        // GET: TestStates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,TestName,TestDesc")] TestState testState)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testState);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testState);
        }

        // GET: TestStates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testState = await _context.testStates.FindAsync(id);
            if (testState == null)
            {
                return NotFound();
            }
            return View(testState);
        }

        // POST: TestStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,TestName,TestDesc")] TestState testState)
        {
            if (id != testState.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestStateExists(testState.id))
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
            return View(testState);
        }

        // GET: TestStates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testState = await _context.testStates
                .FirstOrDefaultAsync(m => m.id == id);
            if (testState == null)
            {
                return NotFound();
            }

            return View(testState);
        }

        // POST: TestStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testState = await _context.testStates.FindAsync(id);
            _context.testStates.Remove(testState);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestStateExists(int id)
        {
            return _context.testStates.Any(e => e.id == id);
        }
    }
}
