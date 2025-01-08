using Microsoft.AspNetCore.Mvc;
using Ohmium.Models;
using System.Linq;

namespace Ohmium.Controllers.RMCCAdmin
{
    public class TestController : Controller
    {
        private CacheContext _cacheContext;
        public TestController(CacheContext cacheContext)
        {
            _cacheContext = cacheContext;
        }

        public IActionResult OrgIndex()
        {
            return View(_cacheContext.orgCache.ToList());
        }

        public IActionResult Index()
        {
            return View(_cacheContext.org.ToList());
        }
    }
}
