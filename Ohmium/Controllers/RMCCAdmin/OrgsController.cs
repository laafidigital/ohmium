using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Ohmium.Models;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class OrgsController : Controller
    {
        private readonly SensorContext _context;
        private readonly CacheContext _cache;

        public OrgsController(SensorContext context, CacheContext cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Orgs
        public async Task<IActionResult> Index()
        {
            var organizations = await _context.org.ToListAsync();
            return View(organizations==null?new Org():organizations);
        }

        // GET: Orgs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var org = await _context.org
                .FirstOrDefaultAsync(m => m.OrgID == id);
            if (org == null)
            {
                return NotFound();
            }

            return View(org);
        }

        // GET: Orgs/Create
        public IActionResult Create()
        {
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View();
        }

        // POST: Orgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrgID,OrgName,status,createdOn,updatedOn,createdBy,updatedBy")] Org org)
        {
            
            if (ModelState.IsValid)
            {
                org.createdOn = DateTime.Now;
                org.createdBy = HttpContext.User.Identity.Name;
                _context.Add(org);
                await _context.SaveChangesAsync();
                _cache.org.Add(org);
                _cache.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(org);
        }

        // GET: Orgs/Edit/83FCC6D9-15CA-4D3E-DA8B-08D9B8B55747
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var org = await _context.org.FindAsync(id);
            if (org == null)
            {
                return NotFound();
            }
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(org);
        }

        // POST: Orgs/Edit/83FCC6D9-15CA-4D3E-DA8B-08D9B8B55747
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind("OrgID,OrgName,status,createdOn,updatedOn,createdBy,updatedBy")] Org org)
        {
            //if (id != org.OrgID)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    org.updatedBy = HttpContext.User.Identity.Name;
                    org.updatedOn = DateTime.Now;
                    _context.Update(org);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrgExists(org.OrgID))
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
            return View(org);
        }

        // GET: Orgs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var org = await _context.org
                .FirstOrDefaultAsync(m => m.OrgID == id);
            if (org == null)
            {
                return NotFound();
            }

            return View(org);
        }

        // POST: Orgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var org = await _context.org.FindAsync(id);
            _context.org.Remove(org);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrgExists(Guid id)
        {
            return _context.org.Any(e => e.OrgID == id);
        }
    }
}
