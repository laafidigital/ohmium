using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ohmium.Models
{
  public  class Request
    {
        public Guid id { get; set; }
        public string query { get; set; }
        public VisualizationRequest vr { get; set; }
    }

    public class VisualizationRequest
    {
        public string siteID { get; set; }
        public string deviceID { get; set; }
        public string sensorName { get; set; }
        public string stkMfgID { get; set; }
        public DateTime sTime { get; set; } = DateTime.Now.AddSeconds(-3).ToUniversalTime();
        public DateTime eTime { get; set; } = DateTime.Now.AddSeconds(-3).ToUniversalTime();
        public string[] selectedStack { get; set; }
        public string timeZone { get; set; }
        public string sequenceName { get; set; }
        public int sec { get; set; }
    }
}
