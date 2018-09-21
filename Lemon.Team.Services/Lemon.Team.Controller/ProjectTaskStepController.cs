using Lemon.Team.DAL;
using Lemon.Team.Entity;
using Lemon.Team.Entity.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lemon.Extensions;

namespace Lemon.Team.Controllers
{
    public class ProjectTaskStepController : ControllerBase
    {
        [SessionValidate]
        public MyResult ListByProjectID(int par0)
        {
            List<EProjectTaskStep> steps = DBBase.Query<EProjectTaskStep>("ProjectID=" + par0);
            return ServiceResult(steps);
        }
    }
}
