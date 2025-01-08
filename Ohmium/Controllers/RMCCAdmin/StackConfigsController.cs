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
    public class StackConfigsController : Controller
    {
        private readonly SensorContext _context;

        public StackConfigsController(SensorContext context)
        {
            _context = context;
        }

        // GET: StackConfigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.sconfig.ToListAsync());
        }

        // GET: StackConfigs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackConfig = await _context.sconfig
                .FirstOrDefaultAsync(m => m.configID == id);
            if (stackConfig == null)
            {
                return NotFound();
            }

            return View(stackConfig);
        }

        // GET: StackConfigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StackConfigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("configID,configName,configString,colorConfig")] StackConfig stackConfig)
        {
            if (ModelState.IsValid)
            {
                stackConfig.configID = Guid.NewGuid();
                _context.Add(stackConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stackConfig);
        }

        // GET: StackConfigs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackConfig = await _context.sconfig.FindAsync(id);
            if (stackConfig == null)
            {
                return NotFound();
            }
            return View(stackConfig);
        }

        // POST: StackConfigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("configID,configName,configString,colorConfig")] StackConfig stackConfig)
        {
            //if (id != stackConfig.configID)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stackConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StackConfigExists(stackConfig.configID))
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
            return View(stackConfig);
        }

        // GET: StackConfigs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackConfig = await _context.sconfig
                .FirstOrDefaultAsync(m => m.configID == id);
            if (stackConfig == null)
            {
                return NotFound();
            }

            return View(stackConfig);
        }

        // POST: StackConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var stackConfig = await _context.sconfig.FindAsync(id);
            _context.sconfig.Remove(stackConfig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StackConfigExists(Guid id)
        {
            return _context.sconfig.Any(e => e.configID == id);
        }
    }
}
