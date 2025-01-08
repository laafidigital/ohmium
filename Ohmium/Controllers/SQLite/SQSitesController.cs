using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models;
using Ohmium.Models.EFModels;

namespace Ohmium.Controllers.SQLite
{
    public class SQSitesController : Controller
    {
        private readonly CacheContext _context;

        public SQSitesController(CacheContext context)
        {
            _context = context;
        }

        // GET: SQSites
        public async Task<IActionResult> Index(Guid? id)
        {
            var cacheContext = _context.site.Where(e=>e.orgID==id).Include(s => s.org).Include(s => s.reg);
            return View(await cacheContext.ToListAsync());
        }

        // GET: SQSites/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.site
                .Include(s => s.org)
                .Include(s => s.reg)
                .FirstOrDefaultAsync(m => m.id == id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // GET: SQSites/Create
        public IActionResult Create()
        {
            ViewData["orgID"] = new SelectList(_context.org, "OrgID", "OrgName");
            ViewData["Region"] = new SelectList(_context.region, "name", "name");
            return View();
        }

        // POST: SQSites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,orgID,Region,siteLat,siteLng,email,status,h2Production,powerConsumption,siteEfficiency")] Site site)
        {
            if (ModelState.IsValid)
            {
                site.id = Guid.NewGuid();
                _context.Add(site);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["orgID"] = new SelectList(_context.org, "OrgID", "OrgName", site.orgID);
            ViewData["Region"] = new SelectList(_context.region, "name", "name", site.Region);
            return View(site);
        }

        // GET: SQSites/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.site.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }
            ViewData["orgID"] = new SelectList(_context.org, "OrgID", "OrgName", site.orgID);
            ViewData["Region"] = new SelectList(_context.region, "name", "name", site.Region);
            return View(site);
        }

        // POST: SQSites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,name,orgID,Region,siteLat,siteLng,email,status,h2Production,powerConsumption,siteEfficiency")] Site site)
        {
            if (id != site.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(site);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiteExists(site.id))
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
            ViewData["orgID"] = new SelectList(_context.org, "OrgID", "OrgName", site.orgID);
            ViewData["Region"] = new SelectList(_context.region, "name", "name", site.Region);
            return View(site);
        }

        // GET: SQSites/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.site
                .Include(s => s.org)
                .Include(s => s.reg)
                .FirstOrDefaultAsync(m => m.id == id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // POST: SQSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var site = await _context.site.FindAsync(id);
            _context.site.Remove(site);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiteExists(Guid id)
        {
            return _context.site.Any(e => e.id == id);
        }
    }
}
