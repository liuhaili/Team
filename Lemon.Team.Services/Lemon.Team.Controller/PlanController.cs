using Lemon.Team.DAL;
using Lemon.Team.Entity;
using Lemon.Team.Entity.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lemon.Team.Controllers
{
    public class PlanController : ControllerBase
    {
        [SessionValidate]
        public MyResult ListMyProjectID(int par0)
        {
            List<EPlan> plist = DBBase.Query<EPlan>("ProjectID=" + par0);
            return ServiceResult(plist);
        }

        [SessionValidate]
        public MyResult GetPlan(int par0)
        {
            EPlan plan = DBBase.Query<EPlan>("ProjectID=" + par0).FirstOrDefault();
            MyResult ret = ServiceResult(plan);
            EProject project = DBBase.Get<EProject>(par0);
            ret.ServiceReturn.AddData("ProjectName", project.Name);
            return ret;
        }
    }
}
