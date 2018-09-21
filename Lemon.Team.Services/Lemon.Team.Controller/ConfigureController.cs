using Lemon.Team.DAL;
using Lemon.Team.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lemon.Team.Controllers
{
    public class ConfigureController : ControllerBase
    {
        [SessionValidate]
        public MyResult GetValue(string par0)
        {
            EConfigure obj = DBBase.Query<EConfigure>("`Key`='" + par0 + "'").FirstOrDefault();
            if (obj == null)
                return  ServiceErrorResult("未找到数据");
            else
            {
                return ServiceResult(obj);
            }
            
        }
    }
}
