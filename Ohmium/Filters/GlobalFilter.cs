using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ohmium.Filters
{
    public class GlobalFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
if(context.Exception.Message=="" || context.Exception.Message==null)
            {
                context.Result = new BadRequestObjectResult("Possible Remote Connection Error");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
