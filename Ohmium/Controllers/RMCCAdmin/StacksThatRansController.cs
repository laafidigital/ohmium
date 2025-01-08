using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels.LotusModels;
using Ohmium.Models.EFModels.ViewModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
      public class StacksThatRansController : Controller
    {
        private readonly SensorContext _context;

        public StacksThatRansController(SensorContext context)
        {
            _context = context;
        }

        // GET: StacksThatRans
        public async Task<IActionResult> Index()
        {
            return View(await _context.StacksThatRan.ToListAsync());
        }

        // GET: StacksThatRans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stacksThatRan = await _context.StacksThatRan
                .FirstOrDefaultAsync(m => m.stackID == id);
            if (stacksThatRan == null)
            {
                return NotFound();
            }

            return View(stacksThatRan);
        }

        // GET: StacksThatRans/Create
        public IActionResult Create()
        {
            ViewData["devices"] = new SelectList(_context.device, "EqMfgID", "EqMfgID");
            return View();
        }

        
        // POST: StacksThatRans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string deviceID)
        {
            //var stklist =  _context.StacksThatRan.FromSqlRaw("spStacksThatRan p0", deviceID);
            //var stackids = _context.StacksThatRan.Select(e => new
            //{
            //    stackid = e.stackID
            //}).ToArray();
            var stklist = _context.StacksThatRan.FromSqlRaw("select stackMfgID as stackID, deviceID from mtsstackdata where deviceid={0} and timestamp >= {1}", deviceID, DateTime.Now.AddDays(-1));
            foreach (StacksThatRan stk in stklist)
            {
                _context.StacksThatRan.Add(stk);
                _context.SaveChanges();
            }
            return RedirectToAction("index");
        }



        // GET: StacksThatRans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stacksThatRan = await _context.StacksThatRan.FindAsync(id);
            if (stacksThatRan == null)
            {
                return NotFound();
            }
            return View(stacksThatRan);
        }

        // POST: StacksThatRans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("stackID,deviceID")] StacksThatRan stacksThatRan)
        {
            if (id != stacksThatRan.stackID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stacksThatRan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StacksThatRanExists(stacksThatRan.stackID))
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
            return View(stacksThatRan);
        }

        // GET: StacksThatRans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stacksThatRan = await _context.StacksThatRan
                .FirstOrDefaultAsync(m => m.stackID == id);
            if (stacksThatRan == null)
            {
                return NotFound();
            }

            return View(stacksThatRan);
        }

        // POST: StacksThatRans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var stacksThatRan = await _context.StacksThatRan.FindAsync(id);
            _context.StacksThatRan.Remove(stacksThatRan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StacksThatRanExists(string id)
        {
            return _context.StacksThatRan.Any(e => e.stackID == id);
        }

        public async Task<IActionResult> OperationCleanUp()
        {
            ViewBag.stacks= new SelectList(await _context.stack.ToListAsync(), "stackMfgID", "stackMfgID", "Select");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OperationCleanUp(string stack)
        {
            if(stack.Length>0)

            ViewBag.stacks = new SelectList(await _context.stack.ToListAsync(), "stackMfgID", "stackMfgID", "Select");
            return View();
        }
    }
}
