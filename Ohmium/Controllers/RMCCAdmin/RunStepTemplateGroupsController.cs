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
    public class RunStepTemplateGroupsController : Controller
    {
        private readonly SensorContext _context;

        public RunStepTemplateGroupsController(SensorContext context)
        {
            _context = context;
        }

        // GET: RunStepTemplateGroups
        public async Task<IActionResult> Index(int? data)
        {
            if (data == 3)
            {
                ViewBag.status = "InActive Sequences";
                return View(await _context.runStepTemplateGroup.Where(e => e.status == 3).ToListAsync());
            }
            else
                return View(await _context.runStepTemplateGroup.Where(e => e.status == 1).ToListAsync());
        }

        // GET: RunStepTemplateGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepTemplateGroup = await _context.runStepTemplateGroup
                .FirstOrDefaultAsync(m => m.id == id);
            if (runStepTemplateGroup == null)
            {
                return NotFound();
            }

            return View(runStepTemplateGroup);
        }

        // GET: RunStepTemplateGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RunStepTemplateGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,numLoops")] RunStepTemplateGroup runStepTemplateGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(runStepTemplateGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(runStepTemplateGroup);
        }

        // GET: RunStepTemplateGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepTemplateGroup = await _context.runStepTemplateGroup.FindAsync(id);
            if (runStepTemplateGroup == null)
            {
                return NotFound();
            }
            return View(runStepTemplateGroup);
        }

        // POST: RunStepTemplateGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,numLoops")] RunStepTemplateGroup runStepTemplateGroup)
        {
            if (id != runStepTemplateGroup.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    runStepTemplateGroup.status = int.Parse(Request.Form["status"].ToString());

                    _context.Update(runStepTemplateGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunStepTemplateGroupExists(runStepTemplateGroup.id))
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
            return View(runStepTemplateGroup);
        }

        // GET: RunStepTemplateGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepTemplateGroup = await _context.runStepTemplateGroup
                .FirstOrDefaultAsync(m => m.id == id);
            if (runStepTemplateGroup == null)
            {
                return NotFound();
            }

            return View(runStepTemplateGroup);
        }

        // POST: RunStepTemplateGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var runStepTemplateGroup = await _context.runStepTemplateGroup.FindAsync(id);
            runStepTemplateGroup.status = 3;
            _context.Entry(runStepTemplateGroup).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RunStepTemplateGroupExists(int id)
        {
            return _context.runStepTemplateGroup.Any(e => e.id == id);
        }
    }
}
