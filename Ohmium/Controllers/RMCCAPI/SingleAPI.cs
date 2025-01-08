using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ohmium.Models;
using Ohmium.Models.EFModels.ViewModels;
using Ohmium.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ohmium.Controllers.RMCCAPI
{
    //[EnableCors("MyAllowSpecificOrigins")]
    //[EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class SingleAPI : ControllerBase
    {
        private SensorContext _context;
        public SingleAPI(SensorContext context)
        {
            _context = context;
        }
        [HttpGet]
        public OrgSiteTestStandStack OSTS()
        {
            OrgSiteTestStandStack orgSiteTestStandStack = new OrgSiteTestStandStack()
            {
                deviceList = _context.device.ToList(),
                orgList = _context.org.ToList(),
                siteList = _context.site.ToList(),
                stackList = _context.stack.ToList()
            };
            return orgSiteTestStandStack;
        }
     
    }
}
