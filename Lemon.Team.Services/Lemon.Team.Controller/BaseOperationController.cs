using Lemon.Team.DAL;
using Lemon.Team.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lemon.Extensions;
using System.Reflection;
using Lemon.Team.Controller.Logic;

namespace Lemon.Team.Controllers
{
    public class BaseOperationController : ControllerBase
    {
        [SessionValidate]
        public MyResult Create(string par0, string par1)
        {
            Assembly asmb = Assembly.GetAssembly(typeof(ServiceReturn));
            Type type = asmb.GetType(par0);
            object obj = JsonConvert.DeserializeObject(par1, type);
            object newObj = DBBase.Create(obj);

            if (type == typeof(ETask))
            {
                ETask task = (ETask)obj;
                if (MySession.UserID != task.ExecutorID && task.ExecutorID != 0)
                {
                    EUser myuser = DBBase.Get<EUser>(MySession.UserID);
                    EUser excuteuser = DBBase.Get<EUser>(task.ExecutorID);
                    List<string> ulist = new List<string>();
                    ulist.Add(excuteuser.PushClientID);
                    PushMessageToList.PushToList(task.Title, "指派人" + myuser.Name, ulist, true);
                }
            }
            else if (type == typeof(EPlan))
            {
                EPlan plan = (EPlan)obj;
                EProject project = DBBase.Get<EProject>(plan.ProjectID);
                EUserSearch userSearch = new EUserSearch() { IsDefault = false, Name = project.Name + "-" + plan.Name, UserID = MySession.UserID, ProjectID = plan.ProjectID, PlanID = plan.ID };
                DBBase.Create(userSearch);
            }
            return ServiceResult(newObj);
        }

        [SessionValidate]
        public MyResult Change(string par0, string par1)
        {
            ServiceReturn ret = new ServiceReturn();
            Assembly asmb = Assembly.GetAssembly(typeof(ServiceReturn));
            Type type = asmb.GetType(par0);
            object obj = JsonConvert.DeserializeObject(par1, type);
            object newObj = DBBase.Change(obj);
            return ServiceResult(newObj);
        }

        [SessionValidate]
        public MyResult Delete(string par0, int par1)
        {
            ServiceReturn ret = new ServiceReturn();
            Assembly asmb = Assembly.GetAssembly(typeof(ServiceReturn));
            Type type = asmb.GetType(par0);
            DBBase.Delete(type, par1);
            return ServiceResult(par1.ToString());
        }

        [SessionValidate]
        public MyResult Get(string par0, int par1)
        {
            ServiceReturn ret = new ServiceReturn();
            Assembly asmb = Assembly.GetAssembly(typeof(ServiceReturn));
            Type type = asmb.GetType(par0);
            object newObj = null;
            if (type == typeof(EProject))
                newObj = DBBase.Get<EProject>(par1);
            else if (type == typeof(EFeedback))
                newObj = DBBase.Get<EFeedback>(par1);
            else if (type == typeof(EMessage))
                newObj = DBBase.Get<EMessage>(par1);
            else if (type == typeof(EPeople))
                newObj = DBBase.Get<EPeople>(par1);
            else if (type == typeof(EPlan))
                newObj = DBBase.Get<EPlan>(par1);
            else if (type == typeof(EProjectTeam))
                newObj = DBBase.Get<EProjectTeam>(par1);
            else if (type == typeof(ETask))
                newObj = DBBase.Get<ETask>(par1);
            else if (type == typeof(ETaskTransfer))
                newObj = DBBase.Get<ETaskTransfer>(par1);
            else if (type == typeof(EUser))
                newObj = DBBase.Get<EUser>(par1);
            else if (type == typeof(EUserSearch))
                newObj = DBBase.Get<EUserSearch>(par1);
            return ServiceResult(newObj);
        }
    }
}
