using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Ohmium.Models.EFModels
{
    public class Segment
    {
        [Key]
        public Guid id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Org
    {
        [Key]
        public Guid OrgID { get; set; }
        [Required]
        [Display(Name = "ORGANIZATION")]
        public string OrgName { get; set; }
        [Display(Name = "STATUS")]
        //active, inactive, deleted, suspended
        public string status { get; set; }
        [Display(Name = "CREATED ON")]
        public DateTime createdOn { get; set; }
        [Display(Name = "UPDATED ON")]
        public DateTime? updatedOn { get; set; } = DateTime.UtcNow;
        [Display(Name = "CREATED BY")]
        public string createdBy { get; set; }
        [Display(Name = "UPDATED BY")]
        public string? updatedBy { get; set; } = "N/A";
    }

    public class OrgSegmentMapping
    {
        [Key]
        public Guid id { get; set; }
        [ForeignKey("org")]
        public Guid orgID { get; set; }
        [ForeignKey("segment")]
        public Guid segmentID { get; set; }
        public Org org { get; set; }
        public Segment segment { get; set; }
    }

    public class Region
    {
        [Key]
        public string name { get; set; }
        public string desc { get; set; }
    }

    public class Site
    {
        [Key]
        public Guid id { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [ForeignKey("org")]
        public Guid orgID { get; set; }
        public Org org { get; set; }
        [ForeignKey("reg")]
        public string Region { get; set; }
        public Region reg { get; set; }
        public float siteLat { get; set; }
        public float siteLng { get; set; }

        public Address address { get; set; }
        public List<Contact> contact { get; set; }
        [Required]
        public string email { get; set; }
        public string status { get; set; }
        public float h2Production { get; set; }
        public float powerConsumption { get; set; }
        public float siteEfficiency { get; set; }
    }

    public class SQSite
    {
        [Key]
        public Guid id { get; set; }
        public Guid sqlId { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [ForeignKey("org")]
        public Guid orgID { get; set; }
        public Org org { get; set; }
        [ForeignKey("reg")]
        public string Region { get; set; }
        public Region reg { get; set; }
        public float siteLat { get; set; }
        public float siteLng { get; set; }

        [Required]
        public string email { get; set; }
        public string status { get; set; }
        public float h2Production { get; set; }
        public float powerConsumption { get; set; }
        public float siteEfficiency { get; set; }
    }


    public class Contact
    {
        public int id { get; set; }
        [ForeignKey("site")]
        public Guid siteID { get; set; }
        public Site site { get; set; }
        public string contactName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class Address
    {
        [Key]
        public Guid id { get; set; }
        [ForeignKey("site")]
        public Guid sid { get; set; }
        public Site site { get; set; }
        [Required]
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        [Required]
        public string postalCode { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string state { get; set; }
        public string country { get; set; }
    }

    public class UserLogin
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime loginDateTime { get; set; }
    }

    /// <summary>
    /// Device refers to a System - single or aggretate units/equipments with their subsystems
    /// </summary>
    public class Device
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //varcar(max) in database
        [Display(Name ="Test Stand")]
        public string EqMfgID { get; set; }
        [Display(Name = "Test Stand Description")]
        public string EqDes { get; set; } = "Tester Description";
        [Display(Name = "Test stand Type")]
        public string deviceType { get; set; }
        [ForeignKey("site")]
        public Guid siteID { get; set; }
        public Site site { get; set; }
        [ForeignKey("ec")]
        [Display(Name ="Test Stand Config")]
        public Guid configID { get; set; }
        public EquipmentConfiguration ec { get; set; }
        public bool comStatus { get; set; }
        public float h2Production { get; set; }
        public float powerConsumption { get; set; }
        public float siteEfficiency { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public int nStack { get; set; }
        [ForeignKey("statustype")]
        public int? status { get; set; }
        public StatusType? statustype { get; set; }
        public string ver { get; set; }
        public string isRunning { get; set; }
        public DateTime lastDataReceivedOn { get; set; }
        public string dCmd { get; set; }
        public DateTime timeStamp { get; set; }
        public float? mnWbT { get; set; } = 0;
        public float? mxWbT { get; set; } = 0;
        public float? hxT { get; set; } = 0;

    }

    public class SQDevice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //varcar(max) in database
        [Display(Name = "Test Stand")]
        public string EqMfgID { get; set; }
        [Display(Name = "Test Stand Description")]
        public string EqDes { get; set; } = "Tester Description";
        [Display(Name = "Test stand Type")]
        public string deviceType { get; set; }
        [ForeignKey("site")]
        public Guid siteID { get; set; }
        public SQSite site { get; set; }
        [ForeignKey("ec")]
        [Display(Name = "Test Stand Config")]
        public Guid configID { get; set; }
        public EquipmentConfiguration ec { get; set; }
        public bool comStatus { get; set; }
        public float h2Production { get; set; }
        public float powerConsumption { get; set; }
        public float siteEfficiency { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public int nStack { get; set; }
        [ForeignKey("statustype")]
        public int? status { get; set; }
        public StatusType? statustype { get; set; }
        public string ver { get; set; }
        public string isRunning { get; set; }
        public DateTime lastDataReceivedOn { get; set; }
        public float? wL { get; set; }
        public float? wP { get; set; }
        public float? wC { get; set; }
        public float? wT { get; set; }
        [Display(Name = "HRT")]
        public float? hxiT { get; set; }
        public float? hxoT { get; set; }
        public float? HYS { get; set; }
        public float? wPp { get; set; }

    }
    //1-Active, 2-Paused, 3-Inactive, 4-Disabled 
    public class StatusType
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Status Type")]
        public string name { get; set; }
    }

    public class TestState
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Test State")]
        public string TestName { get; set; }
        public string TestDesc { get; set; }
    }

    public class Stack
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //varcar(max) in database
        public string stackMfgID { get; set; }
        [ForeignKey("sid")]
        public Guid siteID { get; set; }
        public Site sid { get; set; }
        [ForeignKey("device"), Display(Name ="Test Stand")]
        public string deviceID { get; set; }
        //[ForeignKey("scon")]
        public Guid stackConfig { get; set; }
        //public StackConfig scon { get; set; }
        public Device device { get; set; }
        public string stackPosition { get; set; }
        [Display(Name ="MEA Number")]
        public int meaNum { get; set; }
        [Display(Name = "MEA Area")]
        public float meaArea { get; set; }
        [ForeignKey("sStatus")]
        public int status { get; set; }
        public StatusType sStatus { get; set; }
        public bool? loop { get; set; }
        [Display(Name = "Stack Command")]
        public string command { get; set; } = "stop"; //start, pause,stop, Decommissioned


    }

    public class SQStack
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //varcar(max) in database
        public string stackMfgID { get; set; }
        [ForeignKey("sid")]
        public Guid siteID { get; set; }
        public SQSite sid { get; set; }
        [ForeignKey("device")]
        public string deviceID { get; set; }
        public SQDevice device { get; set; }
        [ForeignKey("scon")]
        public Guid stackConfig { get; set; }
        public StackConfig scon { get; set; }
        public string stackPosition { get; set; }
        [Display(Name = "MEA Number")]
        public int meaNum { get; set; }
        [Display(Name = "MEA Area")]
        public float meaArea { get; set; }
        [ForeignKey("sStatus")]
        public int status { get; set; }
        public StatusType sStatus { get; set; }

    }

    public class StackPhaseSettingNew
    {
        public int id { get; set; }
        [ForeignKey("stk")]
        public string stackID { get; set; }
        public Stack stk { get; set; }
        [Display(Name ="Step")]
        public int? phaseGroup { get; set; }
        [Display(Name ="Step Loop")]
        public int? phaseLoop { get; set; }
        [Display(Name ="Script")]
        public string SequenceListWithLoop { get; set; }
        [NotMapped]
        public RunStepTemplateGroup? rsg { get; set; }

    }
    public class StackPhaseSetting
    {
        public int id { get; set; }
        [ForeignKey("stk")]
        public string stackID { get; set; }
        public Stack stk { get; set; }
        [Display(Name ="Step")]
        public int phase { get; set; }
        public int? phaseGroup { get; set; }
        [Display(Name ="Phase Loop")]
        public int? phaseGroupLoop { get; set; } = null;
        [ForeignKey("rsg"), Display(Name = "Sequence Name")]
        public int rsgid { get; set; }
        public RunStepTemplateGroup rsg { get; set; }
        public int loop { get; set; }
        public float? hrs { get; set; }
        [Display(Name ="Total Hours")]
        public float? totalHours { get; set; }
        public float? mins { get; set; }
        [Display(Name ="Total minutes")]
        public float? totalMins { get; set; }
        public float? seconds { get; set; }
        public string hrsminsdisplay { get; set; }
        public string totalhrsminsdisplay { get; set; }
        public bool check { get; set; } = false;
    }
    public class EquipmentConfiguration
    {
        [Key, Display(Name ="Test Stand Config ID")]
        public Guid equipmentConfigID { get; set; }
        public string configName { get; set; }
        [Display(Name ="Test Stand Config")]
        public string equipmentConfiguration { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public string colorConfig { get; set; }
    }

    public class StackConfig
    {
        [Key]
        public Guid configID { get; set; }
        public string configName { get; set; }
        public string configString { get; set; }
        public string colorConfig { get; set; }
    }

    public class DeviceData
    {
        [Key]
        public Guid transID { get; set; }
        [Required]
        [ForeignKey("device"), Display(Name ="Test Stand")]
        public string deviceID { get; set; }
        public Device device { get; set; }
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        [NotMapped]
        public string strTime { get; set; }
        [Required]
        public string siteID { get; set; }
        //[ForeignKey("eqConfig")]
        public Guid configID { get; set; }
        //public EquipmentConfiguration eqConfig { get; set; }
        public string ver { get; set; }
        public int status { get; set; }
        public float? wL { get; set; }
        public float? wP { get; set; }
        public float? wC { get; set; }
        public float? wT { get; set; }
        public float? hxiT { get; set; }
        public float? hxoT { get; set; }
        public float? wPp { get; set; }
        public float? HYS { get; set; }
        public string verM { get; set; }
        public Int16? CommStatus { get; set; }
        [NotMapped]
        public TSStackData S01 { get; set; }
        [NotMapped]
        public TSStackData S02 { get; set; }
        [NotMapped]
        public TSStackData S03 { get; set; }
        [NotMapped]
        public TSStackData S04 { get; set; }
        [NotMapped]
        public TSStackData S05 { get; set; }
        [NotMapped]
        public TSStackData S06 { get; set; }
    }

    public class MTSDeviceData
    {
        [Key, Display(Name ="Test Stand")]
        //[ForeignKey("device")]
        public string deviceID { get; set; }
        //public Device device { get; set; }
        [Key]
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        [NotMapped]
        public string strTime { get; set; }
        [Required]
        public string siteID { get; set; }
        //[ForeignKey("eqConfig")]
        public Guid configID { get; set; }
        //public EquipmentConfiguration eqConfig { get; set; }
        public string ver { get; set; }
        public int status { get; set; }
        public float? wL { get; set; }
        public float? wP { get; set; }
        public float? wC { get; set; }
        public float? wT { get; set; }
        [Display(Name ="HRT")]
        public float? hxiT { get; set; }
        public float? hxoT { get; set; }
        public float? HYS { get; set; }
        public float? wPp { get; set; }
        public string verM { get; set; }
        public Int16? CommStatus { get; set; }
        [NotMapped]
        public MTSStackData S01 { get; set; }
        [NotMapped]
        public MTSStackData S02 { get; set; }
        [NotMapped]
        public MTSStackData S03 { get; set; }
        [NotMapped]
        public MTSStackData S04 { get; set; }
        [NotMapped]
        public MTSStackData S05 { get; set; }
        [NotMapped]
        public MTSStackData S06 { get; set; }
    }

    public class MTSDeviceDataNew
    {
        [Key, Display(Name = "Test Stand")]
        //[ForeignKey("device")]
        public string deviceID { get; set; }
        //public Device device { get; set; }
        [Key]
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        [NotMapped]
        public string strTime { get; set; }
        [Required]
        public string siteID { get; set; }
        //[ForeignKey("eqConfig")]
        public Guid configID { get; set; }
        //public EquipmentConfiguration eqConfig { get; set; }
        public string ver { get; set; }
        public int status { get; set; }
        public float? wL { get; set; }
        public float? wP { get; set; }
        public float? wC { get; set; }
        public float? wT { get; set; }
        [Display(Name = "HRT")]
        public float? hxiT { get; set; }
        public float? hxoT { get; set; }
        public float? HYS { get; set; }
        public float? wPp { get; set; }
        public string verM { get; set; }
        public Int16? CommStatus { get; set; }
        [NotMapped]
        public MTSStackDataNew S01 { get; set; }
        [NotMapped]
        public MTSStackDataNew S02 { get; set; }
        [NotMapped]
        public MTSStackDataNew S03 { get; set; }
        [NotMapped]
        public MTSStackDataNew S04 { get; set; }
        [NotMapped]
        public MTSStackDataNew S05 { get; set; }
        [NotMapped]
        public MTSStackDataNew S06 { get; set; }
    }

    public class MTSDeviceDataNew2
    {
        [Key, Display(Name = "Test Stand")]
        //[ForeignKey("device")]
        public string deviceID { get; set; }
        //public Device device { get; set; }
        [Key]
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        [NotMapped]
        public string strTime { get; set; }
        [Required]
        public string siteID { get; set; }
        //[ForeignKey("eqConfig")]
        public Guid configID { get; set; }
        //public EquipmentConfiguration eqConfig { get; set; }
        public string ver { get; set; }
        public int status { get; set; }
        public float? wL { get; set; }
        public float? wP { get; set; }
        public float? wC { get; set; }
        public float? wT { get; set; }
        [Display(Name = "HRT")]
        public float? hxiT { get; set; }
        public float? hxoT { get; set; }
        public float? HYS { get; set; }
        public float? wPp { get; set; }
        public string verM { get; set; }
        public Int16? CommStatus { get; set; }
        [NotMapped]
        public MTSStackDataNew2 S01 { get; set; }
        [NotMapped]
        public MTSStackDataNew2 S02 { get; set; }
        [NotMapped]
        public MTSStackDataNew2 S03 { get; set; }
        [NotMapped]
        public MTSStackDataNew2 S04 { get; set; }
        [NotMapped]
        public MTSStackDataNew2 S05 { get; set; }
        [NotMapped]
        public MTSStackDataNew2 S06 { get; set; }
    }

    public class TSStackData
    {
        [Key]
        public Guid transID { get; set; }
        //[Required]
        [ForeignKey("device"),Display(Name ="Test Stand")]
        public string deviceID { get; set; }
        public Device device { get; set; }
        //[NotMapped]
        public string position { get; set; }
        [ForeignKey("smfgid")]
        //[Required]
        public string stackMfgID { get; set; }
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        public Stack smfgid { get; set; }
        public float? wF { get; set; }
        public float? wT { get; set; }
        public float? wP { get; set; }
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
        public float? imF { get; set; }
        public float? imA { get; set; }
        public float? status { get; set; }
    }

    public class StackTestRunHours
    {
        public int id { get; set; }
        public string stkMfgId { get; set; }
        public DateTime timeStampUTC { get; set; }
        public float? cumulativeHours { get; set; }
    }

    public class MinMaxParams
    {
        public int id { get; set; }
        public string equipment { get; set; }
        public string sensorName { get; set; }
        public float min { get; set; }
        public float max { get; set; }

    }
    public class MTSStackData
    {
        //[Required]
        [Key, Display(Name ="Test Stand")]
        //[ForeignKey("device")]
        
        public string deviceID { get; set; }
        //public Device device { get; set; }
        //[NotMapped]
        public string position { get; set; }
        [Key]
        //[ForeignKey("smfgid")]
        //[Required]
        public string stackMfgID { get; set; }
        [Key]
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        //public Stack smfgid { get; set; }
        public float? wF { get; set; }
        public float? wT { get; set; }
        public float? wP { get; set; }
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
        //[Display(Name = "Total Run Hours")]
        public float? cumulativeHours { get; set; } = 0.0F;
        public int? stepNumber { get; set; } = 0;
        public int? loopcnt { get; set; } = 0;
        public float? cM1 { get; set; }
        public float? cM2 { get; set; }
        public float? cM3 { get; set; }
        public float? cM4 { get; set; }
        public float? cM5 { get; set; }
        public string seqName { get; set; }
        public float? imF { get; set; }
        public float? imA { get; set; }
        public int? status { get; set; }
        public string interLock { get; set; } = "";
        public float? scriptLoopCnt { get; set; } = 0;
        public float? seqLoopCnt { get; set; } = 0;
        public float? scriptStep { get; set; } = 0;
        public float? seqStep { get; set; } = 0;
    }

    public class MTSStackDataNew
    {
        //[Required]
        [Key, Display(Name = "Test Stand")]
        //[ForeignKey("device")]

        public string deviceID { get; set; }
        //public Device device { get; set; }
        //[NotMapped]
        public string position { get; set; }
        [Key]
        //[ForeignKey("smfgid")]
        //[Required]
        public string stackMfgID { get; set; }
        [Key]
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        //public Stack smfgid { get; set; }
        public float? wF { get; set; }
        public float? wT { get; set; }
        public float? wP { get; set; }
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
        //[Display(Name = "Total Run Hours")]
        public float? cumulativeHours { get; set; } = 0.0F;
        public int? stepNumber { get; set; } = 0;
        public int? loopcnt { get; set; } = 0;
        public float? cM1 { get; set; }
        public float? cM2 { get; set; }
        public float? cM3 { get; set; }
        public float? cM4 { get; set; }
        public float? cM5 { get; set; }
        public string seqName { get; set; }
        public float? imF { get; set; }
        public float? imA { get; set; }
        public int? status { get; set; }
        public string interLock { get; set; } = "";
        public float? scriptLoopCnt { get; set; } = 0;
        public float? seqLoopCnt { get; set; } = 0;
        public float? scriptStep { get; set; } = 0;
        public float? seqStep { get; set; } = 0;
    }

    public class MTSStackDataNew2
    {
        //[Required]
        [Key, Display(Name = "Test Stand")]
        //[ForeignKey("device")]

        public string deviceID { get; set; }
        //public Device device { get; set; }
        //[NotMapped]
        public string position { get; set; }
        [Key]
        //[ForeignKey("smfgid")]
        //[Required]
        public string stackMfgID { get; set; }
        [Key]
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        //public Stack smfgid { get; set; }
        public float? wF { get; set; }
        public float? wT { get; set; }
        public float? wP { get; set; }
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
        //[Display(Name = "Total Run Hours")]
        public float? cumulativeHours { get; set; } = 0.0F;
        public int? stepNumber { get; set; } = 0;
        public int? loopcnt { get; set; } = 0;
        public float? cM1 { get; set; }
        public float? cM2 { get; set; }
        public float? cM3 { get; set; }
        public float? cM4 { get; set; }
        public float? cM5 { get; set; }
        public string seqName { get; set; }
        public float? imF { get; set; }
        public float? imA { get; set; }
        public int? status { get; set; }
        public string interLock { get; set; } = "";
        public float? scriptLoopCnt { get; set; } = 0;
        public float? seqLoopCnt { get; set; } = 0;
        public float? scriptStep { get; set; } = 0;
        public float? seqStep { get; set; } = 0;
    }
    public class RunProfile
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Run Profile")]
        public string profileName { get; set; }//datalist - pick from existing profiles or type a new one
        [ForeignKey("device"), Display(Name ="Test Stand")]
        public string deviceID { get; set; }
        public Device device { get; set; }
        [Display(Name = "Test Stand Command")]
        public string dCmd { get; set; }
        public DateTime timeStamp { get; set; }
        public bool? fan { get; set; } = true;
        public bool? pump { get; set; }
        //public float? mnHxT { get; set; } = 0;
        //public float? mxHxT { get; set; } = 0;
        //public float? wbT { get; set; } = 0;
        public float? mnWbT { get; set; } = 0;
        public float? mxWbT { get; set; } = 0;
        public float? hxT { get; set; } = 0;
        public List<StackRunProfile> stackRunProfile { get; set; }
    }

    public class RunProfileTemplate
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Run Profile")]
        public string profileName { get; set; }//datalist - pick from existing profiles or type a new one
        [Display(Name = "Test Stand Command")]
        public string dCmd { get; set; }
        public bool? fan { get; set; }
        public bool? pump { get; set; }
        public float? mnHxT { get; set; } = 0; //obsolete
        public float? mxHxT { get; set; } = 0; //obsolete
        public float? wbT { get; set; } = 0; //obsolete
        public float? mnWbT { get; set; } = 0;
        public float? mxWbT { get; set; } = 0;
        public float? hxT { get; set; } = 0;

        public DateTime timeStamp { get; set; }
        public List<StackRunProfileTemplate> stackRunProfileTemplate { get; set; }
       [ForeignKey("stt")]
        public int? status { get; set; }
        public StatusType stt { get; set; }
    }

    public class StackRunProfile
    {
        public int id { get; set; }
        [Display(Name = "Stk Run Profile")]
        public string name { get; set; }
        [ForeignKey("profile"), Display(Name = "Run Profile")]
        public int profileID { get; set; }
        public RunProfile profile { get; set; }
        [ForeignKey("stk")]
        public string stackID { get; set; }
        public Stack stk { get; set; }
        public string stackPosition { get; set; }
        public bool loop { get; set; }
        [Display(Name = "Stack Command")]
        public string command { get; set; }
        public List<RunStep> runStep { get; set; }
    }

    public class StackRunProfileTemplate
    {
        public int id { get; set; }
        [Display(Name = "Stk Run Profile")]
        public string name { get; set; }
        [ForeignKey("profile"), Display(Name = "Run Profile")]
        public int profileID { get; set; }
        public RunProfileTemplate profile { get; set; }
        public bool loop { get; set; }
        [Display(Name = "Stack Command")]
        public string command { get; set; }
        public List<RunStepTemplate> runStepTemplate { get; set; }
        [ForeignKey("stt")]
        public int? status { get; set; }
        public StatusType stt { get; set; }
    }

    public class RunStep
    {
        //set values and thresholds
        public int id { get; set; }
        [ForeignKey("srp")]
        public int stkRunProfileID { get; set; }
        public StackRunProfile srp { get; set; }
        [Display(Name = "test State")]
        //[ForeignKey("tState")]
        public int? testState { get; set; } = 0;
        //public TestState tState { get; set; }
        [Display(Name = "Step NO")]
        public int stepNumber { get; set; }
        [Display(Name = "Step Command")]
        public string sCmd { get; set; }
        [Display(Name = "Duration")]
        public int duration { get; set; }
        public float? cI { get; set; } //Current set point
        public float? cV { get; set; } //Voltage set point
        public float? wP { get; set; } //Water back pressure set point
        public float? hP { get; set; } //Hydrogen back pressure set point
        public float? wFt { get; set; } //Water flow threshold
        public float? wTt { get; set; } //Water out temperature threshold
        public float? cVt { get; set; } //Cell voltage threshold
        public float? cVl { get; set; }
        public float? mnF { get; set; } = 0; //minimum frequency for sweep
        public float? mxF { get; set; } = 0;//maximum frequency for sweep
        [ForeignKey("rstg")]
        public int? rstGID { get; set; }
        public RunStepTemplateGroup rstg { get; set; }
        public float? imF { get; set; }
        public float? imA { get; set; }
    }

    public class RunStepTemplate
    {
        //set values and thresholds
        public int id { get; set; }
        [ForeignKey("srp"),Display(Name ="Stack Run Profile")]
        public int stkRunProfileID { get; set; }
        public StackRunProfileTemplate srp { get; set; }
        [Display(Name = "test State")]
        [ForeignKey("tState")]
        public int testState { get; set; } = 1;
        public TestState tState { get; set; }
        [Display(Name = "Step NO")]
        public int stepNumber { get; set; }
        [Display(Name = "Step Command")]
        public string sCmd { get; set; }
        [Display(Name = "Duration")]
        public int duration { get; set; }
        public float? cI { get; set; } //Current set point
        public float? cV { get; set; } //Voltage set point
        public float? wP { get; set; } //Water back pressure set point
        public float? hP { get; set; } //Hydrogen back pressure set point
        public float? wFt { get; set; } //Water flow threshold
        public float? wTt { get; set; } //Water out temperature threshold
        public float? cVt { get; set; } //Cell voltage threshold
        public float? cVl { get; set; } // Cell Voltage Limit
        public float? mnF { get; set; } = 0; //minimum frequency for sweep
        public float? mxF { get; set; } = 0;//maximum frequency for sweep
        [ForeignKey("rstg")]
        public int? rstGID { get; set; }
        public RunStepTemplateGroup rstg { get; set; }
        [ForeignKey("stt")]
        public int? status { get; set; }
        public StatusType stt { get; set; }
        public float? imF { get; set; }
        public float? imA { get; set; }

    }

    public class RunStepTemplateGroup
    {
        public int id { get; set; }
        public string name { get; set; }
        public int numLoops { get; set; }
        [ForeignKey("stype")]
        public int? status { get; set; }=0;
        public StatusType? stype { get; set; }
        [NotMapped]
        public bool check { get; set; }
        [NotMapped]
        public int sequence { get; set; }

    }

    public class DeviceTemplateAllocation
    {
        public int id { get; set; }
        [ForeignKey("device"),Display(Name ="Test Stand")]
        public string deviceID { get; set; }
        public Device device { get; set; }
        [ForeignKey("rtp")]
        public int templateID { get; set; }
        public RunProfileTemplate rpt { get; set; }
        [ForeignKey("stack")]
        public string stackID { get; set; }
        public Stack stack { get; set; }
        [ForeignKey("srpt")]
        public int stackRunProfileTemplateID { get; set; }
        public StackRunProfileTemplate srpt { get; set; }
    }

    public class TestProfileConfig
    {
        [Key]
        public int id { get; set; }
        public string Config { get; set; }
    }

    #region // Start region to comment old lotus Models
    /// <summary>
    /// System(Device) with all the serial numbers for cabinets
    /// </summary>
    public class SYS
    {
        [Key]
        public string scSn { get; set; } = "LTMK1CKB0000001";
        public string scHn { get; set; } = "LTCSCHW000001";
        public string scFn { get; set; } = "LTCSCFW000001";
        public int nLCC { get; set; } = 1;
        public int nStack { get; set; } = 4;
        public string dSn { get; set; } = "LTMK1CKB0000001";
        public string lhc1Sn { get; set; } = "LHC10000001";
        public string lpc1Sn { get; set; } = "LPC10000001";
        public string lhc2Sn { get; set; } = "LHC10000002";
        public string lpc2Sn { get; set; } = "LPC10000002";
        public string lhc3Sn { get; set; } = "LHC10000003";
        public string lpc3Sn { get; set; } = "LPC10000003";
        public string lhc4Sn { get; set; } = "LHC10000004";
        public string lpc4Sn { get; set; } = "LPC10000004";
        public string lwcSn { get; set; } = "LWC00000001";
        public string ltcSn { get; set; } = "LTC00000001";
        public string compSn { get; set; } = "EDGE0000001";
        public string dryerSn { get; set; } = "Dryer00000001";
        public string uc1Sn { get; set; } = "UC100000001";
        public string uc1Hn { get; set; } = "UC1HW00000001";
        public string uc1Fn { get; set; } = "UC1SW00000001";
        public string uc2Sn { get; set; } = "UC200000001";
        public string uc2Hn { get; set; } = "UC2HW00000001";
        public string uc2Fn { get; set; } = "UC2SW00000001";
        public string uc3Sn { get; set; } = "UC300000001";
        public string uc3Hn { get; set; } = "UC3HW00000001";
        public string uc3Fn { get; set; } = "UC3SW00000001";
        public string uc4Sn { get; set; } = "UC400000001";
        public string uc4Hn { get; set; } = "UC4HW00000001";
        public string uc4Fn { get; set; } = "UC4SW00000001";

    }
    public class LotusDeviceData
    {
        [Key]
        public Guid transID { get; set; }
        [Required]
        [ForeignKey("device")]
        public string scSn { get; set; }
        public Device device { get; set; }
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
        [NotMapped]
        public string strTime { get; set; }
        [Required]
        public string siteID { get; set; }
        //[ForeignKey("eqConfig")]
        public Guid configID { get; set; }
        //public EquipmentConfiguration eqConfig { get; set; }
        public string ver { get; set; }
        public int status { get; set; }
        //public float wL { get; set; }
        //public float wP { get; set; }
        //public float wC { get; set; }
        //public float wT { get; set; }
        //public float hxiT { get; set; }
        //public float hxoT { get; set; }
        [NotMapped]
        public LHC LHCC01 { get; set; }
        [NotMapped]
        public LHC LHCC02 { get; set; }
        [NotMapped]
        public LHC LHCC03 { get; set; }
        [NotMapped]
        public LHC LHCC04 { get; set; }

        [NotMapped]
        public LPC LPCC01 { get; set; }

        [NotMapped]
        public LPC LPCC02 { get; set; }
        [NotMapped]
        public LPC LPCC03 { get; set; }

        [NotMapped]
        public LPC LPCC04 { get; set; }

        [NotMapped]
        public LCC LCCC01 { get; set; }
        [NotMapped]
        public LCC LCCC02 { get; set; }
        [NotMapped]
        public LWC lwc { get; set; }
        [NotMapped]
        public LTC ltc { get; set; }

    }
    public class LHC

    {
        public Guid id { get; set; }
        public string scSn { get; set; }
        public string hcSN { get; set; }
        public float maxCV0 { get; set; }
        public float maxCV1 { get; set; }
        public float maxCV2 { get; set; }
        public float maxCV3 { get; set; }
        public float maxCV4 { get; set; }
        public float maxCV5 { get; set; }
        public float maxCV6 { get; set; }
        public float maxCV7 { get; set; }
        public float maxCV8 { get; set; }
        public float maxCV9 { get; set; }
        public float minCV0 { get; set; }
        public float minCV1 { get; set; }
        public float minCV2 { get; set; }
        public float minCV3 { get; set; }
        public float minCV4 { get; set; }
        public float minCV5 { get; set; }
        public float minCV6 { get; set; }
        public float minCV7 { get; set; }
        public float minCV8 { get; set; }
        public float minCV9 { get; set; }
        public float pws101 { get; set; }
        public float pr101 { get; set; }
        public float lvl101 { get; set; }
        public float lvl102 { get; set; }
        public float ttc101 { get; set; }
        public float ttc102 { get; set; }
        public float pr102 { get; set; }
        public float ebc801 { get; set; }
        public float cos101 { get; set; }
        public float prt103 { get; set; }
        public float prt401 { get; set; }
        public float ktc401 { get; set; }
        public float ktc402 { get; set; }
        public float hsv101 { get; set; }
        public float hsv402 { get; set; }

        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
    }

    public class LPC
    {
        public Guid id { get; set; }
        public string scSn { get; set; }
        public string pcSN { get; set; }
        public float tDcA { get; set; }
        public float tAcA { get; set; }
        public float tAcV { get; set; }
        public float mcState { get; set; }
        public float psu1V { get; set; }
        public float psu2V { get; set; }
        public float psu3V { get; set; }
        public float psu4V { get; set; }
        public float psu5V { get; set; }
        public float psu6V { get; set; }
        public float psu7V { get; set; }
        public float psu8V { get; set; }
        public float psu9V { get; set; }
        public float psu10V { get; set; }
        public float psu11V { get; set; }
        public float psu12V { get; set; }
        public float psu13V { get; set; }
        public float psu14V { get; set; }
        public float psu15V { get; set; }
        public float psu16V { get; set; }
        public float psu17V { get; set; }
        public float psu18V { get; set; }
        public float psu19V { get; set; }
        public float psu20V { get; set; }
        public float psu21V { get; set; }
        public float psu22V { get; set; }
        public float psu23V { get; set; }
        public float psu24V { get; set; }
        public float psu25V { get; set; }
        public float psu26V { get; set; }
        public float psu27V { get; set; }
        public float psu28V { get; set; }
        public float psu1A { get; set; }
        public float psu2A { get; set; }
        public float psu3A { get; set; }
        public float psu4A { get; set; }
        public float psu5A { get; set; }
        public float psu6A { get; set; }
        public float psu7A { get; set; }
        public float psu8A { get; set; }
        public float psu9A { get; set; }
        public float psu10A { get; set; }
        public float psu11A { get; set; }
        public float psu12A { get; set; }
        public float psu13A { get; set; }
        public float psu14A { get; set; }
        public float psu15A { get; set; }
        public float psu16A { get; set; }
        public float psu17A { get; set; }
        public float psu18A { get; set; }
        public float psu19A { get; set; }
        public float psu20A { get; set; }
        public float psu21A { get; set; }
        public float psu22A { get; set; }
        public float psu23A { get; set; }
        public float psu24A { get; set; }
        public float psu25A { get; set; }
        public float psu26A { get; set; }
        public float psu27A { get; set; }
        public float psu28A { get; set; }
        public float psu1fecf { get; set; }
        public float psu2fecf { get; set; }
        public float psu3fecf { get; set; }
        public float psu4fecf { get; set; }
        public float psu5fecf { get; set; }
        public float psu6fecf { get; set; }
        public float psu7fecf { get; set; }
        public float psu8fecf { get; set; }
        public float psu9fecf { get; set; }
        public float psu10fecf { get; set; }
        public float psu11fecf { get; set; }
        public float psu12fecf { get; set; }
        public float psu13fecf { get; set; }
        public float psu14fecf { get; set; }
        public float psu15fecf { get; set; }
        public float psu16fecf { get; set; }
        public float psu17fecf { get; set; }
        public float psu18fecf { get; set; }
        public float psu19fecf { get; set; }
        public float psu20fecf { get; set; }
        public float psu21fecf { get; set; }
        public float psu22fecf { get; set; }
        public float psu23fecf { get; set; }
        public float psu24fecf { get; set; }
        public float psu25fecf { get; set; }
        public float psu26fecf { get; set; }
        public float psu27fecf { get; set; }
        public float psu28fecf { get; set; }
        public float psu1dcf { get; set; }
        public float psu2dcf { get; set; }
        public float psu3dcf { get; set; }
        public float psu4dcf { get; set; }
        public float psu5dcf { get; set; }
        public float psu6dcf { get; set; }
        public float psu7dcf { get; set; }
        public float psu8dcf { get; set; }
        public float psu9dcf { get; set; }
        public float psu10dcf { get; set; }
        public float psu11dcf { get; set; }
        public float psu12dcf { get; set; }
        public float psu13dcf { get; set; }
        public float psu14dcf { get; set; }
        public float psu15dcf { get; set; }
        public float psu16dcf { get; set; }
        public float psu17dcf { get; set; }
        public float psu18dcf { get; set; }
        public float psu19dcf { get; set; }
        public float psu20dcf { get; set; }
        public float psu21dcf { get; set; }
        public float psu22dcf { get; set; }
        public float psu23dcf { get; set; }
        public float psu24dcf { get; set; }
        public float psu25dcf { get; set; }
        public float psu26dcf { get; set; }
        public float psu27dcf { get; set; }
        public float psu28dcf { get; set; }
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
    }

    public class LCC
    {
        public Guid id { get; set; }
        public string lcc1SN { get; set; }
        public float lvl901 { get; set; }
        public float ktc901 { get; set; }
        public float pmp901 { get; set; }
        public float pmp902 { get; set; }
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
    }

    public class LWC
    {
        public Guid id { get; set; }
        public string lwcSN { get; set; }
        public float cos701 { get; set; }
        public float cos702 { get; set; }
        public float cos703 { get; set; }
        public float prt701 { get; set; }
        public float prt702 { get; set; }
        public float prt703 { get; set; }
        public float lvl701 { get; set; }
        public float lvl702 { get; set; }
        public float lvl703 { get; set; }
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
    }
    public class LTC
    {
        public Guid id { get; set; }
        public string ltcSN { get; set; }
        public float upsInV { get; set; }
        public float upsInA { get; set; }
        public float upsSoc { get; set; }
        public float upsWar { get; set; }
        public float upsOV { get; set; }
        public DateTime timeStamp { get; set; } = DateTime.UtcNow;
    }

    //Lotus Models
    //public class ModulSYS
    //{
    //    public int id { get; set; }
    //    [Display(Name = "D_R101")]
    //    public string Param_R101 { get; set; }
    //    public string Param_R102 { get; set; }
    //    public string Param_R103 { get; set; }
    //    public string Param_R104 { get; set; }
    //    public string Param_R105 { get; set; }
    //    public string Param_R106 { get; set; }
    //    public string Param_R107 { get; set; }
    //    public string Param_R108 { get; set; }
    //    public string Param_R109 { get; set; }
    //    public string Param_R110 { get; set; }
    //    public string Param_R111 { get; set; }
    //    public string Param_R112 { get; set; }
    //    public string Param_R113 { get; set; }
    //    public string Param_R114 { get; set; }
    //    public string Param_R115 { get; set; }
    //    public string Param_R116 { get; set; }
    //    public string Param_R117 { get; set; }
    //    public string Param_R118 { get; set; }
    //    public string Param_R119 { get; set; }
    //    public string Param_R120 { get; set; }
    //    public string Param_R121 { get; set; }
    //    public string Param_R122 { get; set; }
    //    public string Param_R123 { get; set; }
    //    public string Param_R124 { get; set; }
    //    public string Param_R125 { get; set; }
    //    public string Param_R126 { get; set; }
    //    public string Param_R127 { get; set; }
    //    public string Param_R128 { get; set; }
    //    public string Param_R129 { get; set; }
    //    public string Param_R130 { get; set; }
    //    public string Param_R131 { get; set; }
    //    public string Param_R132 { get; set; }
    //    public string Param_R133 { get; set; }
    //    public string Param_R134 { get; set; }
    //    public string Param_R135 { get; set; }

    //}
    //public class ModulLPC
    //{
    //    public int id { get; set; }
    //    public string Param_R301 { get; set; }
    //    public string Param_R302 { get; set; }
    //    public string Param_R303 { get; set; }
    //    public string Param_R304 { get; set; }
    //    public string Param_R305 { get; set; }
    //    public string Param_R306 { get; set; }
    //    public string Param_R307 { get; set; }
    //    public string Param_R308 { get; set; }
    //    public string Param_R309 { get; set; }
    //    public string Param_R310 { get; set; }
    //    public string Param_R311 { get; set; }
    //    public string Param_R312 { get; set; }
    //    public string Param_R313 { get; set; }
    //    public string Param_R314 { get; set; }
    //    public string Param_R315 { get; set; }
    //    public string Param_R316 { get; set; }
    //    public string Param_R317 { get; set; }
    //    public string Param_R318 { get; set; }
    //    public string Param_R319 { get; set; }
    //    public string Param_R320 { get; set; }
    //    public string Param_R321 { get; set; }
    //    public string Param_R322 { get; set; }
    //    public string Param_R323 { get; set; }
    //    public string Param_R324 { get; set; }
    //    public string Param_R325 { get; set; }
    //    public string Param_R326 { get; set; }
    //    public string Param_R327 { get; set; }
    //    public string Param_R328 { get; set; }
    //    public string Param_R329 { get; set; }
    //    public string Param_R330 { get; set; }
    //    public string Param_R331 { get; set; }
    //    public string Param_R332 { get; set; }
    //    public string Param_R333 { get; set; }
    //    public string Param_R334 { get; set; }
    //    public string Param_R335 { get; set; }
    //    public string Param_R336 { get; set; }
    //    public string Param_R337 { get; set; }
    //    public string Param_R338 { get; set; }
    //    public string Param_R339 { get; set; }
    //    public string Param_R340 { get; set; }
    //    public string Param_R341 { get; set; }
    //    public string Param_R342 { get; set; }
    //    public string Param_R343 { get; set; }
    //    public string Param_R344 { get; set; }
    //    public string Param_R345 { get; set; }
    //    public string Param_R346 { get; set; }
    //    public string Param_R347 { get; set; }
    //    public string Param_R348 { get; set; }
    //    public string Param_R349 { get; set; }
    //    public string Param_R350 { get; set; }
    //    public string Param_R351 { get; set; }
    //    public string Param_R352 { get; set; }
    //    public string Param_R353 { get; set; }
    //    public string Param_R354 { get; set; }
    //    public string Param_R355 { get; set; }
    //    public string Param_R356 { get; set; }
    //    public string Param_R357 { get; set; }
    //    public string Param_R358 { get; set; }
    //    public string Param_R359 { get; set; }
    //    public string Param_R360 { get; set; }
    //    public string Param_R361 { get; set; }
    //    public string Param_R362 { get; set; }
    //    public string Param_R363 { get; set; }
    //    public string Param_R364 { get; set; }
    //    public string Param_R365 { get; set; }
    //    public string Param_R366 { get; set; }
    //    public string Param_R367 { get; set; }
    //    public string Param_R368 { get; set; }
    //    public string Param_R369 { get; set; }
    //    public string Param_R370 { get; set; }
    //    public string Param_R371 { get; set; }
    //    public string Param_R372 { get; set; }
    //    public string Param_R373 { get; set; }
    //    public string Param_R374 { get; set; }
    //    public string Param_R375 { get; set; }
    //    public string Param_R376 { get; set; }
    //    public string Param_R377 { get; set; }
    //    public string Param_R378 { get; set; }
    //    public string Param_R379 { get; set; }
    //    public string Param_R380 { get; set; }
    //    public string Param_R381 { get; set; }
    //    public string Param_R382 { get; set; }
    //    public string Param_R383 { get; set; }
    //    public string Param_R384 { get; set; }
    //    public string Param_R385 { get; set; }
    //    public string Param_R386 { get; set; }
    //    public string Param_R387 { get; set; }
    //    public string Param_R388 { get; set; }
    //    public string Param_R389 { get; set; }
    //    public string Param_R390 { get; set; }
    //    public string Param_R391 { get; set; }
    //    public string Param_R392 { get; set; }
    //    public string Param_R393 { get; set; }
    //    public string Param_R394 { get; set; }
    //    public string Param_R395 { get; set; }
    //    public string Param_R396 { get; set; }
    //    public string Param_R397 { get; set; }
    //    public string Param_R398 { get; set; }
    //    public string Param_R399 { get; set; }
    //    public string Param_R400 { get; set; }
    //    public string Param_R401 { get; set; }
    //    public string Param_R402 { get; set; }
    //    public string Param_R403 { get; set; }
    //    public string Param_R404 { get; set; }
    //    public string Param_R405 { get; set; }
    //    public string Param_R406 { get; set; }
    //    public string Param_R407 { get; set; }
    //    public string Param_R408 { get; set; }
    //    public string Param_R409 { get; set; }
    //    public string Param_R410 { get; set; }
    //    public string Param_R411 { get; set; }
    //    public string Param_R412 { get; set; }
    //    public string Param_R413 { get; set; }
    //    public string Param_R414 { get; set; }
    //    public string Param_R415 { get; set; }
    //    public string Param_R416 { get; set; }
    //    public string Param_R417 { get; set; }
    //    public string Param_R418 { get; set; }
    //    public string Param_R419 { get; set; }
    //    public string Param_R420 { get; set; }
    //    public string Param_R421 { get; set; }
    //    public string Param_R422 { get; set; }
    //    public string Param_R423 { get; set; }
    //    public string Param_R424 { get; set; }
    //    public string Param_R425 { get; set; }
    //    public string Param_R426 { get; set; }
    //    public string Param_R427 { get; set; }
    //    public string Param_R428 { get; set; }
    //    public string Param_R429 { get; set; }
    //    public string Param_R430 { get; set; }
    //    public string Param_R431 { get; set; }
    //    public string Param_R432 { get; set; }
    //    public string Param_R433 { get; set; }
    //    public string Param_R434 { get; set; }
    //    public string Param_R435 { get; set; }
    //    public string Param_R436 { get; set; }
    //    public string Param_R437 { get; set; }
    //    public string Param_R438 { get; set; }
    //    public string Param_R439 { get; set; }
    //    public string Param_R440 { get; set; }

    //}//power
    //public class ModulLHC
    //{
    //    public int id { get; set; }
    //    public string Param_R501 { get; set; }
    //    public string Param_R502 { get; set; }
    //    public string Param_R503 { get; set; }
    //    public string Param_R504 { get; set; }
    //    public string Param_R505 { get; set; }
    //    public string Param_R506 { get; set; }
    //    public string Param_R507 { get; set; }
    //    public string Param_R508 { get; set; }
    //    public string Param_R509 { get; set; }
    //    public string Param_R510 { get; set; }
    //    public string Param_R511 { get; set; }
    //    public string Param_R512 { get; set; }
    //    public string Param_R513 { get; set; }
    //    public string Param_R514 { get; set; }
    //    public string Param_R515 { get; set; }
    //    public string Param_R516 { get; set; }
    //    public string Param_R517 { get; set; }
    //    public string Param_R518 { get; set; }
    //    public string Param_R519 { get; set; }
    //    public string Param_R520 { get; set; }
    //    public string Param_R521 { get; set; }
    //    public string Param_R522 { get; set; }
    //    public string Param_R523 { get; set; }
    //    public string Param_R524 { get; set; }
    //    public string Param_R525 { get; set; }
    //    public string Param_R526 { get; set; }
    //    public string Param_R527 { get; set; }
    //    public string Param_R528 { get; set; }
    //    public string Param_R529 { get; set; }
    //    public string Param_R530 { get; set; }
    //    public string Param_R531 { get; set; }
    //    public string Param_R532 { get; set; }
    //    public string Param_R533 { get; set; }
    //    public string Param_R534 { get; set; }
    //    public string Param_R535 { get; set; }
    //    public string Param_R536 { get; set; }
    //    public string Param_R537 { get; set; }
    //    public string Param_R538 { get; set; }
    //    public string Param_R539 { get; set; }
    //    public string Param_R540 { get; set; }
    //} //hydrogen
    //public class ModulLCC
    //{
    //    public int id { get; set; }
    //    public string Param_R801 { get; set; }
    //    public string Param_R802 { get; set; }
    //    public string Param_R803 { get; set; }
    //    public string Param_R804 { get; set; }
    //    public string Param_R805 { get; set; }
    //    public string Param_R806 { get; set; }
    //    public string Param_R807 { get; set; }
    //    public string Param_R808 { get; set; }
    //    public string Param_R809 { get; set; }
    //    public string Param_R810 { get; set; }
    //} //cooling
    //public class ModulLWC
    //{
    //    public int id { get; set; }
    //    public string Param_R851 { get; set; }
    //    public string Param_R852 { get; set; }
    //    public string Param_R853 { get; set; }
    //    public string Param_R854 { get; set; }
    //    public string Param_R855 { get; set; }
    //    public string Param_R856 { get; set; }
    //    public string Param_R857 { get; set; }
    //    public string Param_R858 { get; set; }
    //    public string Param_R859 { get; set; }
    //    public string Param_R860 { get; set; }
    //    public string Param_R861 { get; set; }
    //    public string Param_R862 { get; set; }
    //    public string Param_R863 { get; set; }
    //    public string Param_R864 { get; set; }
    //    public string Param_R865 { get; set; }
    //    public string Param_R866 { get; set; }
    //    public string Param_R867 { get; set; }
    //    public string Param_R868 { get; set; }
    //    public string Param_R869 { get; set; }
    //    public string Param_R870 { get; set; }
    //    public string Param_R871 { get; set; }
    //}//water
    //public class LTC//telemetry
    //{
    //    public int id { get; set; }
    //    public string Param_R901 { get; set; }
    //    public string Param_R902 { get; set; }
    //    public string Param_R903 { get; set; }
    //    public string Param_R904 { get; set; }
    //    public string Param_R905 { get; set; }
    //} 
    //public class ModulCTR
    //{
    //    public int id { get; set; }
    //    public string Param_W101 { get; set; }
    //    public string Param_W102 { get; set; }
    //    public string Param_W103 { get; set; }
    //    public string Param_W104 { get; set; }
    //    public string Param_W105 { get; set; }
    //    public string Param_W106 { get; set; }
    //    public string Param_W107 { get; set; }
    //    public string Param_W108 { get; set; }
    //    public string Param_W109 { get; set; }
    //    public string Param_W110 { get; set; }
    //    public string Param_W111 { get; set; }
    //    public string Param_W112 { get; set; }
    //    public string Param_W113 { get; set; }
    //    public string Param_W114 { get; set; }
    //    public string Param_W115 { get; set; }
    //    public string Param_W116 { get; set; }
    //    public string Param_W117 { get; set; }
    //    public string Param_W118 { get; set; }
    //    public string Param_W119 { get; set; }
    //    public string Param_W201 { get; set; }
    //    public string Param_W202 { get; set; }
    //    public string Param_W203 { get; set; }
    //    public string Param_W204 { get; set; }
    //    public string Param_W205 { get; set; }
    //    public string Param_W206 { get; set; }
    //    public string Param_W207 { get; set; }
    //    public string Param_W208 { get; set; }
    //    public string Param_W209 { get; set; }
    //    public string Param_W210 { get; set; }
    //    public string Param_W211 { get; set; }
    //    public string Param_W212 { get; set; }
    //    public string Param_W213 { get; set; }
    //    public string Param_W214 { get; set; }
    //    public string Param_W215 { get; set; }
    //    public string Param_W216 { get; set; }
    //    public string Param_W217 { get; set; }
    //    public string Param_W218 { get; set; }
    //    public string Param_W219 { get; set; }
    //    public string Param_W220 { get; set; }
    //}
    ////sgc

    //color config

    #endregion // end region to comment old lotus Models

    public class ColorConfig
    {
        public int id { get; set; }
        public string sensorName { get; set; }
        public string colorCode { get; set; }
    }

    public class Feedback
    {
        public int id { get; set; }
        public string user { get; set; }
        public string comment { get; set; }
        public string attachment { get; set; }
        public DateTime dstamp { get; set; }
    }

   
    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string errorText { get; set; }

    }

    public class DeviceAndStack
    {
        public DateTime timeStamp { get; set; }
        [Required]
        [ForeignKey("device")]
        public string deviceID { get; set; }
        public Device? device { get; set; }
        public string siteID { get; set; }
        //[ForeignKey("eqConfig")]
        public Guid configID { get; set; }
        //public EquipmentConfiguration eqConfig { get; set; }
        public string ver { get; set; }
        public int status { get; set; }
        public float? wL { get; set; }
        //public float? wP { get; set; }
        public float? wC { get; set; }
        //public float? wT { get; set; }
        public float? hxiT { get; set; }
        public float? hxoT { get; set; }
        public float? wPp { get; set; }
        public string verM { get; set; }
        public Int16? CommStatus { get; set; }
        public string position { get; set; }
        [ForeignKey("smfgid")]
        //[Required]
        public string stackMfgID { get; set; }
        public Stack? smfgid { get; set; }
        public float? wF { get; set; }
        public float? wT { get; set; }
        public float? wP { get; set; }
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
        public float? imF { get; set; }
        public float? imA { get; set; }
        public string seqName { get; set; }
    }

    //DTO -device and stack
    //public class DeviceAndStackDTO
    //{
    //    public DateTime timeStamp { get; set; }
    //    public string deviceID { get; set; }
    //    public string stackMfgID { get; set; }
    //    public float? wF { get; set; }
    //    public float? wT { get; set; }
    //    public float? wP { get; set; }
    //    public float? hP { get; set; }
    //    public float? hT { get; set; }
    //    public float? psI { get; set; }
    //    public float? psV { get; set; }
    //    public float? cV1 { get; set; }
    //    public float? cV2 { get; set; }
    //    public float? cV3 { get; set; }
    //    public float? cV4 { get; set; }
    //    public float? cV5 { get; set; }
    //    public float? cV6 { get; set; }
    //    public float? isF { get; set; } = 0;
    //    public float? cR1 { get; set; }
    //    public float? cR2 { get; set; }
    //    public float? cR3 { get; set; }
    //    public float? cR4 { get; set; }
    //    public float? cR5 { get; set; }
    //    public float? cR6 { get; set; }
    //    public float? cX1 { get; set; }
    //    public float? cX2 { get; set; }
    //    public float? cX3 { get; set; }
    //    public float? cX4 { get; set; }
    //    public float? cX5 { get; set; }
    //    public float? CD { get; set; }
    //    public string state { get; set; } = "START";
    //    public float? runHours { get; set; } = 0.0F;
    //    public float? cumulativeHours { get; set; }
    //    public int? stepNumber { get; set; } = 0;
    //    public int? loopcnt { get; set; } = 0;
    //    public float? cM1 { get; set; }
    //    public float? cM2 { get; set; }
    //    public float? cM3 { get; set; }
    //    public float? cM4 { get; set; }
    //    public float? cM5 { get; set; }
    //    public float? imA { get; set; }
    //    public float? imF { get; set; }
    //    public string position { get; set; }
    //    public string seqName { get; set; }
    //    public float? scriptLoopCnt { get; set; }
    //    public float? scriptStep { get; set; }
    //    public float? seqLoopCnt { get; set; }
    //    public float? seqStep { get; set; }
    //    public string siteID { get; set; }
    //    public Guid configID { get; set; }
    //    public string ver { get; set; }
    //    public float? status { get; set; }
    //    public string interLock { get; set; }
    //    public float? wL { get; set; }
    //    public float? wP_d { get; set; }
    //    public float? wC { get; set; }
    //    public float? wT_d { get; set; }
    //    public float? hxiT { get; set; }
    //    public float? hxoT { get; set; }
    //    public float? wPp { get; set; }
    //    public float? HYS { get; set; }
    //    public string verM { get; set; }
    //    public Int16? CommStatus { get; set; }
    //}

    #region     // Start - Region for Lotus-Mark1 Device
    public class LHCNew
    {
        public string Name { get; set; }
        public string tagType { get; set; }
        public tagFloat Fan101 { get; set; }
        public tagFloat Fan102 { get; set; }
        public tagFloat FAN401 { get; set; }
        public tagFloat FAN402 { get; set; }
        public tagFloat FAN501 { get; set; }
        public tagFloat FAN502 { get; set; }
        public tagFloat COS101 { get; set; }
        public tagFloat KTC401 { get; set; }
        public tagInt ADAINT { get; set; }
        public tagInt ADBINT { get; set; }
        public tagFloat HYS401 { get; set; }
        public tagFloat PRT401 { get; set; }
        public tagFloat HYS501 { get; set; }
        public tagFloat HYS101 { get; set; }
        public tagFloat HYS102 { get; set; }
        public tagFloat LVL101 { get; set; }
        public tagFloat OXS101 { get; set; }
        public tagFloat PRT101 { get; set; }
        public tagFloat PRT102 { get; set; }
        public tagFloat TTC301 { get; set; }
        public tagFloat TTC101 { get; set; }
        public tagFloat TTC102 { get; set; }
        public tagFloat EBV801 { get; set; }
        public tagFloat DRYDPT { get; set; }
        public List<CELV> CELV01 { get; set; } // 0-50
        public List<CELV> CELV02 { get; set; } // 51-100
        public List<CELV> CELV03 { get; set; } // 101- 150
        public List<CELV> CELV04 { get; set; } // 151-198
        public List<CELR> CELR01 { get; set; }
        public List<CELR> CELR02 { get; set; }
        public List<CELR> CELR03 { get; set; }
        public List<CELR> CELR04 { get; set; }
        public List<CELI> CELH01 { get; set; }
        public List<CELI> CELH02 { get; set; }
        public List<CELI> CELH03 { get; set; }
        public List<CELI> CELH04 { get; set; }

        public tagInt BIN1PC { get; set; }

        public List<alarm> alarms { get; set; }

    }

    public class tagFloat
    {
        public string name { get; set; }
        public string documentation { get; set; }

        public string valueSource { get; set; }
        public float dataType { get; set; }

        public int engHigh { get; set; }
        public int engLow { get; set; }

        public string engUnit { get; set; }

        public string tagType { get; set; }
    }

    public class tagInt
    {
        public string name { get; set; }
        public string documentation { get; set; }

        public string valueSource { get; set; }
        public int dataType { get; set; }

        public int engHigh { get; set; }
        public int engLow { get; set; }

        public string engUnit { get; set; }

        public string tagType { get; set; }
    }

    public class CELV
    {
        public string name { get; set; }
        public string documentation { get; set; }

        public string valueSource { get; set; }
        public float[] dataType { get; set; }

        public int engHigh { get; set; }
        public int engLow { get; set; }

        public string engUnit { get; set; }

        public string tagType { get; set; }
    }

    public class CELR
    {
        public string name { get; set; }
        public string documentation { get; set; }

        public string valueSource { get; set; }
        public float[] dataType { get; set; }

        public int engHigh { get; set; }
        public int engLow { get; set; }

        public string engUnit { get; set; }

        public string tagType { get; set; }
    }

    public class CELI
    {
        public string name { get; set; }
        public string documentation { get; set; }

        public string valueSource { get; set; }
        public float[] dataType { get; set; }

        public int engHigh { get; set; }
        public int engLow { get; set; }

        public string engUnit { get; set; }

        public string tagType { get; set; }
    }

    public class alarm
    {

        public string label { get; set; }
        public bool mode { get; set; }

        public string name { get; set; }
        public string priority { get; set; }
        public int bitPosition { get; set; }
        public bool bitOnZero { get; set; }

    } }


