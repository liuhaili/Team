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
    public class ProjectController : ControllerBase
    {
        [SessionValidate]
        public MyResult Create(string par0)
        {
            EProject obj = JsonConvert.DeserializeObject<EProject>(par0);
            EProject newObj = CreateProject(obj, MySession.UserID);

            return ServiceResult(newObj);
        }

        public static EProject CreateProject(EProject obj,int userid)
        {
            EProject newObj = DBBase.Create(obj) as EProject;
            EProjectTeam projectTeam = new EProjectTeam() { UserID = userid, ProjectID = newObj.ID };
            DBBase.Create(projectTeam);

            EProjectTaskStep projectStep1 = new EProjectTaskStep() { ProjectID = newObj.ID, Name = "新任务", Value = 1 };
            DBBase.Create(projectStep1);
            EProjectTaskStep projectStep2 = new EProjectTaskStep() { ProjectID = newObj.ID, Name = "已完成", Value = 2 };
            DBBase.Create(projectStep2);
            return newObj;
        }


        [SessionValidate]
        public MyResult ListMyProject()
        {
            List<EProject> plist = DBBase.Query<EProject>("CreaterID=" + MySession.UserID);
            return ServiceResult(plist);
        }
        [SessionValidate]
        public MyResult ListTeamProject()
        {
            List<EProject> plist = DBBase.Query<EProject>("ID in (select ProjectID from projectteam where UserID=" + MySession.UserID + ")");
            return ServiceResult(plist);
        }
    }
}
