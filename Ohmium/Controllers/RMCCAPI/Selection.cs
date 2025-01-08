using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
//using Microsoft.VisualStudio.Web.CodeGeneration.Contract.Messaging;
using Newtonsoft.Json;
using Ohmium.Models;
using Ohmium.Models.EFModels;
using Ohmium.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ohmium.Controllers.RMCCAPI
{
    [Route("api/readdata")]
    [ApiController]
    //[EnableCors]
    public class Selection : ControllerBase
    {
        private readonly SensorContext _context;
        private readonly ILogger _logger;
        private readonly CacheContext _cache;

        public Selection(SensorContext context, CacheContext cache,ILogger<Selection> logger)
        {
            _context = context;
            _logger = logger;
            _cache = cache;

        }
        //[HttpGet]
        //[Route("stackdatadownload")]
        //public IActionResult stackdatadownload(string stackid, DateTime sdate, DateTime edate)
        //{
        //    var resutls = _context.spsinglestack(stackid, sdate, edate);
        //    return Ok(resutls);

        //}

        // GET: api/Equipments

        [HttpGet,Route("dummy")]
        public IActionResult DummyData()
        {
            return Ok();
        }
        [HttpGet, Route("SequenceNames")]
        public IActionResult SequenceName()
        {
            var sn = _context.runStepTemplateGroup.Select(e => new
            {
                name = e.name
            });

            return Ok(sn);
        }
        [HttpPost]
        public IActionResult GetData([FromBody] Request request)
        {
            string sysMessage = "";
            //request.vr.sTime = request.vr.sTime.ToUniversalTime();
            //request.vr.eTime = request.vr.eTime.ToUniversalTime();
            try
            {
                switch (request.query)
                {
                    case "Org":
                        Guid orgId = request.id;
                        Org org = _context.org.Find(orgId);
                        if (org != null)
                            return Ok(org);
                        else
                            return Ok(_context.org.ToList());
                    case "Site":
                        Guid siteOrgId = request.id;
                        return Ok(_context.site.Where(x => x.orgID == siteOrgId));
                    case "Device":
                        Guid siteID = request.id;

                        List<Device> d = _context.device.Where(x => x.siteID == siteID).ToList();
                        return Ok(d);
                    case "Stack":
                        //return Ok(_context.stack.Where(x => x.deviceID == request.vr.deviceID && !x.stackPosition.Contains("unassigned")));
                        List<Ohmium.Models.EFModels.Stack> stacklist =(List<Ohmium.Models.EFModels.Stack>) _context.stack.FromSqlRaw("spStacksByPosition p0", request.vr.deviceID);
                        return Ok(stacklist);
                    case "DeviceSensor":
                        MTSDeviceDataNew sdlist = _context.mtsDeviceDataNew.Where(x => x.siteID == request.vr.siteID.ToString() && x.deviceID == request.vr.deviceID).OrderByDescending(x => x.timeStamp).FirstOrDefault();
                        if (sdlist == null)
                            sdlist = new MTSDeviceDataNew();
                        return Ok(sdlist);
                    case "DeviceSensorHistoric":
                        List<MTSDeviceDataNew> ddlist = _context.mtsDeviceDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).ToList();
                        if (ddlist == null)
                            return BadRequest();
                        return Ok(ddlist);
                    //case "LotusSensor":
                    //    List<LHCC01> lhcdt = _context.LHCC01Data.Where(x => x.lhc1SN  == request.vr.deviceID).OrderByDescending(x => x.timeStamp).FirstOrDefault();
                    //    return Ok(LotusDevicelist);
                    case "StackSensor":
                        List<MTSStackDataNew> stklist = new List<MTSStackDataNew>();
                        //List<string> stmfgid = _context.stack.Where(x => x.deviceID == request.vr.deviceID && !x.stackPosition.Contains("unassigned")).Select(x => x.stackMfgID).Distinct().ToList();
                        List<string> stmfgid = _context.stack.Where(x => x.deviceID == request.vr.deviceID && x.status == 1).Select(x => x.stackMfgID).Distinct().ToList();
                        foreach (string sp in stmfgid)
                        {
                            try
                            {
                                MTSStackDataNew stk = _context.mtsStackDataNew.Where(x => x.stackMfgID == sp && x.timeStamp >= DateTime.UtcNow.AddSeconds(-20)).OrderByDescending(x => x.timeStamp).FirstOrDefault();
                                stk.position = _context.stack.Single(x => x.stackMfgID == sp).stackPosition;
                                stklist.Add(stk);
                            }
                            catch (Exception ex)
                            {

                                _logger.LogError(ex.Message);
                            }
                        }
                        return Ok(stklist);
                
                    //historic data on stack
                    case "StackSensorHistoric":
                        var stkDataH = new Object();
                        //List<string> stmfgid = _context.stack.Where(x => x.deviceID == request.vr.deviceID && !x.stackPosition.Contains("unassigned")).Select(x => x.stackMfgID).Distinct().ToList();
                        switch (request.vr.sensorName)
                        {
                            case "psV":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID  && x.stackMfgID==request.vr.stkMfgID&& x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.psV
                                    });

                                    break;
                                }
                            case "wF":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.wF
                                    });

                                    break;
                                }
                            case "wT":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.wT
                                    });

                                    break;
                                }
                            case "wP":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.wP
                                    });

                                    break;
                                }
                            case "hT":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.hT
                                    });
                                    break;
                                }
                            case "hP":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.hP
                                    });

                                    break;
                                }
                            case "psI":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.psI
                                    });

                                    break;
                                }
                            case "cV1":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV1
                                    });

                                    break;
                                }
                            case "cV2":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV2
                                    });

                                    break;
                                }
                            case "cV3":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV3
                                    });

                                    break;
                                }
                            case "cV4":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV4
                                    });

                                    break;
                                }
                            case "cV5":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV5
                                    });
                                    break;
                                }
                            case "cR1":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR1
                                    });

                                    break;
                                }
                            case "cR2":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR2
                                    });

                                    break;
                                }
                            case "cR3":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR3
                                    });

                                    break;
                                }
                            case "cR4":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR4
                                    });

                                    break;
                                }
                            case "cR5":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR5
                                    });

                                    break;
                                }
                            case "status":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.status
                                    });

                                    break;
                                }
                            case "scriptLoopCnt":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.scriptLoopCnt,
                                        x.status
                                    });
                                    break;
                                }
                            case "seqLoopCnt":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.seqLoopCnt,
                                        x.status
                                    });
                                    break;
                                }
                            case "scriptStep":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.scriptStep,
                                        x.status
                                    });
                                    break;
                                }

                            case "seqStep":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.seqStep,
                                        x.status
                                    });
                                    break;
                                }
                        }
                        return Ok(stkDataH);


                    //TSStackData stklist = new TSStackData();
                    //try
                    //{
                    //    stklist = _context.mtsStackData.Where(x => x.stackMfgID == request.vr.stkMfgID && x.deviceID == request.vr.deviceID).OrderByDescending(x => x.timeStamp).FirstOrDefault();
                    //}
                    //catch { }
                    //return Ok(stklist);
                    case "DeviceSensorByTimerange":
                        List<MTSDeviceDataNew> sdlist2H = _context.mtsDeviceDataNew.Where(x => x.siteID == request.vr.siteID.ToString() && x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime).ToList();
                        return Ok(sdlist2H);
                    case "StackSensorByTimerange":
                        //List<List<TSStackData>> stacklist = new List<List<TSStackData>>();
                        //List<string> stackmfgid = _context.stack.Where(x => x.deviceID == request.vr.deviceID).Select(x => x.stackMfgID).Distinct().ToList();
                        //foreach (string sp in stackmfgid)
                        //{
                        //                        List<TSStackData> stkList = _context.mtsStackData.Where(x => x.deviceID==request.vr.deviceID && x.timeStamp>=request.vr.sTime && x.timeStamp<=request.vr.eTime).ToList();
                        //  stacklist.Add(stkList);
                        //}
                        //return Ok(stkList);

                        var stkData = new Object();
                        switch (request.vr.sensorName)
                        {
                            case "psV":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.psV
                                    });

                                    break;
                                }
                            case "wF":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.wF
                                    });

                                    break;
                                }
                            case "wT":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.wT
                                    });

                                    break;
                                }
                            case "wP":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.wP
                                    });

                                    break;
                                }
                            case "hT":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.hT
                                    });
                                    break;
                                }
                            case "hP":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.hP
                                    });

                                    break;
                                }
                            case "psI":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.psI
                                    });

                                    break;
                                }
                            case "cV1":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV1
                                    });

                                    break;
                                }
                            case "cV2":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV2
                                    });

                                    break;
                                }
                            case "cV3":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV3
                                    });

                                    break;
                                }
                            case "cV4":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV4
                                    });

                                    break;
                                }
                            case "cV5":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cV5
                                    });
                                    break;
                                }
                            case "cR1":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR1
                                    });

                                    break;
                                }
                            case "cR2":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR2
                                    });

                                    break;
                                }
                            case "cR3":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR3
                                    });

                                    break;
                                }
                            case "cR4":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR4
                                    });

                                    break;
                                }
                            case "cR5":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.cR5
                                    });

                                    break;
                                }
                            case "status":
                                {
                                    stkData = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.timeStamp >= request.vr.sTime && x.timeStamp <= request.vr.eTime && x.stackMfgID == request.vr.stkMfgID).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.timeStamp,
                                        x.status
                                    });

                                    break;
                                }
                            case "scriptLoopCnt":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.scriptLoopCnt,
                                        x.status
                                    });
                                    break;
                                }
                            case "seqLoopCnt":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.seqLoopCnt,
                                        x.status
                                    });
                                    break;
                                }
                            case "scriptStep":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.scriptStep,
                                        x.status
                                    });
                                    break;
                                }

                            case "seqStep":
                                {
                                    stkDataH = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == request.vr.stkMfgID && x.timeStamp >= request.vr.eTime.AddMinutes(-15) && x.timeStamp <= request.vr.eTime).Select(x => new
                                    {
                                        x.deviceID,
                                        x.stackMfgID,
                                        x.seqStep,
                                        x.status
                                    });
                                    break;
                                }

                        }
                        return Ok(stkData);
                    case "StackSensorByID":
                        //List<List<TSStackData>> stacklist = new List<List<TSStackData>>();
                        //List<string> stackmfgid = _context.stack.Where(x => x.deviceID == request.vr.deviceID).Select(x => x.stackMfgID).Distinct().ToList();
                        //foreach (string sp in stackmfgid)
                        //{
                        //                        List<TSStackData> stkList = _context.mtsStackDataNew.Where(x => x.deviceID==request.vr.deviceID && x.timeStamp>=request.vr.sTime && x.timeStamp<=request.vr.eTime).ToList();
                        //  stacklist.Add(stkList);
                        //}
                        //                  return Ok(stkList);

                        var stkDataSensorSpec = new Object();
                        List<Object> stkdss = new List<object>();
                        List<MTSStackDataNew> stklistSpec = new List<MTSStackDataNew>();
                        List<string> stmfgidSpec = _context.stack.Where(x => x.deviceID == request.vr.deviceID && x.status==1).Select(x => x.stackMfgID).Distinct().ToList();
                        foreach (string sp in stmfgidSpec)
                        {
                            MTSStackDataNew stk = _context.mtsStackDataNew.Where(x => x.deviceID == request.vr.deviceID && x.stackMfgID == sp).OrderByDescending(x => x.timeStamp).FirstOrDefault();
                            if (stk != null)
                            {
                                string position = _context.stack.Single(x => x.deviceID == request.vr.deviceID && x.stackMfgID == sp).stackPosition;
                                stk.position = position;


                                try
                                {
                                    switch (request.vr.sensorName)
                                    {
                                        case "psV":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.psV,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "wF":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.wF,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "wT":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.wT,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "wP":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.wP,
                                                    stk.position
                                                };

                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "hT":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.hT,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "hP":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.hP,
                                                    stk.position
                                                };

                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "psI":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.psI,
                                                    stk.position
                                                };

                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cV1":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cV1,
                                                    stk.position
                                                };

                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cV2":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cV2,
                                                    stk.position
                                                };

                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cV3":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cV3,
                                                    stk.position
                                                };

                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cV4":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cV4,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cV5":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cV5,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cV6":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cV6,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cR1":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cR1,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cR2":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cR2,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cR3":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cR3,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cR4":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cR4,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cR5":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cR5,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }
                                        case "cR6":
                                            {
                                                stkDataSensorSpec = new
                                                {
                                                    stk.stackMfgID,
                                                    stk.timeStamp,
                                                    stk.cR6,
                                                    stk.position
                                                };
                                                stkdss.Add(stkDataSensorSpec);
                                                break;
                                            }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex.Message);
                                }
                            }
                        }
                        return Ok(stkdss);

                    case "DeviceConfig":
                        Guid configID = _context.device.Single(x => x.EqMfgID == request.vr.deviceID).configID;
                        string Config = _context.equipmentConfiguration.Single(x => x.equipmentConfigID == configID).equipmentConfiguration;
                        return Ok(Config);
                    case "StackConfig":
                        List<Ohmium.Models.EFModels.Stack> stackList = _context.stack.Where(x => x.deviceID == request.vr.deviceID).ToList();
                        List<StackConfigViewModel> config = new List<StackConfigViewModel>();
                        foreach (Ohmium.Models.EFModels.Stack s in stackList)
                        {
                            StackConfigViewModel scfv = new StackConfigViewModel();
                            scfv.config = _context.sconfig.Single(x => x.configID == s.stackConfig).configString;
                            scfv.stackID = s.stackMfgID;
                            config.Add(scfv);
                        }
                        //Guid stackConfigID = _context.stack.Single(x => x.deviceID == request.vr.deviceID && x.stackMfgID==request.vr.stkMfgID).stackConfig;
                        //StackConfig sc = _context.sconfig.Find(stackConfigID);

                        return Ok(config);
                    case "DateRangeSensor":
                        StackRepo sr = new StackRepo();
                        string stkdata = string.Join(',', request.vr.selectedStack);
                        DateTime stime = DateTime.Parse(request.vr.sTime.ToString(), null, DateTimeStyles.RoundtripKind);
                        DateTime etime = DateTime.Parse(request.vr.eTime.ToString(), null, DateTimeStyles.RoundtripKind);
                        DataTable dt = sr.GetDeviceStackData(stime, etime, request.vr.sequenceName, request.vr.deviceID, stkdata, request.vr.sec);
                        List<MTSStackData> stkDataList = new List<MTSStackData>();

                        List<MTSDeviceData> deviceDataList = new List<MTSDeviceData>();

                        ResponseData rd = new ResponseData();

                        //List<DeviceAndStackDto> deviceAndStackList = new List<DeviceAndStackDto>();
                        //int count = 0;
                        //foreach (DataRow sd in dt.Rows)
                        //{

                        //    DeviceAndStackDto das = new DeviceAndStackDto();

                        //    try
                        //    {

                        //        das.timeStamp = Convert.ToDateTime(sd["timeStamp"].ToString());
                        //        das.wF = sd["wF"].ToString() == string.Empty ? 0 : float.Parse(sd["wF"].ToString());
                        //        das.wL = sd["wL"].ToString() == string.Empty ? 0 : float.Parse(sd["wL"].ToString());
                        //        das.wT = sd["wT"].ToString() == string.Empty ? 0 : float.Parse(sd["wT"].ToString());
                        //        das.wP = sd["wP"].ToString() == string.Empty ? 0 : float.Parse(sd["wP"].ToString());
                        //        das.wC = sd["wC"].ToString() == string.Empty ? 0 : float.Parse(sd["wC"].ToString());
                        //        das.HYS = sd["HYS"].ToString() == string.Empty ? 0 : float.Parse(sd["HYS"].ToString());

                        //        das.wPp = sd["wPp"].ToString() == string.Empty ? 0 : float.Parse(sd["wPp"].ToString());

                        //        das.wT_d = sd["wT_d"].ToString() == string.Empty ? 0 : float.Parse(sd["wT_d"].ToString());
                        //        das.wP_d = sd["wP_d"].ToString() == string.Empty ? 0 : float.Parse(sd["wP_d"].ToString());
                        //        das.hT = sd["hT"].ToString() == string.Empty ? 0 : float.Parse(sd["hT"].ToString());
                        //        das.hP = sd["hP"].ToString() == string.Empty ? 0 : float.Parse(sd["hP"].ToString());

                        //        das.cV1 = sd["cV1"].ToString() == string.Empty ? 0 : float.Parse(sd["cV1"].ToString());
                        //        das.cV2 = sd["cV2"].ToString() == string.Empty ? 0 : float.Parse(sd["cV2"].ToString());
                        //        das.cV3 = sd["cV3"].ToString() == string.Empty ? 0 : float.Parse(sd["cV3"].ToString());
                        //        das.cV4 = sd["cV4"].ToString() == string.Empty ? 0 : float.Parse(sd["cV4"].ToString());
                        //        das.cV5 = sd["cV5"].ToString() == string.Empty ? 0 : float.Parse(sd["cV5"].ToString());
                        //        das.cV6 = sd["cV6"].ToString() == string.Empty ? 0 : float.Parse(sd["cV6"].ToString());
                        //        das.cX1 = sd["cX1"].ToString() == string.Empty ? 0 : float.Parse(sd["cX1"].ToString());
                        //        das.cX2 = sd["cX2"].ToString() == string.Empty ? 0 : float.Parse(sd["cX2"].ToString());
                        //        das.cX3 = sd["cX3"].ToString() == string.Empty ? 0 : float.Parse(sd["cX3"].ToString());
                        //        das.cX4 = sd["cX4"].ToString() == string.Empty ? 0 : float.Parse(sd["cX4"].ToString());
                        //        das.cX5 = sd["cX5"].ToString() == string.Empty ? 0 : float.Parse(sd["cX5"].ToString());
                        //        das.deviceID = sd["deviceID"].ToString() == string.Empty ? "" : sd["deviceID"].ToString();
                        //        das.hxiT = sd["hxiT"].ToString() == string.Empty ? 0 : float.Parse(sd["hxiT"].ToString());
                        //        das.hxoT = sd["hxoT"].ToString() == string.Empty ? 0 : float.Parse(sd["hxoT"].ToString());
                        //        das.imA = sd["imA"].ToString() == string.Empty ? 0 : float.Parse(sd["imA"].ToString());
                        //        das.imF = sd["imF"].ToString() == string.Empty ? 0 : float.Parse(sd["imF"].ToString());
                        //        das.isF = sd["isF"].ToString() == string.Empty ? 0 : float.Parse(sd["isF"].ToString());
                        //        das.cM1 = sd["cM1"].ToString() == string.Empty ? 0 : float.Parse(sd["cM1"].ToString());
                        //        das.cM2 = sd["cM2"].ToString() == string.Empty ? 0 : float.Parse(sd["cM2"].ToString());
                        //        das.cM3 = sd["cM3"].ToString() == string.Empty ? 0 : float.Parse(sd["cM3"].ToString());
                        //        das.cM4 = sd["cM4"].ToString() == string.Empty ? 0 : float.Parse(sd["cM4"].ToString());
                        //        das.cM5 = sd["cM5"].ToString() == string.Empty ? 0 : float.Parse(sd["cM5"].ToString());
                        //        das.CommStatus = short.Parse(sd["CommStatus"].ToString());
                        //        das.ver = sd["ver"].ToString() == string.Empty ? "" : sd["ver"].ToString();
                        //        das.configID = (Guid)sd["configID"];
                        //        das.cR1 = sd["cR1"].ToString() == string.Empty ? 0 : float.Parse(sd["cR1"].ToString());
                        //        das.cR2 = sd["cR2"].ToString() == string.Empty ? 0 : float.Parse(sd["cR2"].ToString());
                        //        das.cR3 = sd["cR3"].ToString() == string.Empty ? 0 : float.Parse(sd["cR3"].ToString());
                        //        das.cR4 = sd["cR4"].ToString() == string.Empty ? 0 : float.Parse(sd["cR4"].ToString());
                        //        das.cR5 = sd["cR5"].ToString() == string.Empty ? 0 : float.Parse(sd["cR5"].ToString());
                        //        das.loopcnt = sd["loopcnt"].ToString() == string.Empty ? 0 : int.Parse(sd["loopcnt"].ToString());
                        //        das.position = sd["position"].ToString() == string.Empty ? "" : sd["position"].ToString();
                        //        das.psI = sd["psI"].ToString() == string.Empty ? 0 : float.Parse(sd["psI"].ToString());
                        //        das.psV = sd["psV"].ToString() == string.Empty ? 0 : float.Parse(sd["psV"].ToString());
                        //        das.runHours = sd["runHours"].ToString() == string.Empty ? 0 : float.Parse(sd["runHours"].ToString());
                        //        das.cumulativeHours = sd["cumulativeHours"].ToString() == string.Empty ? float.Parse(sd["runHours"].ToString()) : float.Parse(sd["cumulativeHours"].ToString());
                        //        das.seqName = sd["seqName"].ToString() == string.Empty ? "" : sd["seqName"].ToString();
                        //        das.siteID = sd["siteID"].ToString() == string.Empty ? "" : sd["siteID"].ToString();
                        //        das.stackMfgID = sd["stackMfgID"].ToString() == string.Empty ? "" : sd["stackMfgID"].ToString();
                        //        das.state = sd["state"].ToString() == string.Empty ? "" : sd["state"].ToString();
                        //        das.stepNumber = sd["stepNumber"].ToString() == string.Empty ? 0 : int.Parse(sd["stepNumber"].ToString());
                        //        das.verM = sd["verM"].ToString() == string.Empty ? "" : sd["verM"].ToString();
                        //        das.status = int.Parse(sd["status"].ToString()) == 0 ? 0 : int.Parse((sd["status"].ToString()));
                        //        das.scriptLoopCnt = sd["scriptLoopCnt"].ToString() == string.Empty ? 0 : float.Parse(sd["scriptLoopCnt"].ToString());
                        //        das.seqLoopCnt = sd["seqLoopCnt"].ToString() == string.Empty ? 0 : float.Parse(sd["seqLoopCnt"].ToString());
                        //        das.scriptStep = sd["scriptStep"].ToString() == string.Empty ? 0 : float.Parse(sd["scriptStep"].ToString());
                        //        das.seqStep = sd["seqStep"].ToString() == string.Empty ? 0 : float.Parse(sd["seqStep"].ToString());
                        //        deviceAndStackList.Add(das);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        //return Ok(ex.Message);
                        //    }
                        //}
                        rd.stackData = stkDataList;
                        rd.deviceData = deviceDataList;
                        //rd.deviceAndStack = deviceAndStackList.OrderBy(e => e.timeStamp).ToList();
                        var t = rd.deviceData;
                        var json = JsonConvert.SerializeObject(dt);
                        rd.deviceAndStack = JsonConvert.DeserializeObject<List<DeviceAndStackDto>>(json);
                        return Ok(rd);
                    //stackdownload across devices
                    //case "DateRangeSensorNew":
                    //    StackRepo srnew = new StackRepo();
                        
                    //    DataTable dtnew = srnew.GetDeviceStackDataNew(request.vr.sTime, request.vr.eTime, request.vr.sequenceName, request.vr.stkMfgID,request.vr.sec);
                    //    List<mtsStackDataNew> stkDataListNew = new List<mtsStackDataNew>();
                    //    List<mtsDeviceDataNew> deviceDataListNew = new List<mtsDeviceDataNew>();

                    //    ResponseData rdnew = new ResponseData();

                    //    List<DeviceAndStackDto> deviceAndStackListNew = new List<DeviceAndStackDto>();
                    //    int countNew = 0;
                    //    foreach (DataRow sd in dtnew.Rows)
                    //    {

                    //        DeviceAndStackDto das = new DeviceAndStackDto();

                    //        try
                    //        {

                    //            das.timeStamp = Convert.ToDateTime(sd["timeStamp"].ToString());
                    //            das.wF = sd["wF"].ToString() == string.Empty ? 0 : float.Parse(sd["wF"].ToString());
                    //            das.wL = sd["wL"].ToString() == string.Empty ? 0 : float.Parse(sd["wL"].ToString());
                    //            das.wT = sd["wT"].ToString() == string.Empty ? 0 : float.Parse(sd["wT"].ToString());
                    //            das.wP = sd["wP"].ToString() == string.Empty ? 0 : float.Parse(sd["wP"].ToString());
                    //            das.wC = sd["wC"].ToString() == string.Empty ? 0 : float.Parse(sd["wC"].ToString());
                    //            das.HYS = sd["HYS"].ToString() == string.Empty ? 0 : float.Parse(sd["HYS"].ToString());

                    //            das.wPp = sd["wPp"].ToString() == string.Empty ? 0 : float.Parse(sd["wPp"].ToString());

                    //            das.wT_d = sd["wT_d"].ToString() == string.Empty ? 0 : float.Parse(sd["wT_d"].ToString());
                    //            das.wP_d = sd["wP_d"].ToString() == string.Empty ? 0 : float.Parse(sd["wP_d"].ToString());
                    //            das.hT = sd["hT"].ToString() == string.Empty ? 0 : float.Parse(sd["hT"].ToString());
                    //            das.hP = sd["hP"].ToString() == string.Empty ? 0 : float.Parse(sd["hP"].ToString());

                    //            das.cV1 = sd["cV1"].ToString() == string.Empty ? 0 : float.Parse(sd["cV1"].ToString());
                    //            das.cV2 = sd["cV2"].ToString() == string.Empty ? 0 : float.Parse(sd["cV2"].ToString());
                    //            das.cV3 = sd["cV3"].ToString() == string.Empty ? 0 : float.Parse(sd["cV3"].ToString());
                    //            das.cV4 = sd["cV4"].ToString() == string.Empty ? 0 : float.Parse(sd["cV4"].ToString());
                    //            das.cV5 = sd["cV5"].ToString() == string.Empty ? 0 : float.Parse(sd["cV5"].ToString());
                    //            das.cV6 = sd["cV6"].ToString() == string.Empty ? 0 : float.Parse(sd["cV6"].ToString());
                    //            das.cX1 = sd["cX1"].ToString() == string.Empty ? 0 : float.Parse(sd["cX1"].ToString());
                    //            das.cX2 = sd["cX2"].ToString() == string.Empty ? 0 : float.Parse(sd["cX2"].ToString());
                    //            das.cX3 = sd["cX3"].ToString() == string.Empty ? 0 : float.Parse(sd["cX3"].ToString());
                    //            das.cX4 = sd["cX4"].ToString() == string.Empty ? 0 : float.Parse(sd["cX4"].ToString());
                    //            das.cX5 = sd["cX5"].ToString() == string.Empty ? 0 : float.Parse(sd["cX5"].ToString());
                    //            das.deviceID = sd["deviceID"].ToString() == string.Empty ? "" : sd["deviceID"].ToString();
                    //            das.hxiT = sd["hxiT"].ToString() == string.Empty ? 0 : float.Parse(sd["hxiT"].ToString());
                    //            das.hxoT = sd["hxoT"].ToString() == string.Empty ? 0 : float.Parse(sd["hxoT"].ToString());
                    //            das.imA = sd["imA"].ToString() == string.Empty ? 0 : float.Parse(sd["imA"].ToString());
                    //            das.imF = sd["imF"].ToString() == string.Empty ? 0 : float.Parse(sd["imF"].ToString());
                    //            das.isF = sd["isF"].ToString() == string.Empty ? 0 : float.Parse(sd["isF"].ToString());
                    //            das.cM1 = sd["cM1"].ToString() == string.Empty ? 0 : float.Parse(sd["cM1"].ToString());
                    //            das.cM2 = sd["cM2"].ToString() == string.Empty ? 0 : float.Parse(sd["cM2"].ToString());
                    //            das.cM3 = sd["cM3"].ToString() == string.Empty ? 0 : float.Parse(sd["cM3"].ToString());
                    //            das.cM4 = sd["cM4"].ToString() == string.Empty ? 0 : float.Parse(sd["cM4"].ToString());
                    //            das.cM5 = sd["cM5"].ToString() == string.Empty ? 0 : float.Parse(sd["cM5"].ToString());
                    //            das.CommStatus = short.Parse(sd["CommStatus"].ToString());
                    //            das.ver = sd["ver"].ToString() == string.Empty ? "" : sd["ver"].ToString();
                    //            das.configID = (Guid)sd["configID"];
                    //            das.cR1 = sd["cR1"].ToString() == string.Empty ? 0 : float.Parse(sd["cR1"].ToString());
                    //            das.cR2 = sd["cR2"].ToString() == string.Empty ? 0 : float.Parse(sd["cR2"].ToString());
                    //            das.cR3 = sd["cR3"].ToString() == string.Empty ? 0 : float.Parse(sd["cR3"].ToString());
                    //            das.cR4 = sd["cR4"].ToString() == string.Empty ? 0 : float.Parse(sd["cR4"].ToString());
                    //            das.cR5 = sd["cR5"].ToString() == string.Empty ? 0 : float.Parse(sd["cR5"].ToString());
                    //            das.loopcnt = sd["loopcnt"].ToString() == string.Empty ? 0 : int.Parse(sd["loopcnt"].ToString());
                    //            das.position = sd["position"].ToString() == string.Empty ? "" : sd["position"].ToString();
                    //            das.psI = sd["psI"].ToString() == string.Empty ? 0 : float.Parse(sd["psI"].ToString());
                    //            das.psV = sd["psV"].ToString() == string.Empty ? 0 : float.Parse(sd["psV"].ToString());
                    //            das.runHours = sd["runHours"].ToString() == string.Empty ? 0 : float.Parse(sd["runHours"].ToString());
                    //            das.cumulativeHours = sd["cumulativeHours"].ToString() == string.Empty ? float.Parse(sd["runHours"].ToString()) : float.Parse(sd["cumulativeHours"].ToString());
                    //            das.seqName = sd["seqName"].ToString() == string.Empty ? "" : sd["seqName"].ToString();
                    //            das.siteID = sd["siteID"].ToString() == string.Empty ? "" : sd["siteID"].ToString();
                    //            das.stackMfgID = sd["stackMfgID"].ToString() == string.Empty ? "" : sd["stackMfgID"].ToString();
                    //            das.state = sd["state"].ToString() == string.Empty ? "" : sd["state"].ToString();
                    //            das.stepNumber = sd["stepNumber"].ToString() == string.Empty ? 0 : int.Parse(sd["stepNumber"].ToString());
                    //            das.verM = sd["verM"].ToString() == string.Empty ? "" : sd["verM"].ToString();
                    //            das.status = int.Parse(sd["status"].ToString()) == 0 ? 0 : int.Parse((sd["status"].ToString()));
                    //            das.scriptLoopCnt = sd["scriptLoopCnt"].ToString() == string.Empty ? 0 : float.Parse(sd["scriptLoopCnt"].ToString());
                    //            das.seqLoopCnt = sd["seqLoopCnt"].ToString() == string.Empty ? 0 : float.Parse(sd["seqLoopCnt"].ToString());
                    //            das.scriptStep = sd["scriptStep"].ToString() == string.Empty ? 0 : float.Parse(sd["scriptStep"].ToString());
                    //            das.seqStep = sd["seqStep"].ToString() == string.Empty ? 0 : float.Parse(sd["seqStep"].ToString());
                    //            deviceAndStackListNew.Add(das);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            //return Ok(ex.Message);
                    //        }
                    //    }
                    //    rdnew.stackData = stkDataListNew;
                    //    rdnew.deviceData = deviceDataListNew;
                    //    rdnew.deviceAndStack = deviceAndStackListNew.OrderBy(e => e.timeStamp).ToList();
                    //    return Ok(rdnew);

                    case "stackanddevicelive":
                        ResponseData rdLive = new ResponseData();

                        //List<DateTime> timeList = rd.deviceData.Select(e => e.timeStamp).ToList();
                        List<DeviceAndStackDto> rdList = new List<DeviceAndStackDto>();
                        List<Ohmium.Models.EFModels.Stack> slist = _context.stack.Where(e => e.siteID.ToString() == request.vr.siteID && e.status==1).ToList();
                        List<MTSStackDataNew> mtssl = new List<MTSStackDataNew>();
                        foreach (Ohmium.Models.EFModels.Stack s in slist)
                        {
                            MTSStackDataNew mtss = _context.mtsStackDataNew.Where(e => e.stackMfgID == s.stackMfgID && e.deviceID == s.deviceID).OrderByDescending(e => e.timeStamp).FirstOrDefault();
                            if(mtss!=null)
                            mtssl.Add(mtss);
                        }
                        List<Ohmium.Models.EFModels.Device> dlist = _context.device.Where(e => e.siteID.ToString() == request.vr.siteID.ToString() && e.status==1).ToList();
                        List<MTSDeviceDataNew> mtsdevice = new List<MTSDeviceDataNew>();
                        foreach (Ohmium.Models.EFModels.Device s in dlist)
                        {
                            MTSDeviceDataNew mtdd = _context.mtsDeviceDataNew.Where(e => e.deviceID == s.EqMfgID).OrderByDescending(e => e.timeStamp).FirstOrDefault();
                            if(mtdd !=null)
                            mtsdevice.Add(mtdd);
                        }
                        foreach (MTSStackDataNew sd in mtssl)
                        {
                            DeviceAndStackDto das = new DeviceAndStackDto();
                            string deviceid = sd.deviceID;
                            try
                            {
                                das.timeStamp = sd.timeStamp;
                                das.wF = sd.wF ?? 0;
                                das.wT = sd.wT ?? 0;
                                das.wP = sd.wP ?? 0;

                                das.HYS = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().HYS ?? 0;

                                das.wPp = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().wPp ?? 0;

                                das.wT_d = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().wT ?? 0;
                                das.wP_d = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().wP ?? 0;
                                das.hT = sd.hT ?? 0;
                                das.hP = sd.hP ?? 0;

                                das.cV1 = sd.cV1 ?? 0;
                                das.cV2 = sd.cV2 ?? 0;
                                das.cV3 = sd.cV3 ?? 0;
                                das.cV4 = sd.cV4 ?? 0;
                                das.cV5 = sd.cV5 ?? 0;
                                das.cV6 = sd.cV6 ?? 0;
                                das.cX1 = sd.cX1 ?? 0;
                                das.cX2 = sd.cX2 ?? 0;
                                das.cX3 = sd.cX3 ?? 0;
                                das.cX4 = sd.cX4 ?? 0;
                                das.cX5 = sd.cX5 ?? 0;
                                das.deviceID = sd.deviceID;
                                das.hxiT = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().hxiT ?? 0;
                                das.hxoT = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().hxoT ?? 0;
                                das.imA = sd.imA ?? 0;
                                das.imF = sd.imF ?? 0;
                                das.isF = sd.isF ?? 0;
                                das.cM1 = sd.cM1 ?? 0;
                                das.cM2 = sd.cM2 ?? 0;
                                das.cM3 = sd.cM3 ?? 0;
                                das.cM4 = sd.cM4 ?? 0;
                                das.cM5 = sd.cM5 ?? 0;
                                das.CommStatus = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().CommStatus ?? 0;
                                das.ver = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().ver == string.Empty ? "" : mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().ver;
                                das.configID = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().configID;
                                das.cR1 = sd.cR1 ?? 0;
                                das.cR2 = sd.cR2 ?? 0;
                                das.cR3 = sd.cR3 ?? 0;
                                das.cR4 = sd.cR4 ?? 0;
                                das.cR5 = sd.cR5 ?? 0;
                                das.loopcnt = sd.loopcnt ?? 0;
                                das.position = sd.position.ToString() == string.Empty ? "" : sd.position;
                                das.psI = sd.psI ?? 0;
                                das.psV = sd.psV ?? 0;
                                double? sdhr = Math.Round(Convert.ToDouble(sd.runHours), 2);
                                das.runHours =(float?)sdhr ?? 0.0F;
                                das.seqName = sd.seqName;
                                das.siteID = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().siteID;
                                das.stackMfgID = sd.stackMfgID;
                                das.state = sd.state;
                                das.stepNumber = sd.stepNumber ?? 0;
                                das.verM = mtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().verM;
                                das.status = sd.status ?? 0;
                                das.interLock = sd.interLock;
                                das.scriptLoopCnt = sd.scriptLoopCnt;
                                das.seqLoopCnt = sd.seqLoopCnt;
                                das.scriptStep = sd.scriptStep;
                                das.seqStep = sd.seqStep;

                                double? sdcm = Math.Round(Convert.ToDouble(sd.cumulativeHours), 2);
                                das.cumulativeHours = (float?)sdcm ?? 0.0F;

                                //das.cumulativeHours = sd.cumulativeHours==0?das.runHours:sd.cumulativeHours;
                                //das.stkStatus = int.Parse(sd["status1"].ToString()) == 0 ? 0 : int.Parse(sd["status1"].ToString());
                                rdList.Add(das);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        rdLive.deviceAndStack = rdList;
                        return Ok(rdLive);
                    case "stackanddeviceliveSqLite":
                        ResponseData sqrdLive = new ResponseData();

                        //List<DateTime> timeList = rd.deviceData.Select(e => e.timeStamp).ToList();
                        List<DeviceAndStackDto> sqrdList = new List<DeviceAndStackDto>();
                        List<Ohmium.Models.EFModels.Stack> sqslist = _context.stack.Where(e => e.siteID.ToString() == request.vr.siteID && e.status == 1).ToList();
                        List<MTSStackData> sqmtssl = new List<MTSStackData>();
                        foreach (Ohmium.Models.EFModels.Stack s in sqslist)
                        {
                            MTSStackData sqmtss = _context.mtsStackData.Where(e => e.stackMfgID == s.stackMfgID && e.deviceID == s.deviceID).OrderByDescending(e => e.timeStamp).FirstOrDefault();
                            if (sqmtss != null)
                                sqmtssl.Add(sqmtss);
                        }
                        List<Ohmium.Models.EFModels.Device> sqdlist = _context.device.Where(e => e.siteID.ToString() == request.vr.siteID.ToString() && e.status == 1).ToList();
                        List<MTSDeviceData> sqmtsdevice = new List<MTSDeviceData>();
                        foreach (Ohmium.Models.EFModels.Device s in sqdlist)
                        {
                            MTSDeviceData sqmtdd = _context.mtsDeviceData.Where(e => e.deviceID == s.EqMfgID).OrderByDescending(e => e.timeStamp).FirstOrDefault();
                            if (sqmtdd != null)
                                sqmtsdevice.Add(sqmtdd);
                        }
                        foreach (MTSStackData sd in sqmtssl)
                        {
                            DeviceAndStackDto das = new DeviceAndStackDto();
                            string deviceid = sd.deviceID;
                            try
                            {
                                das.timeStamp = sd.timeStamp;
                                das.wF = sd.wF ?? 0;
                                das.wT = sd.wT ?? 0;
                                das.wP = sd.wP ?? 0;
                                das.HYS = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().HYS ?? 0;

                                das.wPp = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().wPp ?? 0;

                                das.wT_d = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().wT ?? 0;
                                das.wP_d = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().wP ?? 0;
                                das.hT = sd.hT ?? 0;
                                das.hP = sd.hP ?? 0;

                                das.cV1 = sd.cV1 ?? 0;
                                das.cV2 = sd.cV2 ?? 0;
                                das.cV3 = sd.cV3 ?? 0;
                                das.cV4 = sd.cV4 ?? 0;
                                das.cV5 = sd.cV5 ?? 0;
                                das.cV6 = sd.cV6 ?? 0;
                                das.cX1 = sd.cX1 ?? 0;
                                das.cX2 = sd.cX2 ?? 0;
                                das.cX3 = sd.cX3 ?? 0;
                                das.cX4 = sd.cX4 ?? 0;
                                das.cX5 = sd.cX5 ?? 0;
                                das.deviceID = sd.deviceID;
                                das.hxiT = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().hxiT ?? 0;
                                das.hxoT = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().hxoT ?? 0;
                                das.imA = sd.imA ?? 0;
                                das.imF = sd.imF ?? 0;
                                das.isF = sd.isF ?? 0;
                                das.cM1 = sd.cM1 ?? 0;
                                das.cM2 = sd.cM2 ?? 0;
                                das.cM3 = sd.cM3 ?? 0;
                                das.cM4 = sd.cM4 ?? 0;
                                das.cM5 = sd.cM5 ?? 0;
                                das.CommStatus = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().CommStatus ?? 0;
                                das.ver = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().ver == string.Empty ? "" : sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().ver;
                                das.configID = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().configID;
                                das.wL = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().wL;
                                das.wC = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().wC;
                                das.cR1 = sd.cR1 ?? 0;
                                das.cR2 = sd.cR2 ?? 0;
                                das.cR3 = sd.cR3 ?? 0;
                                das.cR4 = sd.cR4 ?? 0;
                                das.cR5 = sd.cR5 ?? 0;
                                das.loopcnt = sd.loopcnt ?? 0;
                                das.position = sd.position.ToString() == string.Empty ? "" : sd.position;
                                das.psI = sd.psI ?? 0;
                                das.psV = sd.psV ?? 0;
                                double? sdhr = Math.Round(Convert.ToDouble(sd.runHours), 2);
                                das.runHours = (float?)sdhr ?? 0.0F;
                                das.seqName = sd.seqName;
                                das.siteID = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().siteID;
                                das.stackMfgID = sd.stackMfgID;
                                das.state = sd.state;
                                das.stepNumber = sd.stepNumber ?? 0;
                                das.verM = sqmtsdevice.Where(e => e.deviceID == deviceid).FirstOrDefault().verM;
                                das.status = sd.status ?? 0;
                                double? sdcm = Math.Round(Convert.ToDouble(sd.cumulativeHours), 2);
                                das.cumulativeHours = (float?)sdcm ?? 0.0F;
                                das.interLock = sd.interLock;
                                das.scriptLoopCnt = sd.scriptLoopCnt;
                                das.seqLoopCnt = sd.seqLoopCnt;
                                das.scriptStep = sd.scriptStep;
                                das.seqStep = sd.seqStep;
                                //das.cumulativeHours = sd.cumulativeHours == 0 ? das.runHours : sd.cumulativeHours;
                                //das.stkStatus = int.Parse(sd["status1"].ToString()) == 0 ? 0 : int.Parse(sd["status1"].ToString());
                                sqrdList.Add(das);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        sqrdLive.deviceAndStack = sqrdList;
                        return Ok(sqrdLive);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                sysMessage = ex.Message;
            }
            return Ok(sysMessage);
        }
        [HttpGet]
        [Route("AllSites")]
        public IActionResult GetAllSites(string user = "sa")
        {
            List<Site> siteList = new List<Site>();
            if (user == "sa")
            {
                siteList = _context.site.Where(x => !x.name.Contains("test")).ToList();
            }
            foreach (Site s in siteList)
            {
                Org o = _context.org.Find(s.orgID);
                s.org = o;
            }
            return Ok(siteList);
        }

        //[HttpGet, Route("CustomerView")]
        //public IActionResult CustomerView(string user="sa")
        //{
        //    List<Guid> orgs = _context.org.Select(x => x.OrgID).ToList();
        //    var stackdata = _context.mtsStackDataNew.GroupBy(x=>x.)
        //    List<TSStackData> stkdata = from x in _context.mtsStackDataNew join
        //                                d in _context.site on
        //                                x.stackMfgID equals d.id
        //                                select new
        //                                {
        //                                    id = x.stackMfgID,
        //                                    h2 = x.hT
        //                                };
        //}

        [HttpGet, Route("StackColor")]
        public IActionResult GetColor()
        {
            //List<ColorConfig> cc = _context.colorConfig.ToList();
            string config = _context.sconfig.SingleOrDefault().colorConfig;
            return Ok(config);
        }
        [HttpGet, Route("DeviceColor")]
        public IActionResult GetDeviceColor()
        {
            //List<ColorConfig> cc = _context.colorConfig.ToList();
            string config = _context.equipmentConfiguration.FirstOrDefault().colorConfig;
            return Ok(config);
        }
    }

    //public class StackConfigList
    //{
    //    public string stackPosition { get; set; }
    //    public StackConfig stkConfig { get; set; }
    //}
}
