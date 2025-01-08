using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunStepsController : ControllerBase
    {
        private readonly SensorContext _context;

        public RunStepsController(SensorContext context)
        {
            _context = context;
        }

        // GET: api/RunSteps
        [HttpGet]
        public ActionResult<IEnumerable<RunStep>> GetRunSteps(string stkmfgid)
        {
            int stkprf = _context.stkRunProfile.FirstOrDefault(e => e.stackID == stkmfgid).id;
           List<RunStepTemplate> srp = _context.runStepTemplate.Where(e => e.stkRunProfileID == stkprf).Include(e=>e.srp).Include(e=>e.rstg).ToList();
            return Ok(srp);
        }

   
        private bool RunStepExists(int id)
        {
            return _context.stkStep.Any(e => e.id == id);
        }
    }
}
