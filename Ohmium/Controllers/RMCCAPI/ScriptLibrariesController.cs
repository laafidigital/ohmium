using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.TemplateModels;
using efmodels=Ohmium.Models.EFModels;
using Ohmium.Repository;
using Ohmium.Models.EFModels.ViewModels;

namespace Ohmium.Controllers.RMCCAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptLibrariesController : ControllerBase
    {
        private readonly SensorContext _context;

        public ScriptLibrariesController(SensorContext context)
        {
            _context = context;
        }

        // GET: api/ScriptLibraries
        [HttpGet("{teststandid}")]
        public async Task<ActionResult<ScriptLibrary>> GetScriptLibrary(string teststandid)
        {
            List<efmodels.Stack> stkids = new List<efmodels.Stack>();
            try
            {
                stkids = _context.stack.Where(e => e.deviceID == teststandid && e.status == 1).ToList();
            }
            catch(Exception ex)
            {

            }

            List<StackSync> ssl = new List<StackSync>();
            if (stkids != null && stkids.Count > 0)
            {
                foreach (efmodels.Stack stk in stkids)
                {
                    StackSync stackSync = _context.stackSyncData.FirstOrDefault(e => e.stackID == stk.stackMfgID);
                    if(stackSync != null)
                    ssl.Add(stackSync);
                }
                List<ScriptViewModel> svml = new List<ScriptViewModel>();

                List<RunStepLibrary> rsl = new List<RunStepLibrary>();
                foreach (StackSync ss in ssl)
                {
                    List<scripStepViewModel> sstepvml = new List<scripStepViewModel>();
                    ScriptViewModel svm = new ScriptViewModel();
                    svm.stackID = ss.stackID;
                    svm.scriptID = ss.scriptID;
                    svm.testStandID = teststandid;
                    svm.MEA = _context.stack.Single(e => e.stackMfgID == ss.stackID).meaNum;
                    List<ScriptLibrary> scl = _context.scriptLibrary.Where(e => e.scriptId == ss.scriptID).ToList();
                    foreach (ScriptLibrary sl in scl)
                    {


                        scripStepViewModel sstepvm = new scripStepViewModel();
                        sstepvm.scriptID = ss.scriptID;
                        sstepvm.stepLoop = sl.phaseLoop.Value;
                        sstepvm.stepNo = sl.stepNumber;
                        string[] steprs = sl.runStepLibraryWithLoop.Split(',');

                        int count = steprs.Count();
                        List<RunStepListViewModel> rsteplvmlist = new List<RunStepListViewModel>();
                        for (int i = 0; i < count; i++)
                        {
                            RunStepListViewModel rsteplvm = new RunStepListViewModel();
                            try
                            {
                                string[] loop = steprs[i].Split(':');
                                rsteplvm.stepLoop = int.Parse(loop[1].TrimEnd('}'));
                            rsteplvm.stepNo = i + 1;
                                string[] n = steprs[i].Split(':');
                                string name1 = n[0].Trim('{');
                                string name2 = name1.Trim('\"');
                                string name3 = name2.Trim('\\');
                                int sid = _context.sequencyLibrary.Single(e => e.sequenceName == name3).id;
                                rsteplvm.runsteps = _context.runstepLibrary.Where(e => e.seqMasterId == sid).ToList();
                                rsteplvmlist.Add(rsteplvm);
                            }
                            catch(Exception ex) { }
                        }
                        sstepvm.runstepListViewModel = rsteplvmlist;
                        sstepvml.Add(sstepvm);
                    }
                    svm.stk = _context.stack.Single(e => e.stackMfgID == svm.stackID);
                    svm.div = _context.device.Single(e => e.EqMfgID == svm.testStandID);
                    svm.Scripts = sstepvml;
                    svml.Add(svm);
                }

                return Ok(svml);
            }
            else
                return NotFound();
        }
    }
}
