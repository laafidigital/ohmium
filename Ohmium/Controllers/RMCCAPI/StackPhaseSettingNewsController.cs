using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Models.EFModels.ViewModels;
using Ohmium.Repository;

namespace Ohmium.Controllers.RMCCAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackPhaseSettingNewsController : ControllerBase
    {
        private readonly SensorContext _context;

        public StackPhaseSettingNewsController(SensorContext context)
        {
            _context = context;
        }

        // GET: api/StackPhaseSettingNews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StackPhaseSettingNew>>> GetstackPhaseSettingNew()
        {
            return await _context.stackPhaseSettingNew.ToListAsync();
        }

        // GET: api/StackPhaseSettingNews/5
        [HttpGet("{did}")]
        public async Task<ActionResult<StackPhaseSettingsNewViewModel>> GetStackPhaseSettingNew(string did)
        {
            List<string> stackidList = _context.stack.Where(e => e.deviceID == did).Select(e => e.stackMfgID).ToList();
            List<List<StackPhaseSettingNew>> spsllist = new List<List<StackPhaseSettingNew>>();
            List<List<RunStep>> listRunStepList = new List<List<RunStep>>();
            foreach(var stackid in stackidList)
            {
                List<StackPhaseSettingNew> stackPhaseSettingNew = _context.stackPhaseSettingNew.Where(e => e.stackID == stackid).Include(e => e.stk).ToList();
                if (stackPhaseSettingNew.Count > 0)
                {
                    spsllist.Add(stackPhaseSettingNew);

                    //try
                    //{
                    //    int stkprfid = _context.stkRunProfile.FirstOrDefault(e => e.stackID == stackid).id;
                    //    List<RunStep> runStepList = _context.stkStep.Where(e => e.stkRunProfileID == stkprfid).ToList();

                    //    listRunStepList.Add(runStepList);
                    //}
                    //catch { }
                }
            }
            StackPhaseSettingsNewViewModel ssnm = new StackPhaseSettingsNewViewModel();
            ssnm.spsn = spsllist;
            ssnm.runStepLL = listRunStepList;
            ssnm.rstg = _context.runStepTemplateGroup.ToList();

            if (ssnm == null)
            {
                return NotFound();
            }

            return ssnm;
        }

        // PUT: api/StackPhaseSettingNews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutStackPhaseSettingNew(int id, StackPhaseSettingNew stackPhaseSettingNew)
        //{
        //    if (id != stackPhaseSettingNew.id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(stackPhaseSettingNew).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StackPhaseSettingNewExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/StackPhaseSettingNews
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<StackPhaseSettingNew>> PostStackPhaseSettingNew(StackPhaseSettingNew stackPhaseSettingNew)
        //{
        //    _context.stackPhaseSettingNew.Add(stackPhaseSettingNew);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetStackPhaseSettingNew", new { id = stackPhaseSettingNew.id }, stackPhaseSettingNew);
        //}

        //// DELETE: api/StackPhaseSettingNews/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteStackPhaseSettingNew(int id)
        //{
        //    var stackPhaseSettingNew = await _context.stackPhaseSettingNew.FindAsync(id);
        //    if (stackPhaseSettingNew == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.stackPhaseSettingNew.Remove(stackPhaseSettingNew);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool StackPhaseSettingNewExists(int id)
        {
            return _context.stackPhaseSettingNew.Any(e => e.id == id);
        }
    }
}
