using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ohmium.Utilities;
using Ohmium.Models.EFModels;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
//using System.Web.Mvc.Filters;
//using System.Web.Routing;


namespace Ohmium.Filters
{
    public class ProcessAuth : ActionFilterAttribute//, IAuthenticationFilter
    {


        public  Task<AuthenticateResult> AuthenticateAsync()
        {
            //To do : before the action executes  AuthorizationFilterContext context

            //User info = context.HttpContext.Session.Get<User>("User");
            //string user = info == null ? null : info.Login;
            //if (user == null)
            //{
            //    context.Result = new RedirectResult("/Login/Login");
            //    //  return View("~/Views/Login/Login.cshtml");
            //}
            //else
            //{

            //}
            return null;
        }

        public  Task<AuthenticateResult> AuthenticateAsync(AuthenticationProperties? properties, HttpContext context)
            {
            return null;
        }

        public Task ChallengeAsync(AuthenticationProperties? properties) {
            return null;
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
    {
            return null;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
            return null;
        }

}
}
