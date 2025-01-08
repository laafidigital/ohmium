using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Models.EFModels.ViewModels;
using Ohmium.Models.TemplateModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class DashboardController : Controller
    {
        private readonly SensorContext _context;

        public IActionResult SiteList()
        {
            return View(_context.site.ToList());
        }
        public IActionResult TestStandList(Guid siteid)
        {
            return View(_context.device.Where(e=>e.siteID==siteid).ToList());
        }

        public async Task<IActionResult> ScriptList(string divid)
        {
            var stk = _context.stack.Where(e => e.deviceID == divid).Select(e => new
            {
                stkmfgid = e.stackMfgID
            }).ToArray();
            List<StackSync> stsyn = new List<StackSync>();
            if (stk != null)
            {
                foreach (var s in stk)
                {
                    try
                    {
                        string ssk = s.stkmfgid;
                        StackSync ss = _context.stackSyncData.Include(e => e.script).Single(e => e.stackID == ssk);
                        if (ss != null)
                            stsyn.Add(ss);
                    }
                    catch { }
                }
            }

            return View(stsyn);
        }

        public DashboardController(SensorContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index(Guid siteid)
        {
            var stk = _context.stack.Where(e => e.siteID == siteid).Select(e => new
            {
                stkmfgid=e.stackMfgID
            }).ToArray();
            List<StackSync> stsyn = new List<StackSync>();
            if (stk != null)
            {
                foreach (var s in stk)
                {
                    try
                    {
                        string ssk = s.stkmfgid;
                        StackSync ss = _context.stackSyncData.Include(e => e.script).Single(e => e.stackID == ssk);
                        if (ss != null)
                            stsyn.Add(ss);
                    }
                    catch { }
                }
            }
           
            return View(stsyn);
        }

        // GET: Dashboard/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackSync = await _context.stackSyncData
                .Include(s => s.script)
                .FirstOrDefaultAsync(m => m.stackID == id);
            if (stackSync == null)
            {
                return NotFound();
            }

            return View(stackSync);
        }

        // GET: Dashboard/Create
        public IActionResult Create()
        {
            ViewData["scriptID"] = new SelectList(_context.scriptlists, "id", "id");
            return View();
        }

        // POST: Dashboard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("stackID,scriptID")] StackSync stackSync)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stackSync);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["scriptID"] = new SelectList(_context.scriptlists, "id", "id", stackSync.scriptID);
            return View(stackSync);
        }

        // GET: Dashboard/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackSync = await _context.stackSyncData.FindAsync(id);
            if (stackSync == null)
            {
                return NotFound();
            }
            ViewData["scriptID"] = new SelectList(_context.scriptlists, "id", "id", stackSync.scriptID);
            return View(stackSync);
        }

        // POST: Dashboard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("stackID,scriptID")] StackSync stackSync)
        {
            if (id != stackSync.stackID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stackSync);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StackSyncExists(stackSync.stackID))
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
            ViewData["scriptID"] = new SelectList(_context.scriptlists, "id", "id", stackSync.scriptID);
            return View(stackSync);
        }

        // GET: Dashboard/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackSync = await _context.stackSyncData
                .Include(s => s.script)
                .FirstOrDefaultAsync(m => m.stackID == id);
            if (stackSync == null)
            {
                return NotFound();
            }

            return View(stackSync);
        }

        // POST: Dashboard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var stackSync = await _context.stackSyncData.FindAsync(id);
            _context.stackSyncData.Remove(stackSync);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StackSyncExists(string id)
        {
            return _context.stackSyncData.Any(e => e.stackID == id);
        }
    }
}
