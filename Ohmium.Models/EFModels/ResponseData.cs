using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ohmium.Models.EFModels
{
   public class ResponseData
    {
        public List<MTSStackData> stackData { get; set; }
        public List<MTSDeviceData> deviceData { get; set; }
        public List<DeviceAndStackDto>? deviceAndStack { get; set; }
    }

    public class StackWithHeader
    {
        public string cV_Ohms { get; set; }

    }

    public class StackSensorViewModel
    {
        public DateTime tstamp { get; set; }
        public string sensorData { get; set; }
        public string stkMfgId { get; set; }
    }
    public class StackConfigViewModel
    {
        public string stackID { get; set; }
        public string config { get; set; }
    }
}
