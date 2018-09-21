using Lemon.Team.DAL;
using Lemon.Team.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lemon.Team.Controllers
{
    public class ControllerBase : System.Web.Mvc.Controller
    {
        public Session MySession;
        protected MyResult ServiceResult(params object[] objs)
        {
            MyResult ret = new MyResult();
            foreach (var obj in objs)
            {
                if (obj == null)
                {
                    ret.ServiceReturn.SetData(null);
                    continue;
                }
                Type type = obj.GetType();
                if (type == typeof(string)
                    || type == typeof(DateTime)
                    || !type.IsClass
                    || type.IsEnum
                    )
                    ret.ServiceReturn.SetData(obj);
                else
                    ret.ServiceReturn.SetData(JsonConvert.SerializeObject(obj));
            }

            return ret;
        }

        protected MyResult ServiceErrorResult(string msg)
        {
            MyResult ret = new MyResult();
            ret.ServiceReturn.SetMessage(msg);
            return ret;
        }
    }
}
