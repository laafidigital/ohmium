using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class SitesController : Controller
    {
        private readonly SensorContext _context;

        public SitesController(SensorContext context)
        {
            _context = context;
        }

        // GET: Sites
        public async Task<IActionResult> Index(Guid? id)
        {
                var sensorCtx = _context.site.Where(x => x.orgID == id).Include(x=>x.org);
            if(id!=null)
                return View(await sensorCtx.ToListAsync());
            else
            {
                try
                {

                    Guid oid = (Guid)TempData["id"];
                    var sCtx = _context.site.Where(x => x.orgID == oid).Include(x => x.org);
                    return View(await sCtx.ToListAsync());
                }
                catch
                {
                    var sensorContext = _context.site.Include(s => s.org);
                    return View(await sensorContext.ToListAsync());
                }
            }
        }

        // GET: Sites/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.site
                .Include(s => s.org)
                .FirstOrDefaultAsync(m => m.id == id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // GET: Sites/Create
        public IActionResult Create()
        {
            ViewData["orgID"] = new SelectList(_context.org, "OrgID", "OrgName");
            ViewData["Region"] = new SelectList(_context.region, "name", "desc");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View();
        }

        // POST: Sites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,orgID,siteLat,siteLng,email,status,Region")] Site site)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    site.id = Guid.NewGuid();
                    _context.Add(site);
                    await _context.SaveChangesAsync();
                    //time = DateTime.Now.AddSeconds(5);
                    Thread t = new Thread(RunThread);
                    t.Start();
                    Thread.Sleep(5000);

                }
                Address a = new Address();
                a.address1 = Request.Form["add1"];
                a.address2 = Request.Form["add2"];
                a.address3 = Request.Form["add3"];
                a.city = Request.Form["city"];
                a.state = Request.Form["state"];
                a.country = Request.Form["ctry"];
                a.postalCode = Request.Form["zipcode"];
                a.sid = site.id;
                //if (time <= DateTime.Now)
                _context.address.Add(a);
                _context.SaveChanges();
                TempData["id"] = site.orgID;
                return RedirectToAction(nameof(Index));
            }
            catch
            {

            }
                ViewData["orgID"] = new SelectList(_context.org, "OrgID", "OrgName", site.orgID);
                ViewData["Region"] = new SelectList(_context.region, "name", "desc");
                ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(site);
            
        }

        public static void RunThread()
        {
            Console.WriteLine("Please Wait..");
        }

        // GET: Sites/Edit/5
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
            ViewData["Region"] = new SelectList(_context.region, "name","desc");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(site);
        }

        // POST: Sites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,name,orgID,siteLat,siteLng,email,status,Region")] Site site)
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
            ViewData["Region"] = new SelectList(_context.region, "name", "desc");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            return View(site);
        }

        // GET: Sites/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.site
                .Include(s => s.org)
                .FirstOrDefaultAsync(m => m.id == id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // POST: Sites/Delete/5
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