public class ThresholdConfig //Usually moves towards read with higher values.  As an exception values such as Water Level gets to red with lower values
{
    public int id { get; set; }
    [Display(Name ="Parameter")]
    public string paramName { get; set; }
    public float minVal { get; set; }
    public float maxVal { get; set; }
    public string colorSortOrder { get; set; }
}


namespace Ohmium.Models.EFModels.LotusModels
{
    //-------------------------------------------------------------
    public class tag
    {
        public string name { get; set; }
        public string documentation { get; set; }

        public string valueSource { get; set; }
        public string dataType { get; set; }

        public int engHigh { get; set; }
        public int engLow { get; set; }

        public string engUnit { get; set; }

        public string tagType { get; set; }
    }

    public class LHC
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string tagType { get; set; }
        public List <tag> LHCTags  { get; set; }
        
        public List<CEL> cellVoltages { get; set; } // 0-198
      
        public List<CEL> cellResistance { get; set; }
        public List<CEL> CELInductance { get; set; }
      

        public tag alarmsTags { get; set; }

        public List<alarm> alarms { get; set; } //    BIN-1HC, BIN-2HC
        public tag CABSN { get; set; } // Cabinet Serial Number
        public tag CTRSN { get; set; } // Control Box Serial Number
        public tag CTRFW { get; set; } // CTRL Firmware Serial Number
        public tag VSCHW { get; set; } // Voltage Sense Card HW SN (12)
        public tag VSCFW { get; set; } // Voltage Sense Card SW SN (12)

