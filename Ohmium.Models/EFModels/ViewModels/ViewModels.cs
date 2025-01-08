using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ohmium.Models.EFModels.ViewModels
{
    #region  // region to comment old lotus Models
    public class lotusViewModel
    {

        public string StrTime { get; set; }
        public string SiteID { get; set; }

        public string DeviceID { get; set; }
        public string DeviceType { get; set; }
        public string Ver { get; set; }
        public int Status { get; set; }
        public SYS SYS { get; set; }
        public LHCVM LHC { get; set; }        
        public LPCVM LPC { get; set; }
       
        public LCCVM LCC { get; set; }

        public LWC LWC { get; set; }

        public LTC LTC { get; set; }
    }

    public class LHCVM
    {  
        public LHC C01 { get; set; }
        public LHC C02 { get; set; }
        public LHC C03 { get; set; }
        public LHC C04 { get; set; }
    }

    public class LPCVM
    {
        public LPC C01 { get; set; }
        public LPC C02 { get; set; }
        public LPC C03 { get; set; }
        public LPC C04 { get; set; }

    }
    public class LCCVM
    {
        public LCC C01 { get; set; }
        public LCC C02 { get; set; }
    }

    // new view models
    public class Alarm
    {
        public string setpointA { get; set; }
        public string mode { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public string priority { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string documentation { get; set; }
        public string valueSource { get; set; }
        public string dataType { get; set; }
        public string engHigh { get; set; }
        public string engLow { get; set; }

        public string engUnit { get; set; }
        public string tagType { get; set; }

        public List<Alarm> alarms { get; set; }
     
    }

    public class LWCVM
    {
        public string name { get; set; }
        public string tagType { get; set; }
        public List <Tag> tags { get; set; }
    }


    public class LCCVMNew
    {
        public string name { get; set; }
        public string tagType { get; set; }
        public List<Tag> tags { get; set; }
    }

    public class LHCVMNew
    {
        public string name { get; set; }
        public string tagType { get; set; }
        public List<Tag> tags { get; set; }
    }

    public class LTCVMNew
    {
        public string name { get; set; }
        public string tagType { get; set; }
        public List<Tag> tags { get; set; }
    }

    public class LPCVMNew
    {
        public string name { get; set; }
        public string tagType { get; set; }
        public List<Tag> tags { get; set; }
    }


    //  "name": "LWC",
    //"tagType": "UdtType",
    //"tags": [
    //  {
    //    "name": "WSV701P1",
    //    "documentation": "Water Solenoid Valve",
    //    "valueSource": "OPC",
    //    "dataType": "Boolean",
    //    "engHigh": "",
    //    "engLow": "",
    //    "engUnit": "",
    //    "tagType": "AtomicTag",
    //    "alarms": [
    //      {
    //        "setpointA": null,
    //        "mode": "AboveValue",
    //        "name": "",
    //        "label": "",
    //        "priority": "Low"
    //      }
    //    ]
    //  },
    public class lotusViewModelNew
    {


    }
    #endregion // end region to comment old lotus Models

    [NotMapped]
    public class SingleAPIViewModel
    {
        public RunProfile runProfile { get; set; }
        public List<RunStepTemplateGroup> sequenceName { get; set; }
        public List<List<StackPhaseSetting>>  stackPhaseSettings { get; set; }
    }

    [NotMapped]
    public class StackDeviceData
    {
        public string deviceID { get; set; }
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        public string strTime { get; set; }
        public string siteID { get; set; }
        public Guid configID { get; set; }
        public string ver { get; set; }
        public int status { get; set; }
        public float? wL { get; set; }
        public float? dwP { get; set; }
        public float? wC { get; set; }
        public float? dwT { get; set; }
        public float? hxiT { get; set; }
        public float? hxoT { get; set; }
        public float? wPp { get; set; }
        public string verM { get; set; }
        public Int16? CommStatus { get; set; }
        public string position { get; set; }
        public string stackMfgID { get; set; }
        public float? wF { get; set; }
        public float? swT { get; set; }
        public float? swP { get; set; }
        public float? hT { get; set; }
        public float? hP { get; set; }
        public float? psI { get; set; }
        public float? psV { get; set; }
        public float? cV1 { get; set; }
        public float? cV2 { get; set; }
        public float? cV3 { get; set; }
        public float? cV4 { get; set; }
        public float? cV5 { get; set; }
        public float? isF { get; set; } = 0;
        public float? cV6 { get; set; }
        public float? cR1 { get; set; }
        public float? cR2 { get; set; }
        public float? cR3 { get; set; }
        public float? cR4 { get; set; }
        public float? cR5 { get; set; }
        public float? cX1 { get; set; }
        public float? cX2 { get; set; }
        public float? cX3 { get; set; }
        public float? cX4 { get; set; }
        public float? cX5 { get; set; }
        public float? cR6 { get; set; }
        public float? CD { get; set; }
        public string state { get; set; } = "START";
        public float? runHours { get; set; } = 0.0F;
        public int? stepNumber { get; set; } = 0;
        public int? loopcnt { get; set; } = 0;
        public float? cM1 { get; set; }
        public float? cM2 { get; set; }
        public float? cM3 { get; set; }
        public float? cM4 { get; set; }
        public float? cM5 { get; set; }
        public string seqName { get; set; }
        public int stkStatus { get; set; }
    }

    public class StackPhaseSettingsNewViewModel
    {
        public List<List<StackPhaseSettingNew>> spsn { get; set; }
        public List<List<RunStep>> runStepLL { get; set; }
        public List<RunStepTemplateGroup> rstg { get; set; }
    }

    public class StkDataVM
    {
        public string deviceID { get; set; }
        public string stkID { get; set; }
    }

    //[NotMapped]
    //public class OrgSiteTestStandStack
    //{
    //    public List<Org> orgList { get; set; }
    //    public List<Site> siteList { get; set; }
    //    public List<Device> deviceList { get; set; }
    //    public List<Stack> stackList { get; set; }
    //}
    [NotMapped]
    public class OrgSiteTestStandStack
    {
        public List<Org> orgList { get; set; }
        public List<Site> siteList { get; set; }
        public List<Device> deviceList { get; set; }
        public List<Stack> stackList { get; set; }
    }
}
