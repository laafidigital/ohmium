
using Microsoft.Data.Sqlite;
using Ohmium.Models.EFModels;
using Ohmium.Repository;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ohmium.Models
{
    public class SQLiteSeedData
    {
        private readonly CacheContext _context;
        private readonly SensorContext _sc;
        public SQLiteSeedData(CacheContext context, SensorContext sc)
        {
            _context = context;
            _sc = sc;
        }

        public List<Org> AddOrgs()
        {
            StatusType st1 = new StatusType() { id = 1, name="Active" };
            StatusType st2= new StatusType() { id = 2, name = "Paused" };
            StatusType st3= new StatusType() { id = 3, name = "Inactive" };
            StatusType st4 = new StatusType() { id = 4, name = "Disabled" };
            _context.statusType.Add(st1);
            _context.statusType.Add(st2);
            _context.statusType.Add(st3);
            _context.statusType.Add(st4);
            _context.SaveChanges();
            Region r1 = new Region() { name = "North America", desc = "USA, Canada, Greenland" };
            Region r2 = new Region() { name = "Indo pacific", desc = "India, China, Srilanka" };
            _context.region.Add(r1);
            _context.region.Add(r2);
            _context.SaveChanges();
            Org o1 = new Org() { createdBy = "ramesh", createdOn = System.DateTime.UtcNow, OrgName = "Ohmium Fremont", updatedBy = "ramesh", status = "1", updatedOn = System.DateTime.UtcNow };
                Org o2 = new Org() { createdBy = "ramesh", createdOn = System.DateTime.UtcNow, OrgName = "Ohmium India", updatedBy = "ramesh", status = "1", updatedOn = System.DateTime.UtcNow };
                _context.org.Add(o1);
                _context.org.Add(o2);
                _context.SaveChanges();
            Guid sqlsiteid1 = _sc.site.FirstOrDefault(e => e.name == "S-Ohmium-US").id;
            Guid sqlsiteid2 = _sc.site.FirstOrDefault(e => e.name == "S-Ohmium-IND-CKB").id;
            Guid sqlsiteid3 = _sc.site.FirstOrDefault(e => e.name == "S-OHMIUM-IND-MAA").id;
            SQSite s1 = new SQSite() {sqlId=sqlsiteid1, name= "S-Ohmium-US", orgID=o1.OrgID,Region="North America",email= "david.placencia@ohmium.com", status="1", siteLat= 37.49755F,siteLng = -121.949F,h2Production=0,powerConsumption=0,siteEfficiency=0 };
            SQSite s2 = new SQSite() { sqlId = sqlsiteid2, name = "S-Ohmium-IND-CKB", orgID = o2.OrgID,Region="Indo pacific", email="dinesh@ohmium.com",status ="1", siteLat = 13.34098F, siteLng = 77.57533F, h2Production = 0, powerConsumption = 0, siteEfficiency = 0 };
            SQSite s3 = new SQSite() { sqlId = sqlsiteid3, name = "S-OHMIUM-IND-MAA", orgID = o2.OrgID, Region = "Indo pacific", email= "ravindran.s@ohmium.com", status = "1", siteLat = 13.34098F, siteLng = 77.57533F, h2Production = 0, powerConsumption = 0, siteEfficiency = 0 };
            _context.site.Add(s1);
            _context.site.Add(s2);
            _context.site.Add(s3);
            _context.SaveChanges();
            List<Device> divListUS = _sc.device.Where(e=>e.site.name== "S-Ohmium-US").ToList();
            List<Device> divListCKB = _sc.device.Where(e => e.site.name == "S-Ohmium-IND-CKB").ToList();
            List<Device> divListMAA = _sc.device.Where(e => e.site.name == "S-OHMIUM-IND-MAA").ToList();
            foreach (Device d in divListUS)
            {
                SQDevice d1 = new SQDevice();
                d1.siteID = d.siteID;
                d1.EqMfgID = d.EqMfgID;
                d1.comStatus=d.comStatus;
                d1.status = d.status;
                d1.statustype = d.statustype;
                d1.updatedBy = d.updatedBy;
                d1.createdBy = d.createdBy;
                d1.configID = d.configID;
                d1.siteEfficiency = d.siteEfficiency;
                d1.createdOn = d.createdOn;
                d1.deviceType= d.deviceType;
                d1.EqDes = d.EqDes;
                d1.h2Production= d.h2Production;
                d1.isRunning= d.isRunning;
                d1.lastDataReceivedOn= d.lastDataReceivedOn;
                d1.updatedOn= d.updatedOn;
                d1.nStack= d.nStack;
                _context.device.Add(d1);
                _context.SaveChanges();
            }


            foreach (Device d in divListCKB)
            {
                SQDevice d1 = new SQDevice();
                d1.siteID = d.siteID;
                d1.EqMfgID = d.EqMfgID;
                d1.comStatus = d.comStatus;
                d1.status = d.status;
                d1.statustype = d.statustype;
                d1.updatedBy = d.updatedBy;
                d1.createdBy = d.createdBy;
                d1.configID = d.configID;
                d1.siteEfficiency = d.siteEfficiency;
                d1.createdOn = d.createdOn;
                d1.deviceType = d.deviceType;
                d1.EqDes = d.EqDes;
                d1.h2Production = d.h2Production;
                d1.isRunning = d.isRunning;
                d1.lastDataReceivedOn = d.lastDataReceivedOn;
                d1.updatedOn = d.updatedOn;
                d1.nStack = d.nStack;

                _context.device.Add(d1);
                _context.SaveChanges();
            }

            foreach (Device d in divListMAA)
            {
                SQDevice d1 = new SQDevice();
                d1.siteID = d.siteID;
                d1.EqMfgID = d.EqMfgID;
                d1.comStatus = d.comStatus;
                d1.status = d.status;
                d1.statustype = d.statustype;
                d1.updatedBy = d.updatedBy;
                d1.createdBy = d.createdBy;
                d1.configID = d.configID;
                d1.siteEfficiency = d.siteEfficiency;
                d1.createdOn = d.createdOn;
                d1.deviceType = d.deviceType;
                d1.EqDes = d.EqDes;
                d1.h2Production = d.h2Production;
                d1.isRunning = d.isRunning;
                d1.lastDataReceivedOn = d.lastDataReceivedOn;
                d1.updatedOn = d.updatedOn;
                d1.nStack = d.nStack;
                _context.device.Add(d1);
                _context.SaveChanges();
            }

            string[] dividList = _context.device.ToList().Select(e => e.EqMfgID).ToArray();
            List<Stack> stkList = _sc.stack.ToList();
            foreach(Stack s in stkList)
            {
                SQStack stk = new SQStack();
                if (dividList.Contains(s.deviceID))
                {
                    stk.status = s.status;
                    stk.stackConfig = s.stackConfig;
                    stk.sStatus = s.sStatus;
                    stk.stackPosition = s.stackPosition;
                    stk.deviceID = s.deviceID;
                    stk.meaArea = s.meaArea;
                    stk.meaNum = s.meaNum;
                    stk.stackMfgID= s.stackMfgID;
                    stk.siteID = _context.device.FirstOrDefault(e=>e.EqMfgID==s.deviceID).siteID;
                    _context.stack.Add(stk);
                    _context.SaveChanges();
                }
            }
            return _context.org.ToList();
        }
    }
}
