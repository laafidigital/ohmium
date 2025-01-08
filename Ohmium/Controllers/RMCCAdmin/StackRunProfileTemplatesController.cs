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
    public class StackRunProfileTemplatesController : Controller
    {
        private readonly SensorContext _context;

        public StackRunProfileTemplatesController(SensorContext context)
        {
            _context = context;
        }

        // GET: StackRunProfileTemplates
        public IActionResult Index(int? id, int? data)
        {

            if (id != null)
            {
                TempData["id"] = id;
                if (data != null)
                {
                    ViewBag.msg = "InActive SRPT";
                    return View(_context.stackRunProfileTemplate.Where(x => x.profileID == id && x.status == 2).Include(s => s.profile).ToList());
                }
                else
                {
                    ViewBag.msg = "Active SRPT";
                    return View(_context.stackRunProfileTemplate.Where(x => x.profileID == id && x.status != 2).Include(s => s.profile).ToList());
                }
            }
            else
            {
                var st = new List<StackRunProfileTemplate>();
                try
                {
                    int pid = int.Parse(TempData["id"].ToString());
                    if (data != null)
                    {
                        st = _context.stackRunProfileTemplate.Where(x => x.profileID == pid && x.status == 2).Include(s => s.profile).ToList();
                        ViewBag.msg = "InActive SRPT";
                    }
                    else
                    {
                        ViewBag.msg = "Active SRPT";
                        st = _context.stackRunProfileTemplate.Where(x => x.profileID == pid && x.status != 2).Include(s => s.profile).ToList();
                    }
                }
                catch
                {
                    if (data != null)
                    {
                        st = _context.stackRunProfileTemplate.Where(x => x.status == 2).Include(s => s.profile).ToList();
                        ViewBag.msg = "InActive SRPT";
                    }
                    else
                    {
                        st = _context.stackRunProfileTemplate.Where(x => x.status != 2).Include(s => s.profile).ToList();
                        ViewBag.msg = "Active SRPT";
                    }
                }
                return View(st);
            }
        }

        public IActionResult StackTemplateSync()
        {
            return View();
        }

        // GET: StackRunProfileTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackRunProfileTemplate = await _context.stackRunProfileTemplate
                .Include(s => s.profile)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stackRunProfileTemplate == null)
            {
                return NotFound();
            }

            return View(stackRunProfileTemplate);
        }

        // GET: StackRunProfileTemplates/Create
        public IActionResult Create()
        {
            try
            {
                int id = int.Parse(TempData["id"].ToString());
                ViewData["status"] = new SelectList(_context.statusType, "id", "name");
                ViewData["profileID"] = new SelectList(_context.runProfileTemplate.Where(e => e.status == 1 && e.id == id), "id", "profileName");
            }
            catch
            {
                ViewData["status"] = new SelectList(_context.statusType, "id", "name");
                ViewData["profileID"] = new SelectList(_context.runProfileTemplate.Where(e => e.status == 1), "id", "profileName");
            }
            return View();
        }

        // POST: StackRunProfileTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,profileID,command,status")] StackRunProfileTemplate stackRunProfileTemplate)
        {
            if (ModelState.IsValid)
            {
                stackRunProfileTemplate.loop = false;
                _context.Add(stackRunProfileTemplate);
                await _context.SaveChangesAsync();
                TempData["id"] = stackRunProfileTemplate.profileID;
                return RedirectToAction(nameof(Index));
            }
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            ViewData["profileID"] = new SelectList(_context.runProfileTemplate, "id", "profileName", stackRunProfileTemplate.profileID);
            return View(stackRunProfileTemplate);
        }

        // GET: StackRunProfileTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackRunProfileTemplate = await _context.stackRunProfileTemplate.FindAsync(id);
            if (stackRunProfileTemplate == null)
            {
                return NotFound();
            }
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            ViewData["profileID"] = new SelectList(_context.runProfileTemplate, "id", "id", stackRunProfileTemplate.profileID);
            return View(stackRunProfileTemplate);
        }

        // POST: StackRunProfileTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,profileID,loop,command,status")] StackRunProfileTemplate stackRunProfileTemplate)
        {
            if (id != stackRunProfileTemplate.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    stackRunProfileTemplate.loop = false;
                    _context.Update(stackRunProfileTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StackRunProfileTemplateExists(stackRunProfileTemplate.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), stackRunProfileTemplate.profileID);
            }
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            ViewData["profileID"] = new SelectList(_context.runProfileTemplate, "id", "id", stackRunProfileTemplate.profileID);
            return View(stackRunProfileTemplate);
        }

        // GET: StackRunProfileTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackRunProfileTemplate = await _context.stackRunProfileTemplate
                .Include(s => s.profile)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stackRunProfileTemplate == null)
            {
                return NotFound();
            }

            return View(stackRunProfileTemplate);
        }

        // POST: StackRunProfileTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stackRunProfileTemplate = await _context.stackRunProfileTemplate.FindAsync(id);
            stackRunProfileTemplate.status = 2;
            _context.Entry(stackRunProfileTemplate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StackRunProfileTemplateExists(int id)
        {
            return _context.stackRunProfileTemplate.Any(e => e.id == id);
        }
    }
}
