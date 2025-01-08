using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Ohmium.BusinessLogic.Lotus;
using Ohmium.Models.EFModels;
using Ohmium.Repository;
using Microsoft.Extensions.Logging;
using Ohmium.Models.EFModels.LotusModels;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Ohmium.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using System.IO;
using System.Runtime.CompilerServices;
using Ohmium.Controllers.SQLite;
using Azure.Identity;

namespace Ohmium.Controllers.RMCCAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDatasController : ControllerBase
    {
        private readonly SensorContext _context;
        private readonly CacheContext _cache;
        private readonly ILogger _logger;
        private readonly IRMCCRepository<StackTestRunHours> _hr;
        public DeviceDatasController(SensorContext context, ILogger<DeviceDatasController> logger, CacheContext cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }


        [HttpPost, Route("PostDeviceDataList")]
        public async Task<ActionResult<MTSDeviceDataNew>> PostDeviceDataList(List<MTSDeviceDataNew> deviceDataList)
        {
            #region Fetch Device data
            //Logic to fetch the Device stack data once --sasanka--26-09-2022
            string errorLog = "";
            List<Stack> deviceStack = null;
            List<StackTestRunHours> stackTestRunHours = new List<StackTestRunHours>();
            stackTestRunHours = _context.stackTestRunHours.OrderByDescending(e => e.cumulativeHours).ToList();
            List<StackTestRunHours> stackTestRunHoursList = new List<StackTestRunHours>();
            if (deviceDataList.Count > 0)
            {
                MTSDeviceDataNew dd = deviceDataList[0];
                deviceStack = _context.stack.Where(x => x.deviceID == dd.deviceID && x.status == 1).ToList();

                foreach (Stack stk in deviceStack)
                {
                    StackTestRunHours str = new StackTestRunHours();
                    try
                    {
                        str = stackTestRunHours.FirstOrDefault(e => e.stkMfgId == stk.stackMfgID);
                        if (str != null)
                            stackTestRunHoursList.Add(str);
                    }
                    catch (Exception ex)
                    {
                        errorLog += $": cumulative run hours for stack {stk.stackMfgID} could not be fetched with error message {ex.Message + ex.InnerException}. Possibly first run of the stackP";
                    }
                }
            }
            #endregion

            foreach (MTSDeviceDataNew deviceData in deviceDataList)
            {
                int stkcnt = 0;
                try
                {
                    //DeviceData deviceData = (DeviceData)dd;
                    //if(stackIdList ==null)
                    //    stackIdList= _context.stack.Where(x => x.deviceID == deviceData.deviceID).Select(x=>x.stackMfgID).ToList();

                    if (DeviceExists(deviceData.deviceID, deviceData.timeStamp))
                    {
                        try
                        {
                            deviceData.timeStamp = DateTime.Parse(deviceData.strTime, null, DateTimeStyles.RoundtripKind);
                           
                            _context.mtsDeviceDataNew.Add(deviceData);
                            //_cache.mtsDeviceData.Add(deviceData);
                        }
                        catch (Exception ex)
                        {
                            errorLog += ":DeviceModel Exception: " + ex.Message + "DeviceModel Inner Exception: " + ex.InnerException;
                        }
                        try
                        {
                            MTSStackDataNew A01 = null;
                            MTSStackDataNew A02 = null;
                            MTSStackDataNew A03 = null;
                            MTSStackDataNew A04 = null;
                            MTSStackDataNew A05 = null;
                            MTSStackDataNew A06 = null;
                            //Instead of try catch using "if block" to check if the stack exist
                            //try
                            //{
                            if (deviceData.S01 != null)
                            {
                                stkcnt++;
                                deviceData.S01.deviceID = deviceData.deviceID;
                                deviceData.S01.timeStamp = deviceData.timeStamp;
                                deviceData.S01.position = "s01" + "-" + deviceData.deviceID;
                                //Fixing the performance issue ,No need to make multiple call to DB--sasanka--26-09-2022
                                //deviceData.S01.stackMfgID = _context.stack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).stackMfgID;
                                deviceData.S01.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                try
                                {
                                    deviceData.S01.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    try
                                    {
                                        deviceData.S01.cumulativeHours = deviceData.S01.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S01.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S01 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S01.cumulativeHours = null;
                                    }
                                    A01 = deviceData.S01;
                                    _context.mtsStackDataNew.Add(A01);
                                }
                                catch
                                {
                                    errorLog += $"S01 possibly not allocated to a stack in {deviceData.deviceID}";
                                }

                                //_cache.mtsStackData.Add(A01);
                            }

                            if (deviceData.S02 != null)
                            {
                                stkcnt++;
                                deviceData.S02.deviceID = deviceData.deviceID;
                                //Fixing the performance issue ,No need to make multiple call to DB--sasanka--26-09-2022
                                try
                                {
                                    deviceData.S02.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S02" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S02.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S02" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S02.timeStamp = deviceData.timeStamp;
                                    deviceData.S02.position = "s02" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S02.cumulativeHours = deviceData.S02.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S02.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S02 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S02.cumulativeHours = null;
                                    }
                                    A02 = deviceData.S02;
                                    _context.mtsStackDataNew.Add(A02);
                                    //_cache.mtsStackData.Add(A02);
                                }
                                catch
                                {
                                    errorLog += $"S02 possibly not allocated to a stack in {deviceData.deviceID}";
                                }
                            }
                            if (deviceData.S03 != null)
                            {
                                stkcnt++;
                                deviceData.S03.deviceID = deviceData.deviceID;
                                try
                                {
                                    deviceData.S03.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S03" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S03.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S03" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0'); ;
                                    deviceData.S03.timeStamp = deviceData.timeStamp;
                                    deviceData.S03.position = "s03" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S03.cumulativeHours = deviceData.S03.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S03.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S03 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S03.cumulativeHours = null;
                                    }
                                    A03 = deviceData.S03;
                                    _context.mtsStackDataNew.Add(A03);
                                    //_cache.mtsStackData.Add(A03);
                                }
                                catch
                                {
                                    errorLog += $"S03 possibly not allocated to a stack in {deviceData.deviceID}";
                                }

                            }
                            if (deviceData.S04 != null)
                            {
                                stkcnt++;
                                deviceData.S04.deviceID = deviceData.deviceID;
                                try
                                {
                                    deviceData.S04.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S04" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S04.timeStamp = deviceData.timeStamp;
                                    deviceData.S04.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S04" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S04.position = "s04" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S04.cumulativeHours = deviceData.S04.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S04.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S04 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S04.cumulativeHours = null;
                                    }
                                    A04 = deviceData.S04;
                                    _context.mtsStackDataNew.Add(A04);
                                    //_cache.mtsStackData.Add(A04);
                                }
                                catch
                                {
                                    errorLog += $"S04 possibly not allocated to a stack in {deviceData.deviceID}";
                                }

                            }
                            if (deviceData.S05 != null)
                            {
                                stkcnt++;
                                deviceData.S05.deviceID = deviceData.deviceID;
                                try
                                {
                                    deviceData.S05.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S05" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S05.timeStamp = deviceData.timeStamp;
                                    deviceData.S05.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S05" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S05.position = "s05" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S05.cumulativeHours = deviceData.S05.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S05.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": s05 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S05.cumulativeHours = null;
                                    }

                                    A05 = deviceData.S05;
                                    _context.mtsStackDataNew.Add(A05);
                                    //_cache.mtsStackData.Add(A05);
                                }
                                catch
                                {
                                    errorLog += $"S05 possibly not allocated to a stack in {deviceData.deviceID}";
                                }

                            }
                            if (deviceData.S06 != null)
                            {
                                stkcnt++;
                                deviceData.S06.timeStamp = deviceData.timeStamp;
                                deviceData.S06.deviceID = deviceData.deviceID;
                                try
                                {
                                    deviceData.S06.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S06" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S06.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S06" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S06.position = "s06" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S06.cumulativeHours = deviceData.S06.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S06.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S06 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S06.cumulativeHours = null;
                                    }

                                    A06 = deviceData.S06;
                                    _context.mtsStackDataNew.Add(A06);
                                    //_cache.mtsStackData.Add(A06);
                                }
                                catch
                                {
                                    errorLog += $"S06 possibly not allocated to a stack in {deviceData.deviceID}";
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            //errorLog += $"|stack S0{stkcnt} not pushed on test stand {deviceDataList[0].deviceID} " + ex.Message;
                            //_logger.LogError(errorLog);
                        }
                    }
                    else
                    {
                        errorLog += $"| Test Stand {deviceDataList[0].deviceID} duplicate record exists";
                    }
                    //                        return Ok(NotFound(errorLog));
                }
                catch (Exception ex)
                {
                    //LotusDeviceData lotusDeviceData = (LotusDeviceData)dd;
                    _logger.LogError(ex.Message);
                    errorLog += ":" + ex.Message + "|" + ex.InnerException;
                    //                  return Ok();
                }
            }
            if (errorLog != "")
            {
                DeviceDataLog dlog = new DeviceDataLog()
                {
                    description = errorLog,
                    timeStamp = DateTime.UtcNow
                };
                _cache.deviceDataLog.Add(dlog);
            }
            await _cache.SaveChangesAsync();
            var result = await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost, Route("DeviceDataToSQLite")]
        public async Task<ActionResult<MTSDeviceDataNew>> DeviceDataToSQLite(List<MTSDeviceData> deviceDataList)
        {
            int count = _context.mtsStackData.Count();
            #region Delete Device data
            if(count>5000)
            {
                _context.Database.ExecuteSqlRaw("TRUNCATE TABLE mtsdevicedata");
                _context.Database.ExecuteSqlRaw("TRUNCATE TABLE mtsstackdata");
            }
            #endregion
            #region Fetch Device data
            //Logic to fetch the Device stack data once --sasanka--26-09-2022
            string errorLog = "";
            List<Stack> deviceStack = null;
            List<StackTestRunHours> stackTestRunHours = new List<StackTestRunHours>();
            stackTestRunHours = _context.stackTestRunHours.OrderByDescending(e => e.cumulativeHours).ToList();
            List<StackTestRunHours> stackTestRunHoursList = new List<StackTestRunHours>();
            if (deviceDataList.Count > 0)
            {
                MTSDeviceData dd = deviceDataList[0];
                deviceStack = _context.stack.Where(x => x.deviceID == dd.deviceID && x.status == 1).ToList();

                foreach (Stack stk in deviceStack)
                {
                    StackTestRunHours str = new StackTestRunHours();
                    try
                    {
                        str = stackTestRunHours.FirstOrDefault(e => e.stkMfgId == stk.stackMfgID);
                        if (str != null)
                            stackTestRunHoursList.Add(str);
                    }
                    catch (Exception ex)
                    {
                        errorLog += $": cumulative run hours for stack {stk.stackMfgID} could not be fetched with error message {ex.Message + ex.InnerException}. Possibly first run of the stackP";
                    }
                }
            }
            #endregion



            foreach (MTSDeviceData deviceData in deviceDataList)
            {
                int stkcnt = 0;
                try
                {

                    if (DeviceExists(deviceData.deviceID, deviceData.timeStamp))
                    {
                        try
                        {
                            deviceData.timeStamp = DateTime.Parse(deviceData.strTime, null, DateTimeStyles.RoundtripKind);
                            _context.mtsDeviceData.Add(deviceData);
                            //_cache.mtsDeviceData.Add(deviceData);
                        }
                        catch (Exception ex)
                        {
                            errorLog += ":DeviceModel Exception: " + ex.Message + "DeviceModel Inner Exception: " + ex.InnerException;
                        }
                        try
                        {
                            MTSStackData A01 = null;
                            MTSStackData A02 = null;
                            MTSStackData A03 = null;
                            MTSStackData A04 = null;
                            MTSStackData A05 = null;
                            MTSStackData A06 = null;
                            //Instead of try catch using "if block" to check if the stack exist
                            //try
                            //{
                            if (deviceData.S01 != null)
                            {
                                stkcnt++;
                                deviceData.S01.deviceID = deviceData.deviceID;
                                deviceData.S01.timeStamp = deviceData.timeStamp;
                                deviceData.S01.position = "s01" + "-" + deviceData.deviceID;
                                //Fixing the performance issue ,No need to make multiple call to DB--sasanka--26-09-2022
                                //deviceData.S01.stackMfgID = _context.stack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).stackMfgID;
                                try
                                {
                                    deviceData.S01.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S01.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');

                                    try
                                    {
                                        deviceData.S01.cumulativeHours = deviceData.S01.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S01.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S01 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S01.cumulativeHours = null;
                                    }
                                    A01 = deviceData.S01;
                                    _context.mtsStackData.Add(A01);
                                }
                                catch
                                {
                                    errorLog += $"S01 possibly not allocated to a stack in {deviceData.deviceID}";
                                }
                            }

                            if (deviceData.S02 != null)
                            {
                                stkcnt++;
                                deviceData.S02.deviceID = deviceData.deviceID;
                                //Fixing the performance issue ,No need to make multiple call to DB--sasanka--26-09-2022
                                try
                                {
                                    deviceData.S02.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S02" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S02.timeStamp = deviceData.timeStamp;
                                    deviceData.S02.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S02" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S02.position = "s02" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S02.cumulativeHours = deviceData.S02.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S02.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S02 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S02.cumulativeHours = null;
                                    }
                                    A02 = deviceData.S02;
                                    _context.mtsStackData.Add(A02);
                                }
                                catch
                                {
                                    errorLog += $"S02 possibly not allocated to a stack in {deviceData.deviceID}";
                                }
                            }
                            if (deviceData.S03 != null)
                            {
                                stkcnt++;
                                deviceData.S03.deviceID = deviceData.deviceID;
                                try
                                {
                                    deviceData.S03.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S03" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S03.timeStamp = deviceData.timeStamp;
                                    deviceData.S03.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S03" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S03.position = "s03" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S03.cumulativeHours = deviceData.S03.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S03.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S03 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S03.cumulativeHours = null;
                                    }
                                    A03 = deviceData.S03;
                                    _context.mtsStackData.Add(A03);
                                }
                                catch
                                {
                                    errorLog += $"S03 possibly not allocated to a stack in {deviceData.deviceID}";
                                }
                            }
                            if (deviceData.S04 != null)
                            {
                                stkcnt++;
                                deviceData.S04.deviceID = deviceData.deviceID;
                                try
                                {
                                    deviceData.S04.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S04" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S04.timeStamp = deviceData.timeStamp;
                                    deviceData.S04.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S04" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S04.position = "s04" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S04.cumulativeHours = deviceData.S04.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S04.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S04 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S04.cumulativeHours = null;
                                    }
                                    A04 = deviceData.S04;
                                    _context.mtsStackData.Add(A04);
                                }
                                catch
                                {
                                    errorLog += $"S04 possibly not allocated to a stack in {deviceData.deviceID}";
                                }

                            }
                            if (deviceData.S05 != null)
                            {
                                stkcnt++;
                                deviceData.S05.deviceID = deviceData.deviceID;
                                try
                                {
                                    deviceData.S05.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S05" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S05.timeStamp = deviceData.timeStamp;
                                    deviceData.S05.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S05" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S05.position = "s05" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S05.cumulativeHours = deviceData.S05.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S05.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": s05 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S05.cumulativeHours = null;
                                    }

                                    A05 = deviceData.S05;
                                    _context.mtsStackData.Add(A05);
                                }
                                catch
                                {
                                    errorLog += $"S05 possibly not allocated to a stack in {deviceData.deviceID}";
                                }
                            }
                            if (deviceData.S06 != null)
                            {
                                stkcnt++;
                                deviceData.S06.timeStamp = deviceData.timeStamp;
                                deviceData.S06.deviceID = deviceData.deviceID;
                                try
                                {
                                    deviceData.S06.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S06" && x.deviceID == deviceData.deviceID).stackMfgID;
                                    deviceData.S06.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S06" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
                                    deviceData.S06.position = "s06" + "-" + deviceData.deviceID;
                                    try
                                    {
                                        deviceData.S06.cumulativeHours = deviceData.S06.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S06.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLog += $": S06 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
                                        deviceData.S06.cumulativeHours = null;
                                    }

                                    A06 = deviceData.S06;
                                    _context.mtsStackData.Add(A06);
                                }
                                catch
                                {
                                    errorLog += $"S06 possibly not allocated to a stack in {deviceData.deviceID}";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorLog += $"|stack S0{stkcnt} not pushed on test stand {deviceDataList[0].deviceID} " + ex.Message;
                            //_logger.LogError(errorLog);
                        }
                    }
                    else
                    {
                        errorLog += $"| Test Stand {deviceDataList[0].deviceID} duplicate record exists";
                    }
                    //                        return Ok(NotFound(errorLog));
                }
                catch (Exception ex)
                {
                    //LotusDeviceData lotusDeviceData = (LotusDeviceData)dd;
                    _logger.LogError(ex.Message);
                    errorLog += ":" + ex.Message + "|" + ex.InnerException;
                    //                  return Ok();
                }
            }
            
            //PushData pd = new PushData();
            //int count = _context.mtsStackData.ToList().Count();

            //if (count > 5000)
            //{
            //    errorLog += await pd.DeleteSQLiteLiveData();
            //}
            //if (errorLog != "")
            //{
            //    DeviceDataLog dlog = new DeviceDataLog()
            //    {
            //        description = "SqLite Errors: " + errorLog,
            //        timeStamp = DateTime.UtcNow
            //    };
            //    _cache.deviceDataLog.Add(dlog);
            //}
            await _context.SaveChangesAsync();

            return Ok();
        }
        //[HttpPost, Route("DeviceDataToSQLite")]
        //public async Task<ActionResult<MTSDeviceDataNew>> DeviceDataToSQLite(List<MTSDeviceData> deviceDataList)
        //{
        //    #region Fetch Device data
        //    //Logic to fetch the Device stack data once --sasanka--26-09-2022
        //    string errorLog = "";
        //    List<Stack> deviceStack = null;
        //    List<StackTestRunHours> stackTestRunHours = new List<StackTestRunHours>();
        //    stackTestRunHours = _context.stackTestRunHours.OrderByDescending(e => e.cumulativeHours).ToList();
        //    List<StackTestRunHours> stackTestRunHoursList = new List<StackTestRunHours>();
        //    if (deviceDataList.Count > 0)
        //    {
        //        MTSDeviceData dd = deviceDataList[0];
        //        deviceStack = _context.stack.Where(x => x.deviceID == dd.deviceID && x.status == 1).ToList();

        //        foreach (Stack stk in deviceStack)
        //        {
        //            StackTestRunHours str = new StackTestRunHours();
        //            try
        //            {
        //                str = stackTestRunHours.FirstOrDefault(e => e.stkMfgId == stk.stackMfgID);
        //                if (str != null)
        //                    stackTestRunHoursList.Add(str);
        //            }
        //            catch (Exception ex)
        //            {
        //                errorLog += $": cumulative run hours for stack {stk.stackMfgID} could not be fetched with error message {ex.Message + ex.InnerException}. Possibly first run of the stackP";
        //            }
        //        }
        //    }
        //    #endregion



        //    foreach (MTSDeviceData deviceData in deviceDataList)
        //    {
        //        int stkcnt = 0;
        //        try
        //        {

        //            if (DeviceExists(deviceData.deviceID, deviceData.timeStamp))
        //            {
        //                try
        //                {
        //                    deviceData.timeStamp = DateTime.Parse(deviceData.strTime, null, DateTimeStyles.RoundtripKind);
        //                    _cache.mtsDeviceData.Add(deviceData);
        //                    //_cache.mtsDeviceData.Add(deviceData);
        //                }
        //                catch (Exception ex)
        //                {
        //                    errorLog += ":DeviceModel Exception: " + ex.Message + "DeviceModel Inner Exception: " + ex.InnerException;
        //                }
        //                try
        //                {
        //                    MTSStackData A01 = null;
        //                    MTSStackData A02 = null;
        //                    MTSStackData A03 = null;
        //                    MTSStackData A04 = null;
        //                    MTSStackData A05 = null;
        //                    MTSStackData A06 = null;
        //                    //Instead of try catch using "if block" to check if the stack exist
        //                    //try
        //                    //{
        //                    if (deviceData.S01 != null)
        //                    {
        //                        stkcnt++;
        //                        deviceData.S01.deviceID = deviceData.deviceID;
        //                        deviceData.S01.timeStamp = deviceData.timeStamp;
        //                        deviceData.S01.position = "s01" + "-" + deviceData.deviceID;
        //                        //Fixing the performance issue ,No need to make multiple call to DB--sasanka--26-09-2022
        //                        //deviceData.S01.stackMfgID = _context.stack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).stackMfgID;
        //                        try
        //                        {
        //                            deviceData.S01.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).stackMfgID;
        //                            deviceData.S01.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S01" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');

        //                            try
        //                            {
        //                                deviceData.S01.cumulativeHours = deviceData.S01.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S01.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                errorLog += $": S01 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
        //                                deviceData.S01.cumulativeHours = null;
        //                            }
        //                            A01 = deviceData.S01;
        //                            _cache.mtsStackData.Add(A01);
        //                        }
        //                        catch
        //                        {
        //                            errorLog += $"S01 possibly not allocated to a stack in {deviceData.deviceID}";
        //                        }
        //                    }

        //                    if (deviceData.S02 != null)
        //                    {
        //                        stkcnt++;
        //                        deviceData.S02.deviceID = deviceData.deviceID;
        //                        //Fixing the performance issue ,No need to make multiple call to DB--sasanka--26-09-2022
        //                        try
        //                        {
        //                            deviceData.S02.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S02" && x.deviceID == deviceData.deviceID).stackMfgID;
        //                            deviceData.S02.timeStamp = deviceData.timeStamp;
        //                            deviceData.S02.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S02" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
        //                            deviceData.S02.position = "s02" + "-" + deviceData.deviceID;
        //                            try
        //                            {
        //                                deviceData.S02.cumulativeHours = deviceData.S02.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S02.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                errorLog += $": S02 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
        //                                deviceData.S02.cumulativeHours = null;
        //                            }
        //                            A02 = deviceData.S02;
        //                            _cache.mtsStackData.Add(A02);
        //                        }
        //                        catch
        //                        {
        //                            errorLog += $"S02 possibly not allocated to a stack in {deviceData.deviceID}";
        //                        }
        //                    }
        //                    if (deviceData.S03 != null)
        //                    {
        //                        stkcnt++;
        //                        deviceData.S03.deviceID = deviceData.deviceID;
        //                        try
        //                        {
        //                            deviceData.S03.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S03" && x.deviceID == deviceData.deviceID).stackMfgID;
        //                            deviceData.S03.timeStamp = deviceData.timeStamp;
        //                            deviceData.S03.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S03" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
        //                            deviceData.S03.position = "s03" + "-" + deviceData.deviceID;
        //                            try
        //                            {
        //                                deviceData.S03.cumulativeHours = deviceData.S03.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S03.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                errorLog += $": S03 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
        //                                deviceData.S03.cumulativeHours = null;
        //                            }
        //                            A03 = deviceData.S03;
        //                            _cache.mtsStackData.Add(A03);
        //                        }
        //                        catch
        //                        {
        //                            errorLog += $"S03 possibly not allocated to a stack in {deviceData.deviceID}";
        //                        }
        //                    }
        //                    if (deviceData.S04 != null)
        //                    {
        //                        stkcnt++;
        //                        deviceData.S04.deviceID = deviceData.deviceID;
        //                        try
        //                        {
        //                            deviceData.S04.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S04" && x.deviceID == deviceData.deviceID).stackMfgID;
        //                            deviceData.S04.timeStamp = deviceData.timeStamp;
        //                            deviceData.S04.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S04" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
        //                            deviceData.S04.position = "s04" + "-" + deviceData.deviceID;
        //                            try
        //                            {
        //                                deviceData.S04.cumulativeHours = deviceData.S04.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S04.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                errorLog += $": S04 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
        //                                deviceData.S04.cumulativeHours = null;
        //                            }
        //                            A04 = deviceData.S04;
        //                            _cache.mtsStackData.Add(A04);
        //                        }
        //                        catch
        //                        {
        //                            errorLog += $"S04 possibly not allocated to a stack in {deviceData.deviceID}";
        //                        }

        //                    }
        //                    if (deviceData.S05 != null)
        //                    {
        //                        stkcnt++;
        //                        deviceData.S05.deviceID = deviceData.deviceID;
        //                        try
        //                        {
        //                            deviceData.S05.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S05" && x.deviceID == deviceData.deviceID).stackMfgID;
        //                            deviceData.S05.timeStamp = deviceData.timeStamp;
        //                            deviceData.S05.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S05" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
        //                            deviceData.S05.position = "s05" + "-" + deviceData.deviceID;
        //                            try
        //                            {
        //                                deviceData.S05.cumulativeHours = deviceData.S05.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S05.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                errorLog += $": s05 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
        //                                deviceData.S05.cumulativeHours = null;
        //                            }

        //                            A05 = deviceData.S05;
        //                            _cache.mtsStackData.Add(A05);
        //                        }
        //                        catch
        //                        {
        //                            errorLog += $"S05 possibly not allocated to a stack in {deviceData.deviceID}";
        //                        }
        //                    }
        //                    if (deviceData.S06 != null)
        //                    {
        //                        stkcnt++;
        //                        deviceData.S06.timeStamp = deviceData.timeStamp;
        //                        deviceData.S06.deviceID = deviceData.deviceID;
        //                        try
        //                        {
        //                            deviceData.S06.stackMfgID = deviceStack.FirstOrDefault(x => x.stackPosition == "S06" && x.deviceID == deviceData.deviceID).stackMfgID;
        //                            deviceData.S06.interLock = Convert.ToString(deviceStack.FirstOrDefault(x => x.stackPosition == "S06" && x.deviceID == deviceData.deviceID).status, 2).PadLeft(16, '0');
        //                            deviceData.S06.position = "s06" + "-" + deviceData.deviceID;
        //                            try
        //                            {
        //                                deviceData.S06.cumulativeHours = deviceData.S06.runHours + stackTestRunHoursList.Where(e => e.stkMfgId == deviceData.S06.stackMfgID).Sum(e => e.cumulativeHours) ?? 0;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                errorLog += $": S06 cumulative Hours fetch resulted in error on teststand {deviceData.deviceID}";
        //                                deviceData.S06.cumulativeHours = null;
        //                            }

        //                            A06 = deviceData.S06;
        //                            _cache.mtsStackData.Add(A06);
        //                        }
        //                        catch
        //                        {
        //                            errorLog += $"S06 possibly not allocated to a stack in {deviceData.deviceID}";
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    errorLog += $"|stack S0{stkcnt} not pushed on test stand {deviceDataList[0].deviceID} " + ex.Message;
        //                    //_logger.LogError(errorLog);
        //                }
        //            }
        //            else
        //            {
        //                errorLog += $"| Test Stand {deviceDataList[0].deviceID} duplicate record exists";
        //            }
        //            //                        return Ok(NotFound(errorLog));
        //        }
        //        catch (Exception ex)
        //        {
        //            //LotusDeviceData lotusDeviceData = (LotusDeviceData)dd;
        //            _logger.LogError(ex.Message);
        //            errorLog += ":" + ex.Message + "|" + ex.InnerException;
        //            //                  return Ok();
        //        }
        //    }
        //    PushData pd = new PushData();
        //    int count = _cache.mtsStackData.ToList().Count();

        //    if (count > 5000)
        //    {
        //        errorLog += await pd.DeleteSQLiteLiveData();
        //    }
        //    if (errorLog != "")
        //    {
        //        DeviceDataLog dlog = new DeviceDataLog()
        //        {
        //            description = "SqLite Errors: " + errorLog,
        //            timeStamp = DateTime.UtcNow
        //        };
        //        _cache.deviceDataLog.Add(dlog);
        //    }
        //    await _cache.SaveChangesAsync();

        //    return Ok();
        //}

        [HttpGet]
        [Route("GetTestStandConfig")]
        public ActionResult<TestProfileConfig> RunProfile()
        {
            return Ok(_context.testProfileConfig.FirstOrDefault());

        }            // DELETE: api/DeviceDatas/5

        [HttpGet]
        [Route("GetColorParams")]
        public ActionResult GetColorParams()
        {
            return Ok(_context.thresholdconfigs.ToList());
        }
        private bool DeviceExists(string id, DateTime time)
        {
            return _context.device.Any(e => e.EqMfgID == id) && !_context.deviceData.Any(x => x.deviceID == id && x.timeStamp == time);
        }

        [HttpGet]
        public ActionResult<TestProfileConfig> RunTestSteps(string ProfileName)
        {
            int id = _context.runProfile.Single(x => x.profileName == ProfileName).id;
            return Ok(_context.stkStep.Where(x => x.stkRunProfileID == id));

        }            // DELETE: api/DeviceDatas/5

    }
}