        [Display(Name = "CREATED ON")]
        public DateTime createdOn { get; set; }
    }

    public class CEL
    {
        public string name { get; set; }
        public string documentation { get; set; }

        public string valueSource { get; set; }
        public float[] dataType1 { get; set; }
        public float[] dataType2 { get; set; }
        public float[] dataType3 { get; set; }
        public float[] dataType4 { get; set; }

        public int engHigh { get; set; }
        public int engLow { get; set; }

        public string engUnit { get; set; }

        public string tagType { get; set; }
    }

    public class alarm
    {
        public string label { get; set; }
        public bool mode { get; set; }

        public string name { get; set; }
        public string priority { get; set; }
        public int bitPosition { get; set; }
        public bool bitOnZero { get; set; }
    }

    public class LPC
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string tagType { get; set; }
        public List<tag> LPCTags { get; set; }

        public List<CEL> PSUI26 { get; set; } // Current for each 26 PSU
        public List<CEL> PSUV26 { get; set; } // Voltage for each 26 PSU

        public tag alarmsTags { get; set; }

        public List<alarm> alarms { get; set; }    //    BIN-1PC, BIN-2PC

        public tag CABSN { get; set; } // Cabinet Serial Number
        public tag CTRSN { get; set; } // Control Box Serial Number

        public tag CTR1FW { get; set; } // CTRL FW Master Safety      
        public tag CTR2FW { get; set; } // CTRL FW Master Process
        public tag CTR3FW { get; set; } // CTRL FW Gateway
        public tag CTR4FW { get; set; } // CTRL FW IO Card1
        public tag CTR5FW { get; set; } // CTRL FW IO Card2        
        public List<tag> PSUSN { get; set; } // Voltage Sense Card SW SN (12)
        public List<tag> PSU1FW { get; set; } // PSU Firware FEC Version(26)
        public List<tag> PSU2FW { get; set; } // PSU Firware DCDC Version(26)
        public DateTime createdOn { get; set; }

    }

    public class LWC
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string tagType { get; set; }
        public List<tag> LWCTags { get; set; }     

        public tag alarmsTags { get; set; }

        public List<alarm> alarms { get; set; }    //    BIN-1WC, BIN-2WC

        public tag CABSN { get; set; } // Cabinet Serial Number
        public tag CTRSN { get; set; } // Control Box Serial Number

       
        public List<tag> PSU1FW { get; set; } // PSU Firware FEC Version(26)
        public List<tag> PSU2FW { get; set; } // PSU Firware DCDC Version(26)

        [Display(Name = "CREATED ON")]
        public DateTime createdOn { get; set; }
   
    }

    public class LCC
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string tagType { get; set; }
        public List<tag> LCCTags { get; set; }

        public tag alarmsTags { get; set; }

        public List<alarm> alarms { get; set; }    //    BIN-1WC, BIN-2WC

        public tag CABSN { get; set; } // Cabinet Serial Number
        public tag CTR1SN { get; set; } // Control Box Serial Number
        public tag CTR2SN { get; set; } // Control Box Serial Number
        public tag CTR1FW { get; set; } // CTRL Firmware Version
        public tag CTR2FW { get; set; } // CTRL Firmware Version
        public DateTime createdOn { get; set; }
     
    }

    public class LTC
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string tagType { get; set; }
        public List<tag> LTCTags { get; set; }
        public tag CABSN { get; set; } // Cabinet Serial Number
        public tag CTRSN { get; set; } // Control Box Serial Number
        public tag CTR1FW { get; set; } // FW System Safety Version
        public tag CTR2FW { get; set; } // FW System Process Version

        public tag CTR3FW { get; set; } // FW IO Card Version
        public tag EDGSN { get; set; } // Edge Device Serial Number
        public tag EDGSW1 { get; set; } // Edge Firmare Version-RMCA
        public tag EDGSW2 { get; set; } // dge Firmare Version-FieldSupport
        public DateTime createdOn { get; set; }
    }


    public class SDA
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string tagType { get; set; }
        public List<tag> SDATags { get; set; }              
    }

    public class lotusControl
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string tagType { get; set; }
        public List<tag> LCTags { get; set; }
        public tag alarmsTags { get; set; }

        public List<alarm> alarms { get; set; }    //    BIN-1PC, BIN-2PC

        public DateTime createdOn { get; set; }
    }
    public class siteControl
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string tagType { get; set; }
        public List<tag> SCTags { get; set; }
        public DateTime createdOn { get; set; }
    }

    public class lotusMark1
    {
        public Guid id { get; set; }


        public List<LHC> lhc { get; set; }
        public List<LCC> lcc { get; set; }
        public LTC ltc { get; set; }
        public List<LPC> lpc { get; set; }
        public LWC lwc { get; set; }
        public SDA sda { get; set; }
        public siteControl siteCtrl { get; set; }
        public lotusControl lotusCtrl { get; set; }
        public DateTime createdOn { get; set; }
    }

    //-------------------------------------------------------------

    public class LotusTemp
    {
        public Guid id { get; set; }
        public string data { get; set; }
    }


    #endregion // End - Region for Lotus-Mark1 Device
    public class DeviceDataLog
    {
        [Key]
        public DateTime timeStamp { get; set; }
        public string description { get; set; }

    }

    public class StacksThatRan
    {
        [Key,Column(Order =1)]
        public string stackID { get; set; }
        [Key, Column(Order =2)]
        public string deviceID { get; set; }
    }
}