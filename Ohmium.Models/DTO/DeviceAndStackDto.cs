using Ohmium.Models.EFModels;
using System;

namespace Ohmium.Models.EFModels
{
    public class DeviceAndStackDto
    {
      
            public DateTime timeStamp { get; set; }
         
            public string deviceID { get; set; }
      
            public string siteID { get; set; }
            //[ForeignKey("eqConfig")]
            public Guid configID { get; set; }
            //public EquipmentConfiguration eqConfig { get; set; }
            public string ver { get; set; }
            public int status { get; set; }
            public float? wL { get; set; }
            public float? wP_d { get; set; }
            public float? wC { get; set; }
        public float? wT_d { get; set; }
        public float? hxiT { get; set; }
            public float? hxoT { get; set; }
            public float? wPp { get; set; }
        public float? HYS { get; set; }
        public string verM { get; set; }
            public Int16? CommStatus { get; set; }
            public string position { get; set; }
          
            //[Required]
            public string stackMfgID { get; set; }
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

        public float? cumulativeHours { get; set; }
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
        public int? stkStatus { get; set; }
        public string interLock { get; set; }
        public float? scriptLoopCnt { get; set; } = 0;
        public float? seqLoopCnt { get; set; } = 0;
        public float? scriptStep { get; set; } = 0;
        public float? seqStep { get; set; } = 0;

    }
}
