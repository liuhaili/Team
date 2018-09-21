using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Lemon.Team.Services.Controllers
{
    public class SessionValidateAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //var qs = HttpUtility.ParseQueryString(filterContext.Request.Content);
            //string sessionKey = qs[SessionKeyName];
        }
    }
}
