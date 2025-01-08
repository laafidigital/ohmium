using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.TemplateModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.Templates
{
    public class ScriptListsController : Controller
    {
        private readonly SensorContext _context;

        public ScriptListsController(SensorContext context)
        {
            _context = context;
        }

        // GET: ScriptLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.scriptlists.ToListAsync());
        }

        // GET: ScriptLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptList = await _context.scriptlists
                .FirstOrDefaultAsync(m => m.id == id);
            if (scriptList == null)
            {
                return NotFound();
            }

            return View(scriptList);
        }

        // GET: ScriptLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScriptLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,scriptName")] ScriptList scriptList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scriptList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scriptList);
        }

        // GET: ScriptLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptList = await _context.scriptlists.FindAsync(id);
            if (scriptList == null)
            {
                return NotFound();
            }
            return View(scriptList);
        }

        // POST: ScriptLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,scriptName")] ScriptList scriptList)
        {
            if (id != scriptList.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scriptList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScriptListExists(scriptList.id))
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
            return View(scriptList);
        }

        // GET: ScriptLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptList = await _context.scriptlists
                .FirstOrDefaultAsync(m => m.id == id);
            if (scriptList == null)
            {
                return NotFound();
            }

            return View(scriptList);
        }

        // POST: ScriptLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scriptList = await _context.scriptlists.FindAsync(id);
            _context.scriptlists.Remove(scriptList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScriptListExists(int id)
        {
            return _context.scriptlists.Any(e => e.id == id);
        }
    }
}
