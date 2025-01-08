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
    public class RunStepsController : Controller
    {
        private readonly SensorContext _context;

        public RunStepsController(SensorContext context)
        {
            _context = context;
        }

        // GET: RunSteps
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var sc = _context.stkStep.Include(r => r.srp).Include(r=>r.rstg);
                return View(sc);
            }

            var sensorContext = _context.stkStep.Include(r => r.srp).Include(e=>e.rstg).Where(r => r.stkRunProfileID == id);
                    
            ViewData["stkRunProfile"] = _context.stkRunProfile.Include(x=>x.stk).Single(x=>x.id==id);
            int pid = _context.stkRunProfile.Single(x => x.id==id).profileID;
            ViewData["runProfile"] = _context.runProfile.Find(pid);
            return View(await sensorContext.ToListAsync());
        }

        // GET: RunSteps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStep = await _context.stkStep
                .Include(r => r.srp)
                .FirstOrDefaultAsync(m => m.id == id);
            if (runStep == null)
            {
                return NotFound();
            }

            return View(runStep);
        }

        // GET: RunSteps/Create
        public IActionResult Create(int id)
        {
            //ViewData["stkRunProfileID"] = new SelectList(_context.stkRunProfile.Distinct().ToList(), "id", "name");
            ViewData["stkrunProfileID"] = _context.stkRunProfile.Single(x=>x.id==id).name;
            ViewData["rstGID"] = new SelectList(_context.runStepTemplateGroup, "id", "name");
            ViewData["prfID"] = id; 
            int count = _context.stkStep.Where(x => x.stkRunProfileID==id).Count() + 1;
            TempData["stkRnPfID"] = count;


            ViewData["testStates"] = new SelectList(_context.testStates, "id", "TestName");
            return View();
        }

        //public JsonResult GetRunStep(int rpid)
        //{
        //    int count = _context.stkStep.Where(x => x.stkRunProfileID == rpid).Count()+1;
        //    TempData["runStepNo"] = count;
        //    return Json(count);
        //}

        // POST: RunSteps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("sCmd,stkRunProfileID,stepNumber,duration,cI,cV,wP,hP,wFt,wTt,cVt,cVl,testState,mnF,mxF,imF,imA")] RunStep runStep)
        {

            if (ModelState.IsValid)
            {
                _context.Add(runStep);
                await _context.SaveChangesAsync();
                StackRunProfile srp = _context.stkRunProfile.Single(x => x.id == runStep.stkRunProfileID);
                return RedirectToAction(nameof(Index),srp);
            }
            ViewData["stkRunProfileID"] = new SelectList(_context.stkRunProfile, "id", "name", runStep.stkRunProfileID);
            ViewData["testStates"] = new SelectList(_context.testStates, "id", "TestName");
            return View(runStep);
        }

        // GET: RunSteps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStep = await _context.stkStep.FindAsync(id);
            if (runStep == null)
            {
                return NotFound();
            }
            ViewData["stkRunProfileID"] = new SelectList(_context.stkRunProfile, "id", "name", runStep.stkRunProfileID);
            ViewData["testStates"] = new SelectList(_context.testStates, "id", "TestName",runStep.testState);
            ViewData["rstGID"] = new SelectList(_context.runStepTemplateGroup, "id", "name", runStep.testState);
            return View(runStep);
        }

        // POST: RunSteps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,stkRunProfileID,stepNumber,sCmd,duration,cI,cV,wP,hP,wFt,wTt,cVt,cVl,testState,imA,imF,rstGID")] RunStep runStep)
        {
            if (id != runStep.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(runStep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunStepExists(runStep.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StackRunProfile srp = _context.stkRunProfile.Single(x => x.id == runStep.stkRunProfileID);
                return RedirectToAction(nameof(Index),srp);
            }
            ViewData["stkRunProfileID"] = new SelectList(_context.stkRunProfile, "id", "name", runStep.stkRunProfileID);
            return View(runStep);
        }

        // GET: RunSteps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStep = await _context.stkStep
                .Include(r => r.srp)
                .FirstOrDefaultAsync(m => m.id == id);
            if (runStep == null)
            {
                return NotFound();
            }

            return View(runStep);
        }

        // POST: RunSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var runStep = await _context.stkStep.FindAsync(id);
            _context.stkStep.Remove(runStep);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RunStepExists(int id)
        {
            return _context.stkStep.Any(e => e.id == id);
        }
    }
}
