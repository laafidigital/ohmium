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
    public class StackPhaseSettingsController : ControllerBase
    {
        private readonly SensorContext _context;

        public StackPhaseSettingsController(SensorContext context)
        {
            _context = context;
        }

        // GET: api/StackPhaseSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StackPhaseSetting>>> GetstkPhaseSetting()
        {
            return await _context.stkPhaseSetting.ToListAsync();
        }

        // GET: api/StackPhaseSettings/5
        [HttpGet("{stackID}")]
        public ActionResult<List<StackPhaseSetting>> GetStackPhaseSetting(string stackID)
        {
            var stackPhaseSetting = _context.stkPhaseSetting.Where(x => x.stackID == stackID).Include(x => x.rsg).Include(x => x.stk).ToList();

            if (stackPhaseSetting == null)
            {
                return NotFound();
            }

            return stackPhaseSetting;
        }

        //[HttpGet, Route("TestScriptNew")]
        //public async Task<List<StackPhaseSettingNew>> GetStackPhaseSettings(int id)
        //{
        //    return (new List<StackPhaseSettingNew>());
        //}
        private bool StackPhaseSettingExists(int id)
        {
            return _context.stkPhaseSetting.Any(e => e.id == id);
        }
    }
}
