using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.TemplateModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.Templates
{
    public class StackSyncsController : Controller
    {
        private readonly SensorContext _context;

        public StackSyncsController(SensorContext context)
        {
            _context = context;
        }

        // GET: StackSyncs
        public async Task<IActionResult> Index()
        {
            return View(await _context.stackSyncData.Include(e => e.script).ToListAsync());
        }

        // GET: StackSyncs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackSync = await _context.stackSyncData
                .FirstOrDefaultAsync(m => m.stackID == id);
            if (stackSync == null)
            {
                return NotFound();
            }

            return View(stackSync);
        }

        // GET: StackSyncs/Create
        public IActionResult Create()
        {
            ViewBag.site = new SelectList(_context.site, "id", "name");
            ViewBag.teststand = new SelectList(_context.device, "EqMfgID", "EqMfgID");
            ViewBag.stack = new SelectList(_context.stack, "stackMfgID", "stackMfgID");
            ViewBag.script = new SelectList(_context.scriptlists, "id", "scriptName");
            return View();
        }

        public JsonResult GetDevices(Guid id)
        {
            var devices = _context.device.Where(e => e.siteID == id && e.status == 1).Select(e =>
            new
            {
                id = e.EqMfgID,
                text = e.EqMfgID
            });
            return Json(devices);
        }

        public JsonResult GetStacks(string id)
        {
            var stacks = _context.stack.Where(e => e.deviceID == id && e.status == 1).Select(e =>
            new
            {
                id = e.stackMfgID,
                text = e.stackMfgID
            });
            return Json(stacks);
        }

        // POST: StackSyncs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("stackID,scriptID")] StackSync stackSync)
        {
            if (ModelState.IsValid)
            {
                int count = _context.stackSyncData.Where(e => e.stackID == stackSync.stackID).Count();
                if (count == 0)
                    _context.Add(stackSync);
                else
                    _context.Entry(stackSync).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stackSync);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackSync = _context.stackSyncData.FirstOrDefault(e => e.stackID == id);
            if (stackSync == null)
            {
                return NotFound();
            }
            ViewData["script"] = new SelectList(_context.scriptlists, "id", "scriptName");
            return View(stackSync);
        }

        // POST: StackSyncs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StackSync stackSync)
        {
            try
            {
                _context.stackSyncData.RemoveRange(_context.stackSyncData.Where(e => e.stackID == stackSync.stackID));
                _context.SaveChanges();
                _context.stackSyncData.Add(stackSync);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
            return View(stackSync);
        }

        public IActionResult Delete(string stkid)
        {
            return View(_context.stackSyncData.Single(e=>e.stackID==stkid));
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(string stkid)
        {
            _context.stackSyncData.RemoveRange(_context.stackSyncData.Where(e => e.stackID == stkid));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: StackSyncs/Delete/5

        private bool StackSyncExists(string id)
        {
            return _context.stackSyncData.Any(e => e.stackID == id);
        }
    }
}
