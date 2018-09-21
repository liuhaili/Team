using Lemon.Team.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Lemon.Team.Controllers
{
    public class SessionValidateAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        static Dictionary<string, object> clickGUID = new Dictionary<string, object>();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ControllerBase cbase = filterContext.Controller as ControllerBase;
            if (cbase != null)
            {
                Session s = GetSession(cbase.Request);
                if (s == null)
                {
                    var response = new HttpResponseMessage();
                    response.Content = new StringContent("用户登录信息不正确");
                    response.StatusCode = HttpStatusCode.Conflict;
                    throw new HttpResponseException(response);
                }
                else
                    cbase.MySession = s;
            }
        }

        private Session GetSession(HttpRequestBase request)
        {
            string sessionKey = request.Headers["SessionKey"];
            if (string.IsNullOrEmpty(sessionKey))
                return null;
            string[] spar = sessionKey.Split('_');
            if (clickGUID.ContainsKey(spar[1]))
            {
                return null;
            }
            clickGUID.Add(spar[1], "");
            string serverSignal = GetSessionSignal(spar[0], spar[1]);
            if (serverSignal != spar[2])
                return null;
            Session s = new Session();
            s.UserID = int.Parse(spar[0]);
            return s;
        }

        private string GetSessionSignal(string userID,string guid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("lemon20120530");
            sb.Append(userID);
            sb.Append(guid);
            MD5 newMd5 = new MD5CryptoServiceProvider();
            byte[] sourceBit = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] directBit = newMd5.ComputeHash(sourceBit);
            return BitConverter.ToString(directBit).Replace("-", "").ToLower();
        }
    }
}
