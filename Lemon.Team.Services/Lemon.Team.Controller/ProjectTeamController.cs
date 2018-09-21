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
    public class ProjectTeamController : ControllerBase
    {
        [SessionValidate]
        public MyResult GetByProjectID(int par0, int par1)
        {
            EProjectTeam teamOne = DBBase.QueryCustom<EProjectTeam>("select p.*,u.`Name`as UserName,u.Face as UserFace from projectteam p LEFT JOIN `user` u ON p.UserID=u.ID where p.UserID=" + par0 + " and p.ProjectID=" + par1).FirstOrDefault();
            return ServiceResult(teamOne);
        }

        [SessionValidate]
        public MyResult ListByProjectID(int par0)
        {
            List<EProjectTeam> team = DBBase.QueryCustom<EProjectTeam>("select p.*,u.`Name`as UserName,u.Face as UserFace from projectteam p LEFT JOIN `user` u ON p.UserID=u.ID where p.ProjectID=" + par0);
            return ServiceResult(team);
        }
    }
}
