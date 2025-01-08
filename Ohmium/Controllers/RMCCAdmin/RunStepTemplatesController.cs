using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class RunStepTemplatesController : Controller
    {
        private readonly SensorContext _context;

        public RunStepTemplatesController(SensorContext context)
        {
            _context = context;
        }

        // GET: RunStepTemplates
        public async Task<IActionResult> Index(int? id, int? data)
        {
            int? rsiddel = HttpContext.Session.GetInt32("rsid");
            if (rsiddel!= null)
            {
                var sc = _context.runStepTemplate.Where(e => e.stkRunProfileID == rsiddel && e.status != 2).Include(r => r.srp).Include(r => r.tState).Include(r => r.rstg).OrderBy(e => e.rstGID);
                return View(sc);
            }
            try
            {
                TempData.Peek(TempData["stackid"].ToString());
            }
            catch { }
            try
            {
                TempData.Peek(TempData["rsid"].ToString());
                int rsid = int.Parse(TempData["rsid"].ToString());
                var sensorContext5 = _context.runStepTemplate.Where(e => e.stkRunProfileID == rsid && e.status != 2).Include(r => r.srp).Include(r => r.tState).Include(r => r.rstg).OrderBy(e=>e.rstGID);
                return View(sensorContext5);
            }
            catch
            {
                if (data != null)
                {
                    ViewBag.msg = "Inactive Steps";
                    if (id != null)
                    {
                        try
                        {
                            var sensorContext1 = _context.runStepTemplate.Where(e => e.stkRunProfileID == id && e.status == 2).Include(r => r.srp).Include(r => r.tState).Include(r => r.rstg).OrderBy(e => e.rstGID);
                            return View(sensorContext1);
                        }
                        catch
                        {
                            TempData["stackid"] = id;
                            var sensorContext1 = _context.runStepTemplate.Where(e => e.status == 2).Include(r => r.srp).Include(r => r.tState).Include(r => r.rstg).OrderBy(e => e.rstGID);
                            return View(sensorContext1);
                        }
                    }
                    else
                    {
                        var sensorContext2 = _context.runStepTemplate.Where(e => e.status == 2).Include(r => r.srp).Include(r => r.tState).Include(r => r.rstg).OrderBy(e => e.rstGID).OrderBy(e => e.rstGID);
                        return View(sensorContext2);
                    }
                }
                else
                {
                    ViewBag.msg = "Active Steps";
                    if (id != null)
                    {
                        TempData["stackid"] = id;
                        var sensorContext3 = _context.runStepTemplate.Where(e => e.stkRunProfileID == id).Include(r => r.srp).Include(r => r.tState).Include(r => r.rstg).OrderBy(e => e.rstGID);
                        return View(sensorContext3);
                    }
                    else
                    {
                        var sensorContext4 = _context.runStepTemplate.Include(r => r.srp).Include(r => r.tState).Include(r => r.rstg).OrderBy(e => e.rstGID);
                        return View(sensorContext4);
                    }
                }
            }

        }

        // GET: RunStepTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepTemplate = await _context.runStepTemplate
                .Include(r => r.srp)
                .FirstOrDefaultAsync(m => m.id == id);
            if (runStepTemplate == null)
            {
                return NotFound();
            }

            return View(runStepTemplate);
        }

        // GET: RunStepTemplates/Create
        public IActionResult Create()
        {
            try
            {
                TempData.Peek(TempData["stackid"].ToString());
                int id = int.Parse(TempData["stackid"].ToString());
                ViewData["stkRunProfileID"] = new SelectList(_context.stackRunProfileTemplate.Where(e => e.id == id && e.status==1), "id", "name");
            }
            catch
            {
                try
                {
                    TempData.Peek(TempData["rsid"].ToString());
                    int rsid = int.Parse(TempData["rsid"].ToString());
                    ViewData["stkRunProfileID"] = new SelectList(_context.stackRunProfileTemplate.Where(e => e.id == rsid && e.status == 1), "id", "name");
                }
                catch
                {
                    ViewData["stkRunProfileID"] = new SelectList(_context.stackRunProfileTemplate.Where(e=>e.status == 1), "id", "name");
                }
                ViewData["stkRunProfileID"] = new SelectList(_context.stackRunProfileTemplate.Where(e=>e.status==1), "id", "name");
            }
            //ViewData["testStates"] = new SelectList(_context.testStates, "id", "TestName");
            ViewData["rstGID"] = new SelectList(_context.runStepTemplateGroup.Where(e => e.status == 1), "id", "name");
            return View();
        }

        // POST: RunStepTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,stkRunProfileID,stepNumber,sCmd,duration,cI,cV,wP,hP,wFt,wTt,cVt,cVl,imA,imF,rstGID")] RunStepTemplate runStepTemplate)
        {
            if (ModelState.IsValid)
            {
                runStepTemplate.testState = 1;
                _context.Add(runStepTemplate);
                await _context.SaveChangesAsync();
                int stkid = runStepTemplate.stkRunProfileID;
                TempData["rsid"] = stkid;
                return RedirectToAction(nameof(Index));
            }
            ViewData["stkRunProfileID"] = new SelectList(_context.stackRunProfileTemplate.Where(e=>e.status==1), "id", "name", runStepTemplate.stkRunProfileID);
            //ViewData["testStates"] = new SelectList(_context.testStates, "id", "TestName");
            ViewData["rstGID"] = new SelectList(_context.runStepTemplateGroup.Where(e=>e.status==1), "id", "name");
            return View(runStepTemplate);
        }

        // GET: RunStepTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepTemplate = await _context.runStepTemplate.FindAsync(id);
            if (runStepTemplate == null)
            {
                return NotFound();
            }
            ViewData["stkRunProfileID"] = new SelectList(_context.stackRunProfileTemplate.Where(e => e.status == 1), "id", "name", runStepTemplate.stkRunProfileID);
            //ViewData["testStates"] = new SelectList(_context.testStates, "id", "TestName");
            ViewData["rstGID"] = new SelectList(_context.runStepTemplateGroup, "id", "name");
            return View(runStepTemplate);
        }

        // POST: RunStepTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,stkRunProfileID,stepNumber,sCmd,duration,cI,cV,wP,hP,wFt,wTt,cVt,cVl,imA,imF,rstGID")] RunStepTemplate runStepTemplate)
        {
            if (id != runStepTemplate.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(runStepTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunStepTemplateExists(runStepTemplate.id))
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
            ViewData["stkRunProfileID"] = new SelectList(_context.stackRunProfileTemplate.Where(e => e.status == 1), "id", "name", runStepTemplate.stkRunProfileID);
            //ViewData["testStates"] = new SelectList(_context.testStates, "id", "TestName");
            return View(runStepTemplate);
        }

        // GET: RunStepTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepTemplate = await _context.runStepTemplate
                .Include(r => r.srp)
                .FirstOrDefaultAsync(m => m.id == id);
            if (runStepTemplate == null)
            {
                return NotFound();
            }

            return View(runStepTemplate);
        }

        // POST: RunStepTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var runStepTemplate = await _context.runStepTemplate.FindAsync(id);
            _context.runStepTemplate.Remove(runStepTemplate);
            HttpContext.Session.SetInt32("rsid", runStepTemplate.stkRunProfileID);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult NumSteps(int id,int prfid)
        {
            int val = _context.runStepTemplate.Where(x => x.rstGID == id && x.stkRunProfileID == prfid).Count();
            return Json(val);
        }
        private bool RunStepTemplateExists(int id)
        {
            return _context.runStepTemplate.Any(e => e.id == id);
        }
    }
}
