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
    public class DevicesController : Controller
    {
        private readonly SensorContext _context;

        public DevicesController(SensorContext context)
        {
            _context = context;
        }

        // GET: Devices
        public async Task<IActionResult> Index(Guid? id, int? data)
        {
            var sensorContext = _context.device.Where(d=>d.siteID==id && d.status==1).Include(d=>d.statustype).Include(d => d.ec).Include(d => d.site);
            if (data == 3 && id == null)
            {
                sensorContext = _context.device.Where(d => d.status == 3).Include(d => d.statustype).Include(d => d.ec).Include(d => d.site);
                ViewBag.status = "InActive Test Stands";
                return View(sensorContext);
            }
            else if(data==null && id!=null)
            {
                ViewBag.status = "Active Test Stands";
                return View(sensorContext);
            }
            else
            {
                try
                {
                    ViewBag.status = "Active Test Stands";
                    Guid gid = (Guid)TempData["siteid"];
                    var sContext = _context.device.Where(d => d.siteID == gid && d.status==1).Include(d => d.statustype).Include(d => d.ec).Include(d => d.site);
                    return View(await sContext.ToListAsync());
                }
                catch
                {
                    ViewBag.status = "Active Test Stands";
                    return View(await _context.device.Where(d=>d.status==1).Include(d => d.statustype).Include(d => d.ec).Include(d => d.site).ToListAsync());
                }
            }
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.device
                .Include(d => d.ec)
                .Include(d => d.site)
                .FirstOrDefaultAsync(m => m.EqMfgID == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            ViewData["configID"] = new SelectList(_context.equipmentConfiguration, "equipmentConfigID", "configName");
            ViewData["siteID"] = new SelectList(_context.site, "id", "name");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Device device)
        {
            if (ModelState.IsValid)
            {
                device.createdBy = HttpContext.User.Identity.Name;
                device.createdOn = DateTime.UtcNow;
                device.updatedBy = "";
                device.updatedOn = DateTime.UtcNow;
                _context.Add(device);
                await _context.SaveChangesAsync();
                TempData["siteid"] = device.siteID;
                return RedirectToAction(nameof(Index));
            }
            ViewData["configID"] = new SelectList(_context.equipmentConfiguration, "equipmentConfigID", "equipmentConfigID", device.configID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "name", device.siteID);
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.device.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            ViewData["configID"] = new SelectList(_context.equipmentConfiguration, "equipmentConfigID", "configName", device.configID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "name", device.siteID);
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Device device)
        {
            //if (id != device.EqMfgID)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    device.updatedBy = HttpContext.User.Identity.Name;
                    device.updatedOn = DateTime.UtcNow;
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.EqMfgID))
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
            ViewData["configID"] = new SelectList(_context.equipmentConfiguration, "equipmentConfigID", "equipmentConfigID", device.configID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "name", device.siteID);
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(device);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.device
                .Include(d => d.ec)
                .Include(d => d.site)
                .FirstOrDefaultAsync(m => m.EqMfgID == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Device device)
        {
            device.status = 2;
            _context.Entry(device).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(string id)
        {
            return _context.device.Any(e => e.EqMfgID == id);
        }
    }
}
