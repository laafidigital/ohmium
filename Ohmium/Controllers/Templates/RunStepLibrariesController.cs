using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models;
using Ohmium.Models.TemplateModels;
using Ohmium.Repository;



namespace Ohmium.Controllers.Templates
{
    public class RunStepLibrariesController : Controller
    {
        private readonly SensorContext _context;

        public RunStepLibrariesController(SensorContext context)
        {
            _context = context;
        }

        public JsonResult RunSteps(int id)
        {
            List<RunStepLibrary> rsl = _context.runstepLibrary.Where(e => e.seqMasterId == id).Include(e=>e.seqmaster).OrderBy(e=>e.stepNumber).ToList();
            StringBuilder response = new StringBuilder("<table class='table table-striped'><tr><th>SEQUENCE</th><th>RUN STEP NO</th><th>DURATION</th><th>CI</th><th>CV</th><th>WP</th><th>HP</th><th>WFT</th><th>WTT</th><th>CVT</th><th>cVLimit</th><th>IMF</th><th>IMA</th><th colspan='2'>ACTION</th></tr>");
foreach(RunStepLibrary runstep in rsl)
            {
                response.Append($"<tr><td>{runstep.seqmaster.sequenceName}</td><td>{runstep.stepNumber}</td><td>{runstep.duration}</td><td>{runstep.cI}</td><td>{runstep.cV}</td><td>{runstep.wP}</td><td>{runstep.hP}</td><td>{runstep.wFt}</td><td>{runstep.wTt}</td><td>{runstep.cVt}</td><td>{runstep.cVlimit}</td><td>{runstep.imF}</td><td>{runstep.imA}</td><td><a href= RunStepLibraries/edit?id={runstep.id}><i class='fa fa-edit text-warning'></i></a></td><td><a href= RunStepLibraries/delete?id={runstep.id}><i class='fa fa-trash text-danger'></i></a></td></tr>");
            }
            response.Append("</table>");
            return Json(response.ToString());
        }

        // GET: RunStepLibraries
        public async Task<IActionResult> Index(int? id)
        {

            ViewBag.sequences = new SelectList(_context.sequencyLibrary,"id", "sequenceName");
            IEnumerable<RunStepLibrary> cacheContext = new List<RunStepLibrary>();
            if(id == null)
                cacheContext= await _context.runstepLibrary.Include(e => e.seqmaster).ToListAsync();
            else
                cacheContext = await _context.runstepLibrary.Where(e=>e.seqMasterId==id).Include(e=>e.seqmaster).ToListAsync();
            return View(cacheContext);
        }

        //GET: RunStepLibraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepLibrary = await _context.runstepLibrary.FindAsync(id);
            if (runStepLibrary == null)
            {
                return NotFound();
            }

            return View(runStepLibrary);
        }

        public JsonResult GetRunStep(int rpid)
        {
            int count = _context.runstepLibrary.Where(e=>e.seqMasterId==rpid).Count()+1;
            return Json(count);
        }

        public JsonResult GetAllSteps(int rpid)
        {
            List<RunStepLibrary> steps = _context.runstepLibrary.Where(e => e.seqMasterId == rpid).Include(e=>e.seqmaster).ToList();
            StringBuilder steplist = new StringBuilder("<table class='table table-striped'><tr><th>Seq Name</th><th>RUN STEP NO</th><th>Duration</th><th>CI</th><th>CV</th><th>WP</th><th>HP</th><th>WFT</th><th>WTT</th><th>CVT</th><th>cVLimit</th><th>MNF</th><th>MXF</th><th>IMF</th><th>IMA</th></tr>");
            foreach(RunStepLibrary rs in steps)
            {
                steplist.Append("<tr>" +
                      "<td>" +
                    rs.seqmaster.sequenceName +
                    "</td>" +
                    "<td>" +
                    rs.stepNumber +
                    "</td>" +
                     "<td>" +
                    rs.duration +
                    "</td>" +
                     "<td>" +
                    rs.cI +
                    "</td>" +
                     "<td>" +
                    rs.cV +
                    "</td>" +
                     "<td>" +
                    rs.wP +
                    "</td>" +
                     "<td>" +
                    rs.hP +
                    "</td>" +
                     "<td>" +
                    rs.wFt +
                    "</td>" +
                     "<td>" +
                    rs.wTt +
                    "</td>" +
                     "<td>" +
                    rs.cVt +
                    "</td>" +
                       "<td>" +
                    rs.cVlimit +
                    "</td>" +
                     "<td>" +
                    rs.mnF +
                    "</td>" +
                     "<td>" +
                    rs.mxF +
                    "</td>" +
                     "<td>" +
                    rs.imF +
                    "</td>" +
                     "<td>" +
                    rs.imA +
                    "</td>" +
                    "</tr>");
            }
            steplist.Append("</table>");
            string sl = steplist.ToString();
            return Json(sl);
        }

        // GET: RunStepLibraries/Create
        public async Task<IActionResult> Create()
        {
            ViewData["seqMasterId"] = new SelectList(_context.sequencyLibrary, "id", "sequenceName");
            return View();
        }

        // POST: RunStepLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,seqMasterId,stepNumber,duration,cI,cV,wP,hP,wFt,wTt,cVt,cVlimit,mnF,mxF,imF,imA")] RunStepLibrary runStepLibrary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(runStepLibrary);
                await _context.SaveChangesAsync();
                ViewData["seqMasterId"] = new SelectList(_context.sequencyLibrary, "id", "sequenceName", runStepLibrary.seqMasterId);
                return View(runStepLibrary);
            }
            ViewData["seqMasterId"] = new SelectList(_context.sequencyLibrary, "id", "sequenceName", runStepLibrary.seqMasterId);
            return View();
        }

        // GET: RunStepLibraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepLibrary = await _context.runstepLibrary.FindAsync(id);
            if (runStepLibrary == null)
            {
                return NotFound();
            }
            ViewData["seqMasterId"] = new SelectList(_context.sequencyLibrary, "id", "sequenceName", runStepLibrary.seqMasterId);
            return View(runStepLibrary);
        }

        // POST: RunStepLibraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,seqMasterId,stepNumber,duration,cI,cV,wP,hP,wFt,wTt,cVt,cVlimit,mnF,mxF,imF,imA")] RunStepLibrary runStepLibrary)
        {
            if (id != runStepLibrary.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(runStepLibrary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunStepLibraryExists(runStepLibrary.id))
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
            ViewData["seqMasterId"] = new SelectList(_context.sequencyLibrary, "id", "sequenceName", runStepLibrary.seqMasterId);
            return View(runStepLibrary);
        }

        // GET: RunStepLibraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runStepLibrary = await _context.runstepLibrary
                .Include(r => r.seqmaster)
                .FirstOrDefaultAsync(m => m.id == id);
            if (runStepLibrary == null)
            {
                return NotFound();
            }

            return View(runStepLibrary);
        }

        // POST: RunStepLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var runStepLibrary = await _context.runstepLibrary.FindAsync(id);
            _context.runstepLibrary.Remove(runStepLibrary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RunStepLibraryExists(int id)
        {
            return _context.runstepLibrary.Any(e => e.id == id);
        }
    }
}
