using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels.ViewModels;
using Ohmium.Models.TemplateModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.Templates
{
    public class ScriptLibrariesController : Controller
    {
        private readonly SensorContext _context;

        public ScriptLibrariesController(SensorContext context)
        {
            _context = context;
        }

        // GET: ScriptLibraries
        public async Task<IActionResult> Index()
        {
            List<ScriptLibrary> sensorContext = new List<ScriptLibrary>();
            try
            {
                sensorContext = _context.scriptLibrary.Include(s => s.script).OrderBy(e => e.scriptId).ThenBy(e => e.stepNumber).ToList();
                ViewBag.scripts = new SelectList(_context.scriptlists, "id", "scriptName");

                return View(sensorContext.ToList());
            }
            catch
            {
                ViewBag.scripts = new SelectList(_context.scriptlists, "id", "scriptName");

                return View(sensorContext.ToList());
            }
        }

        public async Task<IActionResult> GetScripts(string stkmfgid)
        {
            StackSynceViewModel ssvm = new StackSynceViewModel();
            StackSync ss = _context.stackSyncData.FirstOrDefault(e => e.stackID == stkmfgid);
           ssvm.scriptLib= _context.scriptLibrary.Where(e => e.scriptId == ss.scriptID).Include(e => e.script).ToList();
            ssvm.stk = _context.stack.FirstOrDefault(e => e.stackMfgID == ss.stackID);
            return View(ssvm);
        }
        public JsonResult ScriptSteps(int id)
        {
            List<ScriptLibrary> rsl = _context.scriptLibrary.Where(e => e.scriptId == id).Include(e => e.script).OrderBy(e => e.stepNumber).ToList();
            StringBuilder response = new StringBuilder("<table class='table table-striped'><tr><th>Script</th><th>Script Library Step No</th><th>Script Loop</th><th>Sequence Steps & its loops</th><th colspan='2'>Action</th></tr>");
            foreach (ScriptLibrary sl in rsl)
            {
                response.Append($"<tr><td>{sl.script.scriptName}</td><td>{sl.stepNumber}</td><td>{sl.phaseLoop}</td><td>{sl.runStepLibraryWithLoop}</td><td><a href= ScriptLibraries/EditNew?id={sl.id}><i class='fa fa-edit text-warning'></i></a></td><td><a href= ScriptLibraries/delete?id={sl.id}><i class='fa fa-trash text-danger'></i></a></td></tr>");
            }
            response.Append("</table>");
            return Json(response.ToString());
        }
        // GET: ScriptLibraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptLibrary = await _context.scriptLibrary
                .Include(s => s.script)
                .FirstOrDefaultAsync(m => m.id == id);
            if (scriptLibrary == null)
            {
                return NotFound();
            }

            return View(scriptLibrary);
        }

        public JsonResult GetScriptStep(int rpid)
        {
                int count = _context.scriptLibrary.Where(e => e.scriptId == rpid).Count();
            return Json(count+1);
        }

        // GET: ScriptLibraries/Create
        public IActionResult Create()
        {
            ViewData["scriptId"] = new SelectList(_context.scriptlists, "id", "scriptName");
            return View(_context.sequencyLibrary.ToList());
        }

        // POST: ScriptLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<SequenceLibrary> sqlib)
        {
           sqlib= sqlib.OrderBy(e => e.sortOrder).ToList();
            StringBuilder runstplib = new StringBuilder("{");
            try
            {
                foreach (SequenceLibrary sl in sqlib)
                {
                    if (sl.check)
                    {
                        runstplib.Append("\"" + sl.sequenceName + "\"");
                        runstplib.Append(":" + sl.loopCount + ",");
                    }

                }
                ScriptLibrary sclib = new ScriptLibrary()
                {
                    scriptId = int.Parse(Request.Form["scriptId"].ToString()),
                    stepNumber = int.Parse(Request.Form["stepNumber"].ToString()),
                    phaseLoop = int.Parse(Request.Form["loop"].ToString()),
                    runStepLibraryWithLoop = runstplib.ToString().TrimEnd(',')+"}"
                };
                sclib.status = 1;
                _context.scriptLibrary.Add(sclib);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex) {
                return StatusCode((int)HttpStatusCode.BadRequest, new { status = (int)HttpStatusCode.BadRequest, message = ex.Message });
            }

            ViewData["scriptId"] = new SelectList(_context.scriptlists, "id", "scriptName");
            return View(_context.sequencyLibrary.ToList());
        }

        public IActionResult EditNew(int? id)
        {
            ScriptLibrary sc = _context.scriptLibrary.Include(e=>e.script).Single(e=>e.id==id);
            if (sc == null)
            {
                return BadRequest();
            }
            else
            {
                ViewBag.scriptId = sc.script.id;
                ViewBag.scriptName = sc.script.scriptName;
                ViewBag.stepNumber=sc.stepNumber;
                ViewBag.scriptloop = sc.phaseLoop;
                ViewBag.scriptno = sc.scriptId;
                ViewBag.id = sc.id;
               string rsteplib = sc.runStepLibraryWithLoop;
                string[] rsteplibarry = rsteplib.Split(',');

                List<SequenceLibrary> list = _context.sequencyLibrary.ToList();
                int count = 0;
                foreach (SequenceLibrary sl in list)
                {
                    foreach (string i in rsteplibarry)
                    {
                        try
                        {
                            string[] iy = i.Split(':');
                            if (i.Contains(sl.sequenceName) && (iy[0].Length <= sl.sequenceName.Length + 3))
                            {
                                count++;
                                sl.sortOrder = count;
                                sl.check = true;
                                string[] ix = i.Split(':');
                                sl.loopCount = int.Parse(ix[1].TrimEnd('}'));
                            }
                        }
                        catch { }
                    }

                }
                return View(list);
            }
        }

        // POST: ScriptLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNew(List<SequenceLibrary> sqlib)
        {
            ScriptLibrary sc = new ScriptLibrary();
            sqlib = sqlib.OrderBy(e => e.sortOrder).ToList();
            StringBuilder runstplib = new StringBuilder("{");
            try
            {
                foreach (SequenceLibrary sl in sqlib)
                {
                    if (sl.check)
                    {
                        runstplib.Append("\"" + sl.sequenceName + "\"");
                        runstplib.Append(":" + sl.loopCount + ",");
                    }

                }
                sc.scriptId = int.Parse(Request.Form["scriptId"].ToString());

                sc.stepNumber = int.Parse(Request.Form["stepNumber"].ToString());
                sc.phaseLoop = int.Parse(Request.Form["loop"].ToString());
                sc.id = int.Parse(Request.Form["id"].ToString());

                sc.runStepLibraryWithLoop = runstplib.ToString().TrimEnd(',') + "}";
                sc.status = 1;
                _context.Entry(sc).State=EntityState.Modified;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { status = (int)HttpStatusCode.BadRequest, message = ex.Message });
            }

            ViewData["scriptId"] = new SelectList(_context.scriptlists, "id", "scriptName");
            return View(_context.sequencyLibrary.ToList());
        }


        // GET: ScriptLibraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptLibrary = await _context.scriptLibrary.FindAsync(id);
            if (scriptLibrary == null)
            {
                return NotFound();
            }
            ViewData["scriptId"] = new SelectList(_context.scriptlists, "id", "scriptName", scriptLibrary.scriptId);
            return View(scriptLibrary);
        }

        // POST: ScriptLibraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,scriptId,stepNumber,phaseLoop,runStepLibraryWithLoop")] ScriptLibrary scriptLibrary)
        {
            if (id != scriptLibrary.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scriptLibrary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScriptLibraryExists(scriptLibrary.id))
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
            ViewData["scriptId"] = new SelectList(_context.scriptlists, "id", "id", scriptLibrary.scriptId);
            return View(scriptLibrary);
        }

        // GET: ScriptLibraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptLibrary = await _context.scriptLibrary
                .Include(s => s.script)
                .FirstOrDefaultAsync(m => m.id == id);
            if (scriptLibrary == null)
            {
                return NotFound();
            }

            return View(scriptLibrary);
        }

        // POST: ScriptLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scriptLibrary = await _context.scriptLibrary.FindAsync(id);
            _context.scriptLibrary.Remove(scriptLibrary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScriptLibraryExists(int id)
        {
            return _context.scriptLibrary.Any(e => e.id == id);
        }
    }
}
