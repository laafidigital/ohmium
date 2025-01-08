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
    public class RunProfilesController : Controller
    {
        private readonly SensorContext _context;

        public RunProfilesController(SensorContext context)
        {
            _context = context;
        }

        // GET: RunProfiles
        public async Task<IActionResult> Index()
        {
            ViewBag.site = new SelectList(_context.site, "id", "name");
            var sensorContext = _context.site;
            //var sensorContext = _context.runProfile.Include(r => r.device);
            //return View(await sensorContext.OrderBy(x=>x.device).ToListAsync());
            return View(await sensorContext.ToListAsync());
        }

        public async Task<IActionResult> GetProfile(Guid id)
        {
            List<Device> dlist = _context.device.Where(e => e.siteID == id).ToList();
            List<List<RunProfile>> runProfiles = new List<List<RunProfile>>();
            foreach(Device device in dlist)
            {
                runProfiles.Add(_context.runProfile.Where(e => e.deviceID == device.EqMfgID).Include(r => r.device).ToList());
            }
            List<RunProfile> result = runProfiles.SelectMany(e => e).OrderBy(x => x.deviceID).ToList();
            return View(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> Index(Guid site)
        //{
        //    string[] deviceList = _context.device.Where(e => e.siteID == site).Select(e => e.EqMfgID).ToArray();
        //    var sensorContext = _context.runProfile.Where(e=>e.deviceID(deviceList)).Include(r => r.device);
        //    return View(await sensorContext.OrderBy(x => x.device).ToListAsync());
        //}

        // GET: RunProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runProfile = await _context.runProfile
                .Include(r => r.device)
                .FirstOrDefaultAsync(m => m.id == id);
            if (runProfile == null)
            {
                return NotFound();
            }

            return View(runProfile);
        }

        // GET: RunProfiles/Create
        public IActionResult Create()
        {
           var selectedList = _context.device.Where(d => !_context.runProfile.Any(rp => rp.device.EqMfgID == d.EqMfgID));
           ViewData["deviceID"] = new SelectList(selectedList, "EqMfgID", "EqMfgID");
           return View();
        }

        // POST: RunProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,deviceID,dCmd,fan,pump, mnWbT,mxWbT,hxT")] RunProfile runProfile)
        {
            runProfile.profileName = runProfile.deviceID;
            if (ModelState.IsValid)
            {
                if (!RunProfileNameExists(runProfile.profileName))
                {
                    runProfile.timeStamp = DateTime.UtcNow;
                    _context.Add(runProfile);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "StackRunProfiles", runProfile);
                }
                else
                {
                    ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID");
                    ViewData["msg"] = "This Profile Name Already Exists";
                    return View();
                }
            }
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", runProfile.deviceID);
            return View(runProfile);
        }

        // GET: RunProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runProfile = await _context.runProfile.FindAsync(id);
            if (runProfile == null)
            {
                return NotFound();
            }
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", runProfile.deviceID);
            return View(runProfile);
        }

        // POST: RunProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,profileName,deviceID,dCmd,fan,pump,mnWbT,mxWbT,hxT")] RunProfile runProfile)
        {
            if (id != runProfile.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(runProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunProfileExists(runProfile.id))
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
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", runProfile.deviceID);
            return View(runProfile);
        }

        // GET: RunProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runProfile = await _context.runProfile
                .Include(r => r.device)
                .FirstOrDefaultAsync(m => m.id == id);
            if (runProfile == null)
            {
                return NotFound();
            }

            return View(runProfile);
        }

        // POST: RunProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var runProfile = await _context.runProfile.FindAsync(id);
            _context.runProfile.Remove(runProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RunProfileExists(int id)
        {
            return _context.runProfile.Any(e => e.id == id);
        }
        private bool RunProfileNameExists(string profileName)
        {
            return _context.runProfile.Where(x => x.profileName == profileName).Count() > 0;
        }
    }
}
