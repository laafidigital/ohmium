using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.SQLite
{
    public class SQOrgsController : Controller
    {
        private readonly CacheContext _context;
        private readonly SensorContext _sc;

        private readonly IHttpContextAccessor _accessor;

        public SQOrgsController(CacheContext context,SensorContext sc,IHttpContextAccessor accessor)
        {
            _context = context;
            _sc = sc;
            _accessor = accessor;
        }

        // GET: SQOrgs
        public ActionResult Index()
        {
            List<Org> orgList = _context.org.ToList();
            if (orgList.Count() == 0)
            {
                SQLiteSeedData ss = new SQLiteSeedData(_context,_sc);
                orgList = ss.AddOrgs();
            }
            return View(orgList);
        }

        // GET: SQOrgs/Details/5
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

        // GET: SQOrgs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SQOrgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrgID,OrgName,status")] Org org)
        {
            if (ModelState.IsValid)
            {
                org.OrgID = Guid.NewGuid();
                org.createdOn = DateTime.UtcNow;
                org.updatedOn= DateTime.UtcNow;
                org.createdBy = _accessor.HttpContext.User.Identity.Name;
                org.updatedBy= _accessor.HttpContext.User.Identity.Name;
                _context.Add(org);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(org);
        }

        // GET: SQOrgs/Edit/5
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
            return View(org);
        }

        // POST: SQOrgs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OrgID,OrgName,status,createdOn,updatedOn,createdBy,updatedBy")] Org org)
        {
            if (id != org.OrgID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: SQOrgs/Delete/5
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

        // POST: SQOrgs/Delete/5
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
