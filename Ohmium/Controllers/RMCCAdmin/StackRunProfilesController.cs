using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Models.EFModels.ViewModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class StackRunProfilesController : Controller
    {
        private readonly SensorContext _context;
        public IEnumerable<StackTestRunHours> srt;

        public StackRunProfilesController(SensorContext context)
        {
            _context = context;
        }

        // GET: StackRunProfiles
        public async Task<IActionResult> Index(int? id)
        {
            try { id = int.Parse(TempData["stkprfid"].ToString()); } catch { }
            if (id != null)
            {
                ViewData["deviceConfig"] = _context.runProfile.Find(id);
                List<string> stkids = _context.stkRunProfile.Include(s => s.profile).Include(s => s.stk).Where(x => x.profileID == id).Select(e=>e.stackID).ToList();
                var th = "[ Cumulative Hours StackWise : ";
                foreach (string s in stkids)
                {
                    th += s + " : " + _context.stackTestRunHours.Where(e => e.stkMfgId == s).Sum(e => e.cumulativeHours) + "|";
                    
                }
                th = th.TrimEnd('|');
                ViewBag.ch = th + "]";
                return View(_context.stkRunProfile.Include(s => s.profile).Include(s => s.stk).Where(x => x.profileID == id));
            }

            var sensorContext = _context.stkRunProfile.Include(s => s.profile).Include(s => s.stk);
            return View(await sensorContext.ToListAsync());
        }

        // GET: StackRunProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackRunProfile = await _context.stkRunProfile
                .Include(s => s.profile)
                .Include(s => s.stk)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stackRunProfile == null)
            {
                return NotFound();
            }

            return View(stackRunProfile);
        }

        // GET: StackRunProfiles/Create
        public IActionResult Create(int? id)
        {
            if (id != null)
            {
                List<Stack> stkList = new List<Stack>();
                string DeviceID = _context.runProfile.Find(id).deviceID;
                ViewData["profileID"] = new SelectList(_context.runProfile.Where(x => x.id == id), "id", "profileName");
                string[] stkids = _context.stack.Where(x => x.deviceID == DeviceID).Select(x => x.stackMfgID).ToArray();
                foreach (string stkmf in stkids)
                {
                    try
                    {
                        int count = _context.stkRunProfile.FirstOrDefault(x => x.stackID == stkmf).name.Count();
                    }
                    catch
                    {
                        stkList.Add(_context.stack.Find(stkmf));
                    }

                }
                ViewData["stackID"] = new SelectList(stkList, "stackMfgID", "stackMfgID");
            }
            else
            {
                ViewData["profileID"] = new SelectList(_context.runProfile, "id", "profileName");
                ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID");
            }
            return View();
        }

        // POST: StackRunProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,profileID,stackID,loop,command")] StackRunProfile stackRunProfile)
        {
            if (ModelState.IsValid)
            {
                stackRunProfile.stackPosition = _context.stack.Find(stackRunProfile.stackID).stackPosition;
                _context.Add(stackRunProfile);
                await _context.SaveChangesAsync();
                RunProfile rp = _context.runProfile.Find(stackRunProfile.profileID);
                return RedirectToAction(nameof(Index), rp);
            }
            ViewData["profileID"] = new SelectList(_context.runProfile, "id", "id", stackRunProfile.profileID);
            ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID", stackRunProfile.stackID);
            return View(stackRunProfile);
        }

        // GET: StackRunProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackRunProfile = await _context.stkRunProfile.FindAsync(id);
            if (stackRunProfile == null)
            {
                return NotFound();
            }
            if (id != null)
            {
                int profileID = _context.stkRunProfile.Find(id).profileID;
                string deviceID = _context.runProfile.Find(profileID).deviceID;
                ViewData["profileID"] = new SelectList(_context.runProfile.Where(x => x.id == profileID), "id", "profileName");
                ViewData["stackID"] = new SelectList(_context.stack.Where(x => x.deviceID == deviceID), "stackMfgID", "stackMfgID");
            }
            else
            {
                ViewData["profileID"] = new SelectList(_context.runProfile, "id", "profileName");
                ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID");
            }

            return View(stackRunProfile);
        }

        // POST: StackRunProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,profileID,stackID,loop,command")] StackRunProfile stackRunProfile)
        {
            string status = Request.Form["status"].ToString();
            if (id != stackRunProfile.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    stackRunProfile.stackPosition = _context.stack.Find(stackRunProfile.stackID).stackPosition;
                    _context.Update(stackRunProfile);
                    await _context.SaveChangesAsync();
                    if (status == "true")
                    {
                        StackTestRunHours strh = new StackTestRunHours();
                        strh.stkMfgId = stackRunProfile.stackID;
                        strh.timeStampUTC = DateTime.Now.ToUniversalTime();
                        string stkid = strh.stkMfgId;
                        string divid = _context.stack.FirstOrDefault(e => e.stackMfgID == stkid).deviceID;
                        //MTSStackData stkdata = new MTSStackData
                        //{
                        //    stackMfgID = stkid,
                        //    deviceID = devid
                        //};
                        MTSStackData tssd = _context.mtsStackData.FromSqlRaw<MTSStackData>("exec spruntimerolledover {0}, {1}", stkid, divid).ToList().FirstOrDefault();
                        try {
                            strh.cumulativeHours = tssd.runHours; 
                        }
                        catch(Exception ex) {
                            string msg = ex.Message;
                            strh.cumulativeHours = 0; 
                        }
                        _context.Add(strh);
                        _context.SaveChanges();
                        
                        //srt = (IEnumerable<StackTestRunHours>) _rh.GetAll();
                        
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StackRunProfileExists(stackRunProfile.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["stkprfid"] = stackRunProfile.profileID;
                return RedirectToAction(nameof(Index));
            }
            ViewData["profileID"] = new SelectList(_context.runProfile, "id", "id");
            ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID", stackRunProfile.stackID);
            return View(stackRunProfile);
        }

        // GET: StackRunProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackRunProfile = await _context.stkRunProfile
                .Include(s => s.profile)
                .Include(s => s.stk)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stackRunProfile == null)
            {
                return NotFound();
            }

            return View(stackRunProfile);
        }

        // POST: StackRunProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stackRunProfile = await _context.stkRunProfile.FindAsync(id);
            _context.stkRunProfile.Remove(stackRunProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StackRunProfileExists(int id)
        {
            return _context.stkRunProfile.Any(e => e.id == id);
        }
    }
}
