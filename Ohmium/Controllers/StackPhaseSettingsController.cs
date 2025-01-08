using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers
{
    public class StackPhaseSettingsController : Controller
    {
        private readonly SensorContext _context;
        public StackPhaseSettingsController(SensorContext context)
        {
            _context = context;
        }

        // GET: StackPhaseSettings

        public async Task<IActionResult> IndexNew(string id){
            if (id == null)
            {
                //ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                ViewBag.stk = new SelectList(_context.stack.ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                return View(new List<StackPhaseSettingNew>());
            }
            try
            {
                //added .Include(s => s.rsg) to the below line to show the sequence name after edit
                List<StackPhaseSettingNew> sps = _context.stackPhaseSettingNew.Where(e => e.stackID == id).Include(e => e.stk).ToList();
                ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                if (sps.Select(e => e.id).Max() > 0)
                    return View(sps);
                else
                {
                    ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                    var sensorContext = new List<StackPhaseSettingNew>();
                    return View(sensorContext);
                }
            }
            catch
            {
                var spslist = new List<StackPhaseSettingNew>();
                ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                return View(spslist);
            }
        }
        [HttpPost, ActionName("IndexNew")]
        public async Task<IActionResult> IndexPostNew()
        {
            string stkmfgid = Request.Form["stk"].ToString();
            ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
            var sensorContext = _context.stackPhaseSettingNew.Where(e => e.stackID == stkmfgid).Include(s => s.stk);
            return View(await sensorContext.ToListAsync());
            //return RedirectToAction("DeleteMultiple", new { id = stkmfgid });
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                return View(new List<StackPhaseSetting>());
            }
                try
            {
                //added .Include(s => s.rsg) to the below line to show the sequence name after edit
                List<StackPhaseSetting> sps = _context.stkPhaseSetting.Where(e => e.stackID == id).Include(s => s.rsg).Include(e => e.stk).ToList();
                ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                if (sps.Select(e => e.id).Max() > 0)
                    return View(sps);
                else
                {
                    ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                    var sensorContext = new List<StackPhaseSetting>();
                    return View(sensorContext);
                }
            }
            catch
            {
                var spslist = new List<StackPhaseSetting>();
                ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
                return View(spslist);
            }
        }

        [HttpPost, ActionName("index")]
        public async Task<IActionResult> IndexPost()
        {
            string stkmfgid = Request.Form["stk"].ToString();
            ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
            var sensorContext = _context.stkPhaseSetting.Where(e => e.stackID == stkmfgid).Include(s => s.rsg).Include(s => s.stk).OrderBy(e => e.phase);
            // return View(await sensorContext.ToListAsync());
            return RedirectToAction("DeleteMultiple", new { id = stkmfgid });
        }

        // GET: StackPhaseSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackPhaseSetting = await _context.stkPhaseSetting
                .Include(s => s.rsg)
                .Include(s => s.stk)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stackPhaseSetting == null)
            {
                return NotFound();
            }

            return View(stackPhaseSetting);
        }

        public IActionResult CreateNew()
        {
            ViewData["deviceID"] = new SelectList(_context.device.ToList(), "EqMfgID", "EqMfgID");
            ViewData["stackID"] = new SelectList(_context.stack.ToList(), "stackMfgID", "stackMfgID");
            return View(_context.runStepTemplateGroup.Where(e=>e.status==1).ToList());
        }

        [HttpPost]
        public IActionResult CreateNew(List<RunStepTemplateGroup> rstg)
        {
            rstg = rstg.OrderBy(e => e.sequence).ToList();
            StackPhaseSettingNew sp = new StackPhaseSettingNew();
            sp.stackID = Request.Form["stackID"].ToString();
            sp.phaseGroup = _context.stackPhaseSettingNew.Where(e => e.stackID == sp.stackID).Count() + 1;
            sp.phaseLoop = int.Parse(Request.Form["phaseLoop"].ToString());
            sp.SequenceListWithLoop = "{";
            foreach (var rsg in rstg)
            {
                if(rsg.check)
                sp.SequenceListWithLoop += "\"" + rsg.name + "\"" + ":" + rsg.numLoops + ",";
             }
            sp.SequenceListWithLoop = sp.SequenceListWithLoop.TrimEnd(',') + "}";
            _context.stackPhaseSettingNew.Add(sp);
            _context.SaveChanges();
            return RedirectToAction(nameof(IndexNew));
        }

            public JsonResult GetStacks(string tid)
        {
            var stacks = _context.stack.Where(e => e.deviceID == tid).Select(e => new
            {
                text = e.stackMfgID,
                val = e.stackMfgID
            });
            return Json(stacks);
        }
            // GET: StackPhaseSettings/Create
            public IActionResult Create()
        {
            ViewData["rsgid"] = new SelectList(_context.runStepTemplateGroup.Where(e=>e.status==1), "id", "name");
            ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID");
            ViewData["phaseGroup"] = new SelectList(_context.stkPhaseSetting, "phaseGroup", "phaseGroup");
            return View();
        }
        public JsonResult FetchPhaseCount(string id)
        {
            var count = _context.stkPhaseSetting.Where(c => c.stackID == id).Count();
            return Json(count);
        }

        public JsonResult FetchPhaseGroup(string id)
        {
            List<int?> pglist = _context.stkPhaseSetting.Where(e => e.stackID == id && e.phaseGroup != null).Select(e => e.phaseGroup).Distinct().ToList();
            string msg = pglist.Count == 0 ? "| 0 Groups already created" : "| Groups already created are: " + string.Join(',', pglist);
            return Json(msg);
        }

        // POST: StackPhaseSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,stackID,phase,rsgid,loop,phaseGroup,phaseGroupLoop")] StackPhaseSetting stackPhaseSetting)
        {
            if (ModelState.IsValid)
            {
                //int groupLoop = _context.runStepTemplateGroup.Single(x => x.id == stackPhaseSetting.rsgid).numLoops;
                //int stkrunpfid = _context.stkRunProfile.FirstOrDefault(x => x.stackID == stackPhaseSetting.stackID).id; 
                //float stkStephrsSum = _context.runStepTemplate.Where(x => x.rstGID == stackPhaseSetting.rsgid && x.stkRunProfileID==stkrunpfid).Sum(x => x.duration);
                //int h = (int)((groupLoop * stkStephrsSum)/60);
                //int min = (int)((groupLoop * stkStephrsSum) % 60);
                //stackPhaseSetting.mins = stkStephrsSum;
                //stackPhaseSetting.totalMins = stackPhaseSetting.mins * stackPhaseSetting.loop;
                //stackPhaseSetting.hrs = stackPhaseSetting.totalMins / 60;
                //_context.Add(stackPhaseSetting);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                int groupLoop = 0; float sSum = 0;
                int stkrunpfid = _context.stkRunProfile.FirstOrDefault(x => x.stackID == stackPhaseSetting.stackID).id;
                try
                {
                    groupLoop = _context.runStepTemplateGroup.Single(x => x.id == stackPhaseSetting.rsgid).numLoops;
                    sSum = _context.stkStep.Where(x => x.rstGID == stackPhaseSetting.rsgid && x.stkRunProfileID == stkrunpfid).Sum(x => x.duration);
                }
                catch { }
                int seconds = (int)(sSum * groupLoop);
                int hours = seconds / 3600;
                int minutes = (seconds / 60) - (hours * 60);
                int secs = seconds - (hours * 3600) - (minutes * 60);

                int totalminutes = (int)(minutes);
                stackPhaseSetting.seconds = secs;
                stackPhaseSetting.mins = minutes;
                stackPhaseSetting.hrs = hours;
                stackPhaseSetting.hrsminsdisplay = hours + ":" + minutes + ":" + secs;
                //-------------------------
                int tsecs = (int)(sSum * groupLoop) * stackPhaseSetting.loop;
                int thours = tsecs / 3600;
                int tminutes = (tsecs / 60) - (thours * 60);
                int sec = tsecs - (thours * 3600) - (tminutes * 60);
                stackPhaseSetting.totalHours = thours;
                stackPhaseSetting.totalMins = tminutes;
                stackPhaseSetting.totalhrsminsdisplay = thours + ":" + tminutes + ":" + sec;

                _context.Add(stackPhaseSetting);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = stackPhaseSetting.stackID });
                //                return RedirectToAction(nameof(ContinueCreate), new { id = stackPhaseSetting.stackID });
            }
            //Change select list text to name from id 
            ViewData["rsgid"] = new SelectList(_context.runStepTemplateGroup, "id", "name", stackPhaseSetting.rsgid);

            ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID", stackPhaseSetting.stackID);
            return View(stackPhaseSetting);
        }

        public async Task<IActionResult> ContinueCreate(string id)
        {
            ViewBag.stkid = id;
            ViewData["rsgid"] = new SelectList(_context.runStepTemplateGroup, "id", "name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContinueCreate(StackPhaseSetting sps)
        {
            if (ModelState.IsValid)
            {
                _context.stkPhaseSetting.Add(sps);
                _context.SaveChanges();
            }
            return View(new { id = sps.stackID });

        }

        // GET: StackPhaseSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackPhaseSetting = await _context.stkPhaseSetting.FindAsync(id);
            if (stackPhaseSetting == null)
            {
                return NotFound();
            }
            ViewData["rsgid"] = new SelectList(_context.runStepTemplateGroup.Where(e=>e.status==1), "id", "name", stackPhaseSetting.rsgid);

            ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID", stackPhaseSetting.stackID);
            return View(stackPhaseSetting);
        }

        // POST: StackPhaseSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,stackID,phase,rsgid,loop,phaseGroup,phaseGroupLoop")] StackPhaseSetting stackPhaseSetting)
        {
            if (id != stackPhaseSetting.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //--------------
                    int groupLoop = 0; float sSum = 0;
                    int stkrunpfid = _context.stkRunProfile.FirstOrDefault(x => x.stackID == stackPhaseSetting.stackID).id;
                    try
                    {
                        groupLoop = _context.runStepTemplateGroup.Single(x => x.id == stackPhaseSetting.rsgid).numLoops;
                        sSum = _context.stkStep.Where(x => x.rstGID == stackPhaseSetting.rsgid && x.stkRunProfileID == stkrunpfid).Sum(x => x.duration);
                    }
                    catch
                    {

                    }
                    int seconds = (int)(sSum * groupLoop);
                    int hours = seconds / 3600;
                    int minutes = (seconds / 60) - (hours * 60);
                    int secs = seconds - (hours * 3600) - (minutes * 60);

                    int totalminutes = (int)(minutes);
                    stackPhaseSetting.seconds = secs;
                    stackPhaseSetting.mins = minutes;
                    stackPhaseSetting.hrs = hours;
                    stackPhaseSetting.hrsminsdisplay = hours + ":" + minutes + ":" + secs;
                    //-------------------------
                    int tsecs = (int)(sSum * groupLoop) * stackPhaseSetting.loop;
                    int thours = tsecs / 3600;
                    int tminutes = (tsecs / 60) - (thours * 60);
                    int sec = tsecs - (thours * 3600) - (tminutes * 60);
                    stackPhaseSetting.totalHours = thours;
                    stackPhaseSetting.totalMins = tminutes;
                    stackPhaseSetting.totalhrsminsdisplay = thours + ":" + tminutes + ":" + sec;

                    //------------------------
                    _context.Update(stackPhaseSetting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StackPhaseSettingExists(stackPhaseSetting.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Commented the below line as temp variable is assigned but not used and also creating serilization issue
                // TempData["phasesettinglist"] = _context.stkPhaseSetting.Where(e => e.stackID == stackPhaseSetting.stackID).ToList();
                //add the id parameter 
                return RedirectToAction(nameof(Index), new { id = stackPhaseSetting.stackID });
            }
            ViewData["rsgid"] = new SelectList(_context.runStepTemplateGroup.Where(e => e.status == 1), "id", "name", stackPhaseSetting.rsgid);
            ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID", stackPhaseSetting.stackID);
            return View(stackPhaseSetting);
        }

        // GET: StackPhaseSettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackPhaseSetting = await _context.stkPhaseSetting
                .Include(s => s.rsg)
                .Include(s => s.stk)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stackPhaseSetting == null)
            {
                return NotFound();
            }

            return View(stackPhaseSetting);
        }

        public async Task<IActionResult> DeleteNew(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackPhaseSettingNew = await _context.stackPhaseSettingNew
                .Include(s => s.stk)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stackPhaseSettingNew == null)
            {
                return NotFound();
            }

            return View(stackPhaseSettingNew);
        }

        // POST: StackPhaseSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            StackPhaseSetting stackPhaseSetting = await _context.stkPhaseSetting.FindAsync(id);
            _context.stkPhaseSetting.Remove(stackPhaseSetting);
            await _context.SaveChangesAsync();
            //            List<StackPhaseSetting> sps = _context.stkPhaseSetting.Where(e => e.stackID == stackPhaseSetting.stackID).ToList();
            return RedirectToAction(nameof(Index), new { id = stackPhaseSetting.stackID });
        }

        [HttpPost, ActionName("DeleteNew")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedNew(int id)
        {
            StackPhaseSettingNew stackPhaseSettingNew = await _context.stackPhaseSettingNew.FindAsync(id);
            _context.stackPhaseSettingNew.Remove(stackPhaseSettingNew);
            await _context.SaveChangesAsync();
            //            List<StackPhaseSetting> sps = _context.stkPhaseSetting.Where(e => e.stackID == stackPhaseSetting.stackID).ToList();
            return RedirectToAction(nameof(IndexNew), new { id = stackPhaseSettingNew.stackID });
        }


        public IActionResult DeleteMultiple(string id)
        {
            List<StackPhaseSetting> sps = _context.stkPhaseSetting.Where(e => e.stackID == id).Include(s => s.rsg).Include(e => e.stk).ToList()!=null? _context.stkPhaseSetting.Where(e => e.stackID == id).Include(s => s.rsg).Include(e => e.stk).ToList():new List<StackPhaseSetting>();
            ViewBag.stk = new SelectList(_context.stack.Where(e => e.status == 1).ToList(), "stackMfgID", "stackMfgID"); //dropdownlist of stacks
           
            try
            {
                int x = sps.Select(e => e.id).Max();
            }
            catch
            {
                sps = new List<StackPhaseSetting>();
            }
            return View(sps);
        }

        [HttpPost, ActionName("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultiplePost(List<StackPhaseSetting> spsList)
        {
            string stkid = spsList.FirstOrDefault(e => e.check == true).stk.stackMfgID;
            foreach (var ss in spsList)
            {
                if (ss.check)
                {
                    _context.Entry(ss).State=EntityState.Deleted;
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index", new { id = stkid });
        }

        public async Task<IActionResult> ScriptSetting()
        {
            return View();
        }
        private bool StackPhaseSettingExists(int id)
        {
            return _context.stkPhaseSetting.Any(e => e.id == id);
        }
    }
}
