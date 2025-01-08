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
    public class RunProfileTemplatesController : Controller
    {
        private readonly SensorContext _context;

        public RunProfileTemplatesController(SensorContext context)
        {
            _context = context;
        }

        // GET: RunProfileTemplates
        public async Task<IActionResult> Index(int? data)
        {
            if (data != null)
            {
                ViewBag.msg = "Inactive Run Profile Templates";
                return View(await _context.runProfileTemplate.Where(d => d.status == 2).Include(d=>d.stt).ToListAsync());
            }
            else
            {
                try
                {
                    ViewBag.msg = "Active Run Profile Templates";
                    return View(await _context.runProfileTemplate.Where(d=>d.status!=2).ToListAsync());
                }
                catch
                {
                    ViewBag.msg = "Active Run Profile Templates";
                    return View(await _context.runProfileTemplate.Where(d => d.status != 2).ToListAsync());
                }
            }
        }

        // GET: RunProfileTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runProfileTemplate = await _context.runProfileTemplate
                .FirstOrDefaultAsync(m => m.id == id);
            if (runProfileTemplate == null)
            {
                return NotFound();
            }

            return View(runProfileTemplate);
        }

        // GET: RunProfileTemplates/Create
        public IActionResult Create()
        {
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View();
        }

        // POST: RunProfileTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,profileName,dCmd,fan,pump,mnWbT,mxWbT,hxT,status")] RunProfileTemplate runProfileTemplate)
        {
            if (ModelState.IsValid)
            {
                runProfileTemplate.timeStamp = DateTime.Now;
                _context.Add(runProfileTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(runProfileTemplate);
        }

        // GET: RunProfileTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            if (id == null)
            {
                return NotFound();
            }

            var runProfileTemplate = await _context.runProfileTemplate.FindAsync(id);
            if (runProfileTemplate == null)
            {
                return NotFound();
            }
            return View(runProfileTemplate);
        }

        // POST: RunProfileTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,profileName,dCmd,timeStamp,fan,pump,mnWbT,mxWbT,hxT,status")] RunProfileTemplate runProfileTemplate)
        {
            if (id != runProfileTemplate.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    runProfileTemplate.timeStamp = DateTime.Now;
                    _context.Update(runProfileTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunProfileTemplateExists(runProfileTemplate.id))
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
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(runProfileTemplate);
        }

        // GET: RunProfileTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var runProfileTemplate = await _context.runProfileTemplate
                .FirstOrDefaultAsync(m => m.id == id);
            if (runProfileTemplate == null)
            {
                return NotFound();
            }

            return View(runProfileTemplate);
        }

        // POST: RunProfileTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RunProfileTemplate rpt = _context.runProfileTemplate.Find(id);
            rpt.status = 2;
            _context.Entry(rpt).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RunProfileTemplateExists(int id)
        {
            return _context.runProfileTemplate.Any(e => e.id == id);
        }
    }
}
