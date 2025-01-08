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
    public class SQStacksController : Controller
    {
        private readonly CacheContext _context;

        public SQStacksController(CacheContext context)
        {
            _context = context;
        }

        public IActionResult ErrorLog()
        {
            ViewBag.Errorcount = _context.deviceDataLog.ToList().Count();
            return View(_context.deviceDataLog.ToList());
        }

        public IActionResult StackDataList()
        {
            List<MTSStackData> stackDataList = _context.mtsStackData.ToList();
            ViewBag.count = stackDataList.Count();
            return View(stackDataList);
        }
        public IActionResult Index(string id, int? data)
        {
            var sensorCtx = new List<SQStack>();
            if (data == null && id == null)
            {
                sensorCtx = _context.stack.Where(x => x.status == 1).ToList();
                ViewBag.status = "Active Stacks";
                return View(sensorCtx);

            }
            else if (data != 1 && id == null)
            {
                ViewBag.status = "InActive Stacks";
                sensorCtx = _context.stack.Where(x => x.status == 3).ToList();
                return View(sensorCtx);
            }
            else if (id != null)
            {
                sensorCtx = _context.stack.Where(x => x.deviceID == id && x.status == 1).ToList();
                ViewBag.status = "Active Stacks";
                ViewBag.dev = id;
                return View(sensorCtx);

            }
            else
            {
                sensorCtx = _context.stack.Where(x => x.deviceID == id && x.status == 1).ToList();
                try
                {
                    int count = id.Length;
                    ViewBag.status = "Active Stacks";
                    ViewBag.dev = id;
                    return View(sensorCtx);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string did = TempData["id"].ToString();
                        var sCtx = _context.stack.Where(x => x.deviceID == did && x.status == 1);
                        ViewBag.status = "Active Stacks";
                        ViewBag.dev = id;
                        return View(sCtx.ToList());
                    }
                    catch
                    {
                        var sContext = _context.stack.Where(d => d.status == 1);
                        ViewBag.status = "Active Stacks";
                        return View(sContext.ToList());
                    }
                }
            }
            //var sensorContext = _context.stack.Include(s => s.device).Include(s => s.sid);
            //return View(await sensorContext.ToListAsync());
        }

        // GET: Stacks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stack = await _context.stack
                .Include(s => s.device)
                .Include(s => s.sid)
                .FirstOrDefaultAsync(m => m.stackMfgID == id);
            if (stack == null)
            {
                return NotFound();
            }

            return View(stack);
        }

        // GET: Stacks/Create
        public IActionResult Create(string deviceID)
        {
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID");
            ViewData["siteID"] = new SelectList(_context.site, "id", "name");
            ViewData["stackConfig"] = new SelectList(_context.sconfig, "configID", "configName");
            ViewData["statusType"] = new SelectList(_context.statusType, "id", "name");
            ViewData["stackPosition"] = new SelectList(new List<SelectListItem>
            {
                //new SelectListItem{Text="A1",Value="A1" },
                //new SelectListItem{Text="A2",Value="A1" },
                //new SelectListItem{Text="A3",Value="A1" },
                //new SelectListItem{Text="B1",Value="B1" },
                //new SelectListItem{Text="B2",Value="B2" },
                //new SelectListItem{Text="B3",Value="B3" },
                //new SelectListItem{Text="C1",Value="C1" },
                //new SelectListItem{Text="C2",Value="C2" },
                //new SelectListItem{Text="C3",Value="C3" }
                new SelectListItem{Text="S01",Value="S01" },
                new SelectListItem{Text="S02",Value="S02" },
                new SelectListItem{Text="S03",Value="S03" },
                new SelectListItem{Text="S04",Value="S04" },
                new SelectListItem{Text="S05",Value="S05" },
                new SelectListItem{Text="S06",Value="S06" }
            }, "Value", "Text");
            return View();
        }

        public JsonResult GetDevice(Guid id)
        {

            var deviceList = _context.device.Where(x => x.siteID == id).Select(x => new
            {
                id = x.EqMfgID,
                text = x.EqMfgID
            });
            return Json(deviceList);
        }

        // POST: Stacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("stackMfgID,siteID,deviceID,stackConfig,stackPosition,status,meaNum,meaArea")] Stack stack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stack);
                await _context.SaveChangesAsync();
                TempData["id"] = stack.deviceID;
                return RedirectToAction(nameof(Index));
            }
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", stack.deviceID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "email", stack.siteID);
            ViewData["stackConfig"] = new SelectList(_context.sconfig, "configID", "configName");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            ViewData["stackPosition"] = new SelectList(new List<SelectListItem>
            {
                //new SelectListItem{Text="A1",Value="A1" },
                //new SelectListItem{Text="A2",Value="A1" },
                //new SelectListItem{Text="A3",Value="A1" },
                //new SelectListItem{Text="B1",Value="B1" },
                //new SelectListItem{Text="B2",Value="B2" },
                //new SelectListItem{Text="B3",Value="B3" },
                //new SelectListItem{Text="C1",Value="C1" },
                //new SelectListItem{Text="C2",Value="C2" },
                //new SelectListItem{Text="C3",Value="C3" }
                new SelectListItem{Text="S01",Value="S01" },
                new SelectListItem{Text="S02",Value="S02" },
                new SelectListItem{Text="S03",Value="S03" },
                new SelectListItem{Text="S04",Value="S04" },
                new SelectListItem{Text="S05",Value="S05" },
                new SelectListItem{Text="S06",Value="S06" }
            }, "Value", "Text");

            return View(stack);
        }

        // GET: Stacks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stack = await _context.stack.FindAsync(id);
            if (stack == null)
            {
                return NotFound();
            }
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", stack.deviceID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "name", stack.siteID);
            ViewData["stackConfig"] = new SelectList(_context.sconfig, "configID", "configName");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            ViewData["stackPosition"] = new SelectList(new List<SelectListItem>
            {
                //new SelectListItem{Text="A1",Value="A1" },
                //new SelectListItem{Text="A2",Value="A1" },
                //new SelectListItem{Text="A3",Value="A1" },
                //new SelectListItem{Text="B1",Value="B1" },
                //new SelectListItem{Text="B2",Value="B2" },
                //new SelectListItem{Text="B3",Value="B3" },
                //new SelectListItem{Text="C1",Value="C1" },
                //new SelectListItem{Text="C2",Value="C2" },
                //new SelectListItem{Text="C3",Value="C3" }
                new SelectListItem{Text="S01",Value="S01" },
                new SelectListItem{Text="S02",Value="S02" },
                new SelectListItem{Text="S03",Value="S03" },
                new SelectListItem{Text="S04",Value="S04" },
                new SelectListItem{Text="S05",Value="S05" },
                new SelectListItem{Text="S06",Value="S06" }
            }, "Value", "Text");
            return View(stack);
        }

        public JsonResult Validate(string id)
        {
            string positions = "THE CHOSEN POSITION IS ALREADY ASSIGNED\n PLEASE UNASSIGN A STACK FROM BELOW TO REPLACE \n Used Edit Function to do so \n";
            string[] divpos = id.Split('_');
            List<SQStack> positionValues = _context.stack.Where(x => x.deviceID == divpos[0] && !x.stackPosition.Contains("UnAssigned")).ToList();
            foreach (SQStack s in positionValues)
            {
                positions += s.stackMfgID + " is assigned to " + s.stackPosition + "\n";
            }
            foreach (object s in positionValues)
            {

            }
            string check = _context.stack.Where(x => x.deviceID == divpos[0] && x.stackPosition == divpos[1]).Count() > 0 ? positions : "Available";

            return Json(check);
        }


        // POST: Stacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("stackMfgID,siteID,deviceID,stackConfig,stackPosition,status,meaNum,meaArea")] Stack stack)
        {
            //if (stack.stackMfgID != stack.stackMfgID)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StackExists(stack.stackMfgID))
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
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", stack.deviceID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "name", stack.siteID);
            ViewData["stackConfig"] = new SelectList(_context.sconfig, "configID", "configName");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            ViewData["stackPosition"] = new SelectList(new List<SelectListItem>
            {
                //new SelectListItem{Text="A1",Value="A1" },
                //new SelectListItem{Text="A2",Value="A1" },
                //new SelectListItem{Text="A3",Value="A1" },
                //new SelectListItem{Text="B1",Value="B1" },
                //new SelectListItem{Text="B2",Value="B2" },
                //new SelectListItem{Text="B3",Value="B3" },
                //new SelectListItem{Text="C1",Value="C1" },
                //new SelectListItem{Text="C2",Value="C2" },
                //new SelectListItem{Text="C3",Value="C3" }
                new SelectListItem{Text="S01",Value="S01" },
                new SelectListItem{Text="S02",Value="S02" },
                new SelectListItem{Text="S03",Value="S03" },
                new SelectListItem{Text="S04",Value="S04" },
                new SelectListItem{Text="S05",Value="S05" },
                new SelectListItem{Text="S06",Value="S06" }
            }, "Value", "Text");

            return View(stack);
        }

        // GET: Stacks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stack = await _context.stack
                .Include(s => s.device)
                .Include(s => s.sid)
                .FirstOrDefaultAsync(m => m.stackMfgID == id);
            if (stack == null)
            {
                return NotFound();
            }

            return View(stack);
        }

        // POST: Stacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Stack stack)
        {
            stack.status = 2;
            stack.stackPosition = "UnAssigned";
            _context.Entry(stack).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StackExists(string id)
        {
            return _context.stack.Any(e => e.stackMfgID == id);
        }
    }
}
