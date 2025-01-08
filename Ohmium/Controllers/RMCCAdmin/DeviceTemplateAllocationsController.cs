using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class DeviceTemplateAllocationsController : Controller
    {
        private readonly SensorContext _context;

        public DeviceTemplateAllocationsController(SensorContext context)
        {
            _context = context;
        }

        // GET: DeviceTemplateAllocations
        public async Task<IActionResult> Index()
        {
            List<DeviceTemplateAllocation> sensorContext = await _context.deviceTemplateAllocation.Include(d => d.device).Include(d => d.rpt).Include(d => d.srpt).Include(d => d.stack).ToListAsync();
            foreach (DeviceTemplateAllocation dta in sensorContext)
            {
                dta.rpt = _context.runProfileTemplate.Find(dta.templateID);
            }
            return View(sensorContext);
        }

        // GET: DeviceTemplateAllocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceTemplateAllocation = await _context.deviceTemplateAllocation
                .Include(d => d.device)
                .Include(d => d.srpt)
                .Include(d => d.stack)
                .FirstOrDefaultAsync(m => m.id == id);
            if (deviceTemplateAllocation == null)
            {
                return NotFound();
            }

            return View(deviceTemplateAllocation);
        }

        // GET: DeviceTemplateAllocation    s/Create
        public IActionResult Create()
        {
            //var devList = _context.device.Where(d => !_context.runProfile.Any(rp => rp.device.EqMfgID == d.EqMfgID)).ToList();
            var devList = _context.device.ToList();

            //ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID");
            ViewData["deviceID"] = new SelectList(devList.Where(x=>x.status==1), "EqMfgID", "EqMfgID", devList.FirstOrDefault());
            ViewData["stackRunProfileTemplateID"] = new SelectList(_context.stackRunProfileTemplate.Where(x=>x.status==1), "id", "name");
            ViewData["stackID"] = new SelectList(_context.stack.Where(x=>x.status==1), "stackMfgID", "stackMfgID");
            ViewData["templateID"] = new SelectList(_context.runProfileTemplate.Where(x=>x.status==1), "id", "profileName");
            return View();
        }

        // POST: DeviceTemplateAllocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public JsonResult DeviceSelection(string divid)
        {
            bool check = _context.runProfile.Where(x => x.deviceID == divid).Count() > 0 ? true:false;
            return Json(check);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,deviceID,templateID,stackID,stackRunProfileTemplateID")] DeviceTemplateAllocation deviceTemplateAllocation)
        {
            if (ModelState.IsValid)
            {
                //using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                //{
                try
                {
                    RunProfile rp = new RunProfile();
                    rp = _context.runProfile.FirstOrDefault(x => x.deviceID == deviceTemplateAllocation.deviceID);
                    RunProfileTemplate rprofT = _context.runProfileTemplate.Find(deviceTemplateAllocation.templateID);
                    rp.dCmd = rprofT.dCmd;
                    rp.profileName = rprofT.profileName;
                    rp.timeStamp = DateTime.Now;
                    rp.fan = rprofT.fan;
                    rp.mnWbT = rprofT.mnWbT;
                    rp.mxWbT = rprofT.mxWbT;
                    rp.hxT = rprofT.hxT;
                    rp.pump = rprofT.pump;
                    _context.Entry(rp).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    RunProfile rp = new RunProfile();
                    RunProfileTemplate rprofT = _context.runProfileTemplate.Find(deviceTemplateAllocation.templateID);
                    rp.dCmd = rprofT.dCmd;
                    rp.deviceID = deviceTemplateAllocation.deviceID;
                    rp.profileName = rprofT.profileName;
                    rp.timeStamp = DateTime.Now;
                    rp.fan = rprofT.fan;
                    rp.mnWbT = rprofT.mnWbT;
                    rp.mxWbT = rprofT.mxWbT;
                    rp.hxT = rprofT.hxT;
                    rp.pump = rprofT.pump;
                    _context.Entry(rp).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }
                List<RunStepTemplate> rstL = new List<RunStepTemplate>();
                List<RunStep> lrs = new List<RunStep>();
               
                StackRunProfile srp = _context.stkRunProfile.FirstOrDefault(x => x.stackID == deviceTemplateAllocation.stackID);
                if (srp != null)
                {
                    StackRunProfileTemplate srpfl = _context.stackRunProfileTemplate.Find(deviceTemplateAllocation.stackRunProfileTemplateID);
                    srp.command = srpfl.command;
                    srp.loop = srpfl.loop;
                    srp.name = srpfl.name;
                    srp.stackPosition = _context.stack.Find(deviceTemplateAllocation.stackID).stackPosition;
                    _context.Entry(srp).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    rstL = _context.runStepTemplate.Where(x => x.stkRunProfileID == srpfl.id).ToList();
                    lrs = _context.stkStep.Where(x => x.stkRunProfileID == srp.id).ToList();
                }
                else
                {
                    StackRunProfile srp1 = new StackRunProfile();
                    try
                    {
                        srp1 = _context.stkRunProfile.FirstOrDefault(x => x.stackID == deviceTemplateAllocation.stackID);
                        srp1.stackPosition = _context.stack.Find(deviceTemplateAllocation.stackID).stackPosition;
                    }
                    catch { }
                    if(srp1==null)
                        srp1 = new StackRunProfile();
                    StackRunProfileTemplate srpf2 = _context.stackRunProfileTemplate.Find(deviceTemplateAllocation.stackRunProfileTemplateID);
                        srp1.loop = srpf2.loop;
                    srp1.command = srpf2.command;
                    srp1.name = srpf2.name;
                    srp1.profileID = _context.runProfile.Single(x=>x.deviceID==deviceTemplateAllocation.deviceID).id;
                    srp1.stackID = deviceTemplateAllocation.stackID;
                    _context.Entry(srp1).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    rstL = _context.runStepTemplate.Where(x => x.stkRunProfileID == srpf2.id).ToList();
                    lrs = _context.stkStep.Where(x => x.stkRunProfileID == srp1.id).ToList();
                }

                //StackRunProfileTemplate srpt = _context.stackRunProfileTemplate.FirstOrDefault(x => x.id == deviceTemplateAllocation.stackRunProfileTemplateID);
                //_context.stkStep.DeleteAllOnSubmit(lrs);
                try
                {
                    int ct = lrs.Count;
                    for (int i = ct-1; i >=0; i--)
                    {
                        _context.stkStep.Remove(lrs[i]);
                        _context.SaveChanges();
                    }
                }
                catch
                {

                }
                RunProfileTemplate rpt = _context.runProfileTemplate.Find(deviceTemplateAllocation.templateID);
                try
                {
                    foreach (RunStepTemplate rst in rstL)
                    {
                        //RunStep rs = _context.stkStep.FirstOrDefault(x => x.stepNumber == rst.stepNumber);
                        //if (rs != null)
                        //{
                        //    rs.stkRunProfileID = _context.stkRunProfile.FirstOrDefault(x => x.stackID == deviceTemplateAllocation.stackID).id;
                        //    rs.cI = rst.cI;
                        //    rs.cV = rst.cV;
                        //    rs.cVt = rst.cVt;
                        //    rs.duration = rst.duration;
                        //    rs.hP = rst.hP;
                        //    rs.sCmd = rst.sCmd;
                        //    rs.wFt = rst.wFt;
                        //    rs.wP = rst.wP;
                        //    rs.wTt = rst.wTt;
                        //    _context.Entry(rs).State = EntityState.Modified;
                        //    await _context.SaveChangesAsync();
                        //}
                        //else
                        //{
                        RunStep rs1 = new RunStep();
                        rs1.stkRunProfileID = _context.stkRunProfile.FirstOrDefault(x => x.stackID == deviceTemplateAllocation.stackID).id;
                        rs1.cI = rst.cI;
                        rs1.cV = rst.cV;
                        rs1.cVt = rst.cVt;
                        rs1.duration = rst.duration;
                        rs1.hP = rst.hP;
                        rs1.sCmd = rst.sCmd;
                        rs1.wFt = rst.wFt;
                        rs1.wP = rst.wP;
                        rs1.wTt = rst.wTt;
                        rs1.stepNumber = rst.stepNumber;
                        rs1.imA = rst.imA;
                        rs1.imF = rst.imF;
                        rs1.rstGID = rst.rstGID;
                        _context.stkStep.Add(rs1);
                        await _context.SaveChangesAsync();
                    }
                }
                //    }
                //}
                catch (Exception ex)
                {
                    rstL = new List<RunStepTemplate>();
                }
                _context.Add(deviceTemplateAllocation);
                await _context.SaveChangesAsync();
                //transactionScope.Complete();
                //transactionScope.Dispose();
                //}
                return RedirectToAction(nameof(Index));
            }

            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", deviceTemplateAllocation.deviceID);
            ViewData["stackRunProfileTemplateID"] = new SelectList(_context.stackRunProfileTemplate, "id", "name", deviceTemplateAllocation.stackRunProfileTemplateID);
            ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID", deviceTemplateAllocation.stackID);
            ViewData["templateID"] = new SelectList(_context.runProfileTemplate, "id", "profileName");

            return View(deviceTemplateAllocation);
        }

        // GET: DeviceTemplateAllocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceTemplateAllocation = await _context.deviceTemplateAllocation.FindAsync(id);
            if (deviceTemplateAllocation == null)
            {
                return NotFound();
            }
            int tempid = _context.deviceTemplateAllocation.Find(id).templateID;
            int profid = _context.deviceTemplateAllocation.Find(id).stackRunProfileTemplateID;
            TempData["template"] = _context.runProfileTemplate.Find(tempid).profileName;
            TempData["proftemp"] = _context.stackRunProfileTemplate.Single(x=>x.id==profid).name;
            ViewData["template"] = new SelectList(_context.runProfileTemplate, "id", "profileName");
            ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", deviceTemplateAllocation.deviceID);
            ViewData["stackRunProfileTemplateID"] = new SelectList(_context.stackRunProfileTemplate, "id", "name", deviceTemplateAllocation.stackRunProfileTemplateID);
            ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID", deviceTemplateAllocation.stackID);
            return View(deviceTemplateAllocation);
        }

        // POST: DeviceTemplateAllocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,deviceID,templateID,stackID,stackRunProfileTemplateID")] DeviceTemplateAllocation deviceTemplateAllocation)
        {
            if (id != deviceTemplateAllocation.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                //{
                try
                {
                    RunProfile rp = new RunProfile();
                    rp = _context.runProfile.FirstOrDefault(x => x.deviceID == deviceTemplateAllocation.deviceID);
                    RunProfileTemplate rprofT = _context.runProfileTemplate.Find(deviceTemplateAllocation.templateID);
                    rp.dCmd = rprofT.dCmd;
                    rp.profileName = rprofT.profileName;
                    rp.timeStamp = DateTime.Now;
                    rp.fan = rprofT.fan;
                    rp.mnWbT = rprofT.mnWbT;
                    rp.mxWbT = rprofT.mxWbT;
                    rp.hxT = rprofT.hxT;
                    _context.Entry(rp).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    RunProfile rp = new RunProfile();
                    RunProfileTemplate rprofT = _context.runProfileTemplate.Find(deviceTemplateAllocation.templateID);
                    rp.dCmd = rprofT.dCmd;
                    rp.deviceID = deviceTemplateAllocation.deviceID;
                    rp.profileName = rprofT.profileName;
                    rp.timeStamp = DateTime.Now;
                    rp.fan = rprofT.fan;
                    rp.mnWbT = rprofT.mnWbT;
                    rp.mxWbT = rprofT.mxWbT;
                    rp.hxT = rprofT.hxT;
                    _context.Entry(rp).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }
                List<RunStepTemplate> rstL = new List<RunStepTemplate>();
                List<RunStep> lrs = new List<RunStep>();

                StackRunProfile srp = new StackRunProfile();
                srp = _context.stkRunProfile.FirstOrDefault(x => x.stackID == deviceTemplateAllocation.stackID);
                if (srp != null)
                {
                    StackRunProfileTemplate srpfl = _context.stackRunProfileTemplate.Find(deviceTemplateAllocation.stackRunProfileTemplateID);
                    srp.command = srpfl.command;
                    srp.loop = srpfl.loop;
                    srp.name = srpfl.name;
                    _context.Entry(srp).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    rstL = _context.runStepTemplate.Where(x => x.stkRunProfileID == srpfl.id).ToList();
                    lrs = _context.stkStep.Where(x => x.stkRunProfileID == srp.id).ToList();
                }
                else
                {
                    StackRunProfile srp1 = new StackRunProfile();
                    StackRunProfileTemplate srpf2 = _context.stackRunProfileTemplate.Find(deviceTemplateAllocation.stackRunProfileTemplateID);
                    srp1.loop = srpf2.loop;
                    srp1.command = srpf2.command;
                    srp1.name = srpf2.name;
                    srp1.profileID = _context.runProfile.Single(x => x.deviceID == deviceTemplateAllocation.deviceID).id;
                    srp1.stackID = deviceTemplateAllocation.stackID;
                    _context.Entry(srp1).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    rstL = _context.runStepTemplate.Where(x => x.stkRunProfileID == srpf2.id).ToList();
                    lrs = _context.stkStep.Where(x => x.stkRunProfileID == srp1.id).ToList();
                }

                //StackRunProfileTemplate srpt = _context.stackRunProfileTemplate.FirstOrDefault(x => x.id == deviceTemplateAllocation.stackRunProfileTemplateID);
                try
                {
                    int ct = lrs.Count;
                    for (int i = ct - 1; i >= 0; i--)
                    {
                        _context.stkStep.Remove(lrs[i]);
                        _context.SaveChanges();
                    }
                }
                catch
                {

                }
                RunProfileTemplate rpt = _context.runProfileTemplate.Find(deviceTemplateAllocation.templateID);
                try
                {
                    foreach (RunStepTemplate rst in rstL)
                    {
                        RunStep rs = _context.stkStep.FirstOrDefault(x => x.stepNumber == rst.stepNumber);
                        if (rs != null)
                        {
                            rs.stkRunProfileID = _context.stkRunProfile.FirstOrDefault(x => x.stackID == deviceTemplateAllocation.stackID).id;
                            rs.cI = rst.cI;
                            rs.cV = rst.cV;
                            rs.cVt = rst.cVt;
                            rs.duration = rst.duration;
                            rs.hP = rst.hP;
                            rs.sCmd = rst.sCmd;
                            rs.wFt = rst.wFt;
                            rs.wP = rst.wP;
                            rs.wTt = rst.wTt;
                            rs.rstGID = rst.rstGID;
                            rs.stepNumber = rst.stepNumber;
                            rs.imF = rst.imF;
                            rs.imA = rst.imA;
                            _context.Entry(rs).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            RunStep rs1 = new RunStep();
                            rs1.stkRunProfileID = _context.stkRunProfile.FirstOrDefault(x => x.stackID == deviceTemplateAllocation.stackID).id; ;
                            rs1.cI = rst.cI;
                            rs1.cV = rst.cV;
                            rs1.cVt = rst.cVt;
                            rs1.duration = rst.duration;
                            rs1.hP = rst.hP;
                            rs1.sCmd = rst.sCmd;
                            rs1.wFt = rst.wFt;
                            rs1.wP = rst.wP;
                            rs1.wTt = rst.wTt;
                            rs1.stepNumber = rst.stepNumber;
                            rs1.rstGID = rst.rstGID;
                            _context.stkStep.Add(rs1);
                            await _context.SaveChangesAsync();

                        }
                    }
                }
                catch (Exception ex)
                {
                    rstL = new List<RunStepTemplate>();
                }
                _context.Entry(deviceTemplateAllocation).State=EntityState.Modified;
                await _context.SaveChangesAsync();
            
                    //transactionScope.Complete();
                    //transactionScope.Dispose();
                    //}
                    return RedirectToAction(nameof(Index));
            }
        ViewData["deviceID"] = new SelectList(_context.device, "EqMfgID", "EqMfgID", deviceTemplateAllocation.deviceID);
        ViewData["stackRunProfileTemplateID"] = new SelectList(_context.stackRunProfileTemplate, "id", "name", deviceTemplateAllocation.stackRunProfileTemplateID);
        ViewData["stackID"] = new SelectList(_context.stack, "stackMfgID", "stackMfgID", deviceTemplateAllocation.stackID);
        ViewData["template"] = new SelectList(_context.runProfileTemplate, "id", "profileName");
            return View(deviceTemplateAllocation);

    }

    // GET: DeviceTemplateAllocations/Delete/5
    public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceTemplateAllocation = await _context.deviceTemplateAllocation
                .Include(d => d.device)
                .Include(d => d.srpt)
                .Include(d => d.stack)
                .FirstOrDefaultAsync(m => m.id == id);
            if (deviceTemplateAllocation == null)
            {
                return NotFound();
            }

            return View(deviceTemplateAllocation);
        }

        // POST: DeviceTemplateAllocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deviceTemplateAllocation = await _context.deviceTemplateAllocation.FindAsync(id);
            _context.deviceTemplateAllocation.Remove(deviceTemplateAllocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetStackByID(string id)
        {
            var stackList = _context.stack.Where(x => x.deviceID == id).Select(x => new
            {
                id = x.stackMfgID,
                text = x.stackMfgID
            });
            return Json(stackList);            
        }

        private bool DeviceTemplateAllocationExists(int id)
        {
            return _context.deviceTemplateAllocation.Any(e => e.id == id);
        }
    }

}
