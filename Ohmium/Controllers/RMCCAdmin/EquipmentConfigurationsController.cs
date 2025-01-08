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
    public class EquipmentConfigurationsController : Controller
    {
        private readonly SensorContext _context;

        public EquipmentConfigurationsController(SensorContext context)
        {
            _context = context;
        }

        // GET: EquipmentConfigurations
        public async Task<IActionResult> Index()
        {
            return View(await _context.equipmentConfiguration.ToListAsync());
        }

        // GET: EquipmentConfigurations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentConfiguration = await _context.equipmentConfiguration
                .FirstOrDefaultAsync(m => m.equipmentConfigID == id);
            if (equipmentConfiguration == null)
            {
                return NotFound();
            }

            return View(equipmentConfiguration);
        }

        // GET: EquipmentConfigurations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EquipmentConfigurations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("equipmentConfigID,configName,equipmentConfiguration,colorConfig,createdOn,updatedOn,createdBy,updatedBy")] EquipmentConfiguration eqconfig)
        {
            if (ModelState.IsValid)
            {
                eqconfig.createdBy = HttpContext.User.Identity.Name;
                eqconfig.createdOn = DateTime.Now;
                _context.Add(eqconfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eqconfig);
        }

        // GET: EquipmentConfigurations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentConfiguration = await _context.equipmentConfiguration.FindAsync(id);
            if (equipmentConfiguration == null)
            {
                return NotFound();
            }
            return View(equipmentConfiguration);
        }

        // POST: EquipmentConfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("equipmentConfigID,configName,equipmentConfiguration,colorConfig,createdOn,updatedOn,createdBy,updatedBy")] EquipmentConfiguration ec)
        {
       
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentConfigurationExists(ec.equipmentConfigID))
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
            return View(ec);
        }

        // GET: EquipmentConfigurations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentConfiguration = await _context.equipmentConfiguration
                .FirstOrDefaultAsync(m => m.equipmentConfigID == id);
            if (equipmentConfiguration == null)
            {
                return NotFound();
            }

            return View(equipmentConfiguration);
        }

        // POST: EquipmentConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var equipmentConfiguration = await _context.equipmentConfiguration.FindAsync(id);
            _context.equipmentConfiguration.Remove(equipmentConfiguration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentConfigurationExists(Guid id)
        {
            return _context.equipmentConfiguration.Any(e => e.equipmentConfigID == id);
        }
    }
}
