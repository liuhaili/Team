using Lemon.Team.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lemon.Team.Controllers
{
    public class MyResult : ActionResult
    {
        public ServiceReturn ServiceReturn;

        public MyResult() { ServiceReturn = new ServiceReturn(); }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Output.Write(JsonConvert.SerializeObject(ServiceReturn)); // 输出流对象 
        }
    }
}
