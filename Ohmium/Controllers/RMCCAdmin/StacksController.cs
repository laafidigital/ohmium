using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Models.EFModels.LotusModels;
using Ohmium.Models.TemplateModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class StacksController : Controller
    {
        private readonly SensorContext _context;

        public StacksController(SensorContext context)
        {
            _context = context;
        }

        // GET: Stacks
        public IActionResult Index(string id,int? data)
        {
            var sensorCtx=new List<Stack>();
            if(data==null && id == null)
            {
                sensorCtx = _context.stack.Where(x=>x.status == 1).Include(s => s.device).ToList();
                ViewBag.status = "Active Stacks";
                return View(sensorCtx);

            }
            else if (data != 1 && id==null)
            {
                ViewBag.status = "InActive Stacks";
                sensorCtx = _context.stack.Where(x => x.status == 3 || x.status == 2).Include(s => s.device).ToList();
                return View(sensorCtx);
            }
            else if (id != null)
            {
                sensorCtx = _context.stack.Where(x => x.deviceID == id && x.status == 1).Include(s => s.device).ToList();
                ViewBag.status = "Active Stacks";
                return View(sensorCtx);

            }
            else
            {
                sensorCtx = _context.stack.Where(x => x.deviceID == id && x.status ==1).Include(s => s.device).ToList();
                try
                {
                    int count = id.Length;
                    ViewBag.status = "Active Stacks";
                    return View(sensorCtx);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string did = TempData["id"].ToString();
                        var sCtx = _context.stack.Where(x => x.deviceID == did && x.status == 1).Include(s => s.device);
                        ViewBag.status = "Active Stacks";
                        return View(sCtx.ToList());
                    }
                    catch
                    {
                        var sContext = _context.stack.Where(d => d.status == 1).Include(s => s.device);
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
            ViewData["statusType"] = new SelectList(_context.statusType,"id","name");
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
                new SelectListItem{Text="UnAssigned",Value="UnAssigned" },
                new SelectListItem{Text="S01",Value="S01" },
                new SelectListItem{Text="S02",Value="S02" },
                new SelectListItem{Text="S03",Value="S03" },
                new SelectListItem{Text="S04",Value="S04" },
                new SelectListItem{Text="S05",Value="S05" },
                new SelectListItem{Text="S06",Value="S06" }
            }, "Value", "Text");
            ViewData["Script"] = new SelectList(_context.scriptlists, "id", "scriptName");
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
        public async Task<IActionResult> Create([Bind("stackMfgID,siteID,deviceID,stackConfig,stackPosition,status,meaNum,meaArea,command")] Stack stack)
        {
            if (ModelState.IsValid)
            {
                stack.status = stack.command == "Decommissioned" ? 4 : stack.command == "Paused" ? 2 : stack.command == "Start" ? 1 : 3;
                _context.Add(stack);
                await _context.SaveChangesAsync();
                TempData["id"] = stack.deviceID;
                StackSync ss = new StackSync()
                {
                    stackID = stack.stackMfgID,
                    scriptID = int.Parse(Request.Form["script"].ToString())
                };
                bool check = _context.stackSyncData.FirstOrDefault(e => e.stackID == ss.stackID) == null ? false : true;
                if (check)
                    _context.Entry(ss).State = EntityState.Modified;
                else
                    _context.stackSyncData.Add(ss);
                _context.SaveChanges();
                try
                {
                    StacksThatRan str = new StacksThatRan()
                    {
                        stackID = stack.stackMfgID,
                        deviceID = stack.deviceID,
                    };
                    _context.StacksThatRan.Add(str);
                    _context.SaveChanges();
                }
                catch { };
                return RedirectToAction(nameof(Index));
            }
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", stack.deviceID);
            ViewData["siteID"] = new SelectList(_context.site, "id", "email", stack.siteID);
            ViewData["stackConfig"] = new SelectList(_context.sconfig, "configID", "configName");
            ViewData["status"] = new SelectList(_context.statusType, "id", "name");
            ViewData["stackPosition"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text="UnAssigned",Value="UnAssigned" },
                new SelectListItem{Text="S01",Value="S01" },
                new SelectListItem{Text="S02",Value="S02" },
                new SelectListItem{Text="S03",Value="S03" },
                new SelectListItem{Text="S04",Value="S04" },
                new SelectListItem{Text="S05",Value="S05" },
                new SelectListItem{Text="S06",Value="S06" }
            }, "Value", "Text");
            ViewData["Script"] = new SelectList(_context.scriptlists, "id", "scriptName");
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
                new SelectListItem{Text="Unassigned",Value="Unassigned" },
                new SelectListItem{Text="S01",Value="S01" },
                new SelectListItem{Text="S02",Value="S02" },
                new SelectListItem{Text="S03",Value="S03" },
                new SelectListItem{Text="S04",Value="S04" },
                new SelectListItem{Text="S05",Value="S05" },
                new SelectListItem{Text="S06",Value="S06" }
            }, "Value", "Text",stack.stackPosition);
            try
            {
                int scriptid = _context.stackSyncData.Single(e => e.stackID == stack.stackMfgID).scriptID;

                ViewData["Script"] = new SelectList(_context.scriptlists, "id", "scriptName", scriptid);
            }
            catch
            {
                ViewData["Script"] = new SelectList(_context.scriptlists, "id", "scriptName","Select");
            }
            return View(stack);
        }

        public JsonResult Validate(string id)
        {
            string positions = "THE CHOSEN POSITION IS ALREADY ASSIGNED\n PLEASE UNASSIGN A STACK FROM BELOW TO REPLACE \n Used Edit Function to do so \n";
            string[] divpos = id.Split('_');
            List<Stack> positionValues = _context.stack.Where(x => x.deviceID == divpos[0] && !x.stackPosition.Contains("UnAssigned")).ToList();
            foreach(Stack s in positionValues)
            {
                positions += s.stackMfgID + " is assigned to " + s.stackPosition + "\n";
            }
            //foreach(object s in positionValues)
            //{

            //}
            string check = _context.stack.Where(x => x.deviceID == divpos[0] && x.stackPosition == divpos[1]).Count() > 0 ? positions : "Available";

            return Json(check);
        }


        // POST: Stacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Stack stack)
        {
            //if (stack.stackMfgID != stack.stackMfgID)
            //{
            //    return NotFound();
            //}
            //stack.status = stack.command == "Decommissioned" ? 4 : stack.command == "Paused" ? 2 : stack.command == "Start" ? 1 : 3;

            if (ModelState.IsValid)
            {
                StackTestRunHours strh = new StackTestRunHours();
                strh.stkMfgId = stack.stackMfgID;
                strh.timeStampUTC = DateTime.Now.ToUniversalTime();
                string stkid = strh.stkMfgId;
                string divid = stack.deviceID;
                //MTSStackData stkdata = new MTSStackData
                //{
                //    stackMfgID = stkid,
                //    deviceID = devid
                //};
                MTSStackDataNew tssd = _context.mtsStackDataNew.Where(e => e.stackMfgID == stkid && e.deviceID == divid).OrderByDescending(e => e.timeStamp).FirstOrDefault();
                if (tssd != null)
                    strh.cumulativeHours = tssd.cumulativeHours;
                else
                    try
                    {
                        strh.cumulativeHours = _context.stackTestRunHours.OrderByDescending(e => e.cumulativeHours).Single(e => e.stkMfgId == strh.stkMfgId).cumulativeHours;
                    }
                    catch {
                        strh.cumulativeHours = 0;
                            }
                        _context.stackTestRunHours.Add(strh);
                _context.SaveChanges();

                try
                {
                    StackSync ss = new StackSync()
                    {
                        stackID = stack.stackMfgID,
                        scriptID = int.Parse(Request.Form["script"].ToString())
                    };
                    bool check = _context.stackSyncData.FirstOrDefault(e => e.stackID == ss.stackID) == null ? false : true;
                    if (check)
                    {
                        _context.RemoveRange(_context.stackSyncData.Where(e => e.stackID == ss.stackID));
                        _context.SaveChanges();
                    }
                        _context.stackSyncData.Add(ss);
                        _context.SaveChanges();
                    _context.Update(stack);
                    await _context.SaveChangesAsync();
                    StacksThatRan stran = new StacksThatRan()
                    {
                        stackID = stack.stackMfgID,
                        deviceID = stack.deviceID
                    };

                    if (_context.StacksThatRan.Where(e => e.stackID == stack.stackMfgID && stack.deviceID==stack.deviceID).ToList().Count ==0)
                    {
                        _context.StacksThatRan.Add(stran);
                    }
                    _context.SaveChanges();
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
            try
            {
                int scriptid = _context.stackSyncData.Single(e => e.stackID == stack.stackMfgID).scriptID;

                ViewData["Script"] = new SelectList(_context.scriptlists, "id", "scriptName", scriptid);
            }
            catch
            {
                ViewData["Script"] = new SelectList(_context.scriptlists, "id", "scriptName", "Select");
            }
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
