using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Repository;
using Microsoft.Extensions.Logging;
using Ohmium.Models.EFModels.ViewModels;

namespace Ohmium.Controllers.RMCCAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunProfilesController : ControllerBase
    {
        private readonly SensorContext _context;
        private readonly ILogger _logger;

        public RunProfilesController(SensorContext context, ILogger<RunProfilesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/RunProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RunProfile>>> GetrunProfile()
        {
            return await _context.runProfile.ToListAsync();
        }

        // GET: api/RunProfiles/5
        [HttpGet("{did}")]
        public ActionResult<RunProfile> GetRunProfile(string did)
        {
            var runProfile =  _context.runProfile.Include(x=>x.stackRunProfile).FirstOrDefault(x=>x.deviceID==did);
            foreach(StackRunProfile strp in runProfile.stackRunProfile)
            {
                List<RunStep> rs = _context.stkStep.Where(x=>x.stkRunProfileID==strp.id).ToList();
                strp.runStep = rs;
            }
            if (runProfile == null)
            {
                return NotFound();
            }
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            string rp = JsonSerializer.Serialize(runProfile, options);
            return Ok(rp);
        }

        [HttpGet("DeviceConsolidateAPI/{did}")]
        public ActionResult<RunProfile> DeviceConsolidateAPI(string did)
        {
            SingleAPIViewModel savm = new SingleAPIViewModel();
            List<List<StackPhaseSetting>> sps = new List<List<StackPhaseSetting>>();
            var runProfile = _context.runProfile.Include(x => x.stackRunProfile).FirstOrDefault(x => x.deviceID == did);
            foreach (StackRunProfile strp in runProfile.stackRunProfile)
            {
                List<RunStep> rs = _context.stkStep.Where(x => x.stkRunProfileID == strp.id).ToList();
                strp.runStep = rs;
                sps.Add(_context.stkPhaseSetting.Where(x => x.stackID == strp.stackID).ToList());
            }
            savm.stackPhaseSettings = sps;
            savm.sequenceName = _context.runStepTemplateGroup.ToList();
            if (runProfile == null)
            {
                return NotFound();
            }
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            savm.runProfile = runProfile;
            string rp = JsonSerializer.Serialize(savm, options);
            return Ok(rp);
        }

        // PUT: api/RunProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRunProfile(int id, RunProfile runProfile)
        {
            if (id != runProfile.id)
            {
                return BadRequest();
            }

            _context.Entry(runProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("DBUpdateConcurrencyException");
                if (!RunProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RunProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RunProfile>> PostRunProfile(RunProfile runProfile)
        {
            _context.runProfile.Add(runProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRunProfile", new { id = runProfile.id }, runProfile);
        }

        // DELETE: api/RunProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRunProfile(int id)
        {
            var runProfile = await _context.runProfile.FindAsync(id);
            if (runProfile == null)
            {
                return NotFound();
            }

            _context.runProfile.Remove(runProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       

        private bool RunProfileExists(int id)
        {
            return _context.runProfile.Any(e => e.id == id);
        }
    }
}
