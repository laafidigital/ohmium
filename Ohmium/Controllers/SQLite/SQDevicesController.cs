using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.SQLite
{
    public class SQDevicesController : Controller
    {
        private readonly CacheContext _context;
        private readonly SensorContext _sc;

        public SQDevicesController(CacheContext context, SensorContext sc)
        {
            _context = context;
            _sc = sc;
        }

        // GET: SQDevices
        public async Task<IActionResult> Index(Guid? id,int? data)
        {
            Guid stid = _context.site.First(e => e.id == id).sqlId;
            var sensorContext = _context.device.Where(d => d.siteID == stid && d.status == 1);
            if (data == 3 && id == null)
            {
                sensorContext = _context.device.Where(d => d.status == 3);
                ViewBag.status = "InActive Test Stands";
                return View(sensorContext);
            }
            else if (data == null && id != null)
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
                    var sContext = _context.device.Where(d => d.siteID == gid && d.status == 1);
                    return View(await sContext.ToListAsync());
                }
                catch
                {
                    ViewBag.status = "Active Test Stands";
                    return View(await _context.device.Where(d => d.status == 1).ToListAsync());
                }
            }
        }

        // GET: SQDevices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.device
                .Include(d => d.ec)
                .Include(d => d.site)
                .Include(d => d.statustype)
                .FirstOrDefaultAsync(m => m.EqMfgID == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: SQDevices/Create
        public IActionResult Create()
        {
            ViewData["configID"] = new SelectList(_sc.equipmentConfiguration, "equipmentConfigID", "configName");
            ViewData["siteID"] = new SelectList(_context.site, "id", "name");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View();
        }

        // POST: SQDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EqMfgID,EqDes,deviceType,siteID,configID,comStatus,h2Production,powerConsumption,siteEfficiency,createdOn,updatedOn,createdBy,updatedBy,nStack,status,ver,isRunning,lastDataReceivedOn")] Device device)
        {
            if (ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["configID"] = new SelectList(_context.equipmentConfiguration, "equipmentConfigID", "equipmentConfigID", device.configID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "name", device.siteID);
            ViewData["status"] = new SelectList(_context.statusType, "id", "name", device.status);
            return View(device);
        }

        // GET: SQDevices/Edit/5
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
            ViewData["configID"] = new SelectList(_context.equipmentConfiguration, "equipmentConfigID", "equipmentConfigID", device.configID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "name", device.siteID);
            ViewData["status"] = new SelectList(_context.statusType, "id", "name", device.status);
            return View(device);
        }

        // POST: SQDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EqMfgID,EqDes,deviceType,siteID,configID,comStatus,h2Production,powerConsumption,siteEfficiency,createdOn,updatedOn,createdBy,updatedBy,nStack,status,ver,isRunning,lastDataReceivedOn")] Device device)
        {
            if (id != device.EqMfgID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["status"] = new SelectList(_context.statusType, "id", "name", device.status);
            return View(device);
        }

        // GET: SQDevices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.device
                .Include(d => d.ec)
                .Include(d => d.site)
                .Include(d => d.statustype)
                .FirstOrDefaultAsync(m => m.EqMfgID == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: SQDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var device = await _context.device.FindAsync(id);
            _context.device.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(string id)
        {
            return _context.device.Any(e => e.EqMfgID == id);
        }
    }
}
