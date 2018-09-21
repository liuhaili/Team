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
    public class UserSearchController : ControllerBase
    {
        [SessionValidate]
        public MyResult ListMy()
        {
            List<EUserSearch> plist = DBBase.QueryCustom<EUserSearch>("select s.*,j.`Name` as ProjectName,p.`Name` as PlanName,t.Name as StepName from usersearch s LEFT JOIN project j on s.ProjectID=j.ID LEFT JOIN plan p on s.PlanID=p.ID LEFT JOIN projecttaskstep t ON t.ProjectID=s.ProjectID and t.`Value`=s.TaskState where s.UserID=" + MySession.UserID);
            return ServiceResult(plist);
        }

        [SessionValidate]
        public MyResult SetDefault(int par0)
        {
            List<EUserSearch> plist = DBBase.Query<EUserSearch>("UserID=" + MySession.UserID);
            foreach (var s in plist)
            {
                if (s.ID == par0)
                    s.IsDefault = true;
                else
                    s.IsDefault = false;
                DBBase.Change(s);
            }
            return ServiceResult("ok");
        }

        [SessionValidate]
        public MyResult GetDefault()
        {
            List<EUserSearch> plist = DBBase.QueryCustom<EUserSearch>("select s.*,j.`Name` as ProjectName,p.`Name` as PlanName,t.Name as StepName from usersearch s LEFT JOIN project j on s.ProjectID=j.ID LEFT JOIN plan p on s.PlanID=p.ID LEFT JOIN projecttaskstep t ON t.ProjectID=s.ProjectID and t.`Value`=s.TaskState where s.UserID=" + MySession.UserID + " and s.IsDefault=1");
            return ServiceResult(plist.FirstOrDefault());
        }

        [SessionValidate]
        public MyResult GetByID(int par0)
        {
            List<EUserSearch> plist = DBBase.QueryCustom<EUserSearch>("select s.*,j.`Name` as ProjectName,p.`Name` as PlanName,t.Name as StepName from usersearch s LEFT JOIN project j on s.ProjectID=j.ID LEFT JOIN plan p on s.PlanID=p.ID LEFT JOIN projecttaskstep t ON t.ProjectID=s.ProjectID and t.`Value`=s.TaskState where s.ID=" + par0);
            return ServiceResult(plist.FirstOrDefault());
        }
    }
}
