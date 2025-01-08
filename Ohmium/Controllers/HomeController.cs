using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ohmium.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ohmium.Utilities;
using Ohmium.Models.EFModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Ohmium.Filters;
using Ohmium.Repository;
using Microsoft.Extensions.Caching.Distributed;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
//using StackExchange.Redis;
using Microsoft.AspNetCore.Session;
using Ohmium.Controllers.SQLite;

namespace Ohmium.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SensorContext _context;
        private readonly CacheContext _cache;
        private readonly IHttpContextAccessor _accessor;
        public HomeController(ILogger<HomeController> logger, SensorContext Context, CacheContext cache, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _context = Context;
            _cache = cache;
            _accessor = accessor;
        }

        //[ProcessAuth]
        //[Authorize]
        public IActionResult Index()
        {
            //string name = _accessor.HttpContext.User.Identity.Name;
            //_accessor.HttpContext.Session.Set("user", name);
            //try
            //{
            //    UserLogin ul = new UserLogin()
            //    {
            //        name = ViewBag.Name,
            //        loginDateTime = DateTime.Now
            //    };
            //    _context.userLogin.Add(ul);
            //    _context.SaveChanges();
            //}
            //catch { }

            PushData p = new PushData();
            int count = _cache.deviceDataLog.ToList().Count();
            if (count>12000)
              ViewBag.Message=  p.DeleteLog();
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            string name = _accessor.HttpContext.User.Identity.Name;
            _accessor.HttpContext.Session.Set("user", name);
            try
            {
                UserLogin ul = new UserLogin()
                {
                    name = ViewBag.Name,
                    loginDateTime = DateTime.Now
                };
                _context.userLogin.Add(ul);
                _context.SaveChanges();
            }
            catch { }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Everyone")]
        public IActionResult Everyone()
        {
            return View();
        }
    }
}
