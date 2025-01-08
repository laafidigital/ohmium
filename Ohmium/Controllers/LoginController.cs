using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ohmium.Models.EFModels;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Ohmium.Utilities;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Okta.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Ohmium.Controllers
{
   
    public class LoginController : Controller
    {
        //Hosted web API REST Service base url for Okta
        string Baseurl = "https://ohmiumokta.azurewebsites.net/";
        // GET: LoginController
        
        [HttpGet]
        public ActionResult Login()
        {
            ViewData["Email"] = "Email";
            ViewData["Password"] = "Password";
            return View();
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Login(InputModel input)
        {
           
            if (ModelState.IsValid)
            {
              //  string result = string.Empty;
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   // Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("api/Function1?username=" + input.Email + "&password=" + input.Password);
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var LoginResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        //result = LoginResponse.ToString();

                        var result = JsonConvert.DeserializeObject<AuthResponse>(LoginResponse);
                        if (result.WasSuccessful)
                        {
                            HttpContext.Session.Set<User>("User", result.User);
                            await Task.Delay(10);
                            // User.Identity.IsAuthenticated = true;

                            if (!HttpContext.User.Identity.IsAuthenticated)
                            {
                               // return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
                            }
                           // return RedirectToAction("Index", "Home");
                            return View("~/Views/Home/Index.cshtml");
                        }
                        else
                        {
                           // input.errorText = result.Message;
                            return View("~/Views/shared/login/login.cshtml");    
                        }
                    }
                    //returning the Login object list to view                    
                }
            }
            
        return View("~/Views/Home/Index.cshtml");
    }
        [HttpGet]
        [Route("/Login/Logout")]
        public ActionResult Logout()
        {

            //HttpContext.Current.Session.Abandon();
            //HttpContext.Session.r.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            HttpContext.Session.Remove("User");
            HttpContext.Session.Clear();
            //HttpContext.Session.SetString("AspNetCore.Session", null);
            //HttpContext.Abort();
             return View();
            //return new SignOutResult(
            //   new[]
            //   {
            //         OktaDefaults.MvcAuthenticationScheme,
            //         CookieAuthenticationDefaults.AuthenticationScheme,
            //   },
            //   new AuthenticationProperties { RedirectUri = "/logout" }); 
            //   //"~/Login/logout" });

        }    

    }
}
