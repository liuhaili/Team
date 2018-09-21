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
using Lemon.Team.Controller.Logic;

namespace Lemon.Team.Controllers
{
    public class TaskController : ControllerBase
    {
        [SessionValidate]
        public MyResult Get(int par0)
        {
            ETask newObj = DBBase.QueryCustom<ETask>("select t.*,h.`Name`as TaskHeadName,h.Face as TaskHeadFace,e.`Name` as ExecutorName,e.Face as ExecutorFace,s.Name as StepName from task t LEFT JOIN `user` h ON t.TaskHeadID=h.ID LEFT JOIN `user` e ON t.ExecutorID=e.ID LEFT JOIN projecttaskstep s ON s.ProjectID=t.ProjectID and s.`Value`=t.State where t.ID=" + par0).FirstOrDefault();
            return ServiceResult(newObj);
        }
        /// <summary>
        /// 任务处理 old
        /// </summary>
        /// <param name="par0">taskID</param>
        /// <param name="par1">excuterID</param>
        /// <param name="par2">note</param>
        /// <returns></returns>
        [SessionValidate]
        public MyResult TaskHandle(int par0, int par1, string par2)
        {
            ETask task = DBBase.Get<ETask>(par0);
            task.State = task.State + 1;
            task.ExecutorID = par1;
            ETaskTransfer taskTransfer = new ETaskTransfer()
            {
                TaskID = task.ID,
                AppointPersonID = MySession.UserID,
                AssignedPersonID = par1,
                CreateTime = System.DateTime.Now,
                ToState = task.State,
                Note = par2
            };
            DBBase.Change(task);
            DBBase.Create(taskTransfer);

            if (MySession.UserID != task.ExecutorID && task.ExecutorID != 0)
            {
                EUser myuser = DBBase.Get<EUser>(MySession.UserID);
                EUser excuteuser = DBBase.Get<EUser>(task.ExecutorID);
                List<string> ulist = new List<string>();
                ulist.Add(excuteuser.PushClientID);
                PushMessageToList.PushToList(task.Title, "指派人" + myuser.Name, ulist,true);
            }

            return ServiceResult(task.ID);
        }

        [SessionValidate]
        public MyResult TaskProcess(int par0, int par1, int par2,string par3)
        {
            ETask task = DBBase.Get<ETask>(par0);
            task.ExecutorID = par1;
            task.State = par2;
            ETaskTransfer taskTransfer = new ETaskTransfer()
            {
                TaskID = task.ID,
                AppointPersonID = MySession.UserID,
                AssignedPersonID = par1,
                CreateTime = System.DateTime.Now,
                ToState = task.State,
                Note = par3
            };
            DBBase.Change(task);
            DBBase.Create(taskTransfer);

            if (MySession.UserID != task.ExecutorID && task.ExecutorID != 0)
            {
                EUser myuser = DBBase.Get<EUser>(MySession.UserID);
                EUser excuteuser = DBBase.Get<EUser>(task.ExecutorID);
                List<string> ulist = new List<string>();
                ulist.Add(excuteuser.PushClientID);
                PushMessageToList.PushToList(task.Title, "指派人" + myuser.Name, ulist, true);
            }

            return ServiceResult(task.ID);
        }

        [SessionValidate]
        public MyResult SetComplated(int par0)
        {
            ETask task = DBBase.Get<ETask>(par0);
            task.IsComplated = true;
            ETaskTransfer taskTransfer = new ETaskTransfer()
            {
                TaskID = task.ID,
                AppointPersonID = MySession.UserID,
                AssignedPersonID = 0,
                CreateTime = System.DateTime.Now,
                ToState = task.State,
                Note = "设置为已完成"
            };
            DBBase.Change(task);
            DBBase.Create(taskTransfer);

            return ServiceResult(task.ID);
        }
        /// <summary>
        /// 获取负责的任务
        /// </summary>
        /// <param name="par0">planID</param>
        /// <param name="par1">handleID</param>
        /// <returns></returns>
        [SessionValidate]
        public MyResult ListByPlanID(int par0, int par1)
        {
            List<ETask> plist = DBBase.QueryCustom<ETask>("select t.*,h.`Name`as TaskHeadName,h.Face as TaskHeadFace,e.`Name` as ExecutorName,e.Face as ExecutorFace,s.Name as StepName from task t LEFT JOIN `user` h ON t.TaskHeadID=h.ID LEFT JOIN `user` e ON t.ExecutorID=e.ID LEFT JOIN projecttaskstep s ON s.ProjectID=t.ProjectID and s.`Value`=t.State where t.PlanID=" + par0);
            if (par1 > 0)
                plist = plist.Where(c => c.TaskHeadID == par1).ToList();
            return ServiceResult(plist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="par0">planID</param>
        /// <param name="par1">taskState</param>
        /// <param name="par2">taskPriority</param>
        /// <returns></returns>
        [SessionValidate]
        public MyResult ListMyHomeTask(int par0, int par1, int par2, TaskPriority? par3)
        {
            List<ETask> plist = null;
            if (par0 != 0)
                plist = DBBase.QueryCustom<ETask>("select t.*,h.`Name`as TaskHeadName,h.Face as TaskHeadFace,e.`Name` as ExecutorName,e.Face as ExecutorFace,s.Name as StepName from task t LEFT JOIN `user` h ON t.TaskHeadID=h.ID LEFT JOIN `user` e ON t.ExecutorID=e.ID LEFT JOIN projecttaskstep s ON s.ProjectID=t.ProjectID and s.`Value`=t.State where t.ProjectID=" + par0 + " and t.ExecutorID=" + MySession.UserID);
            else
                plist = DBBase.QueryCustom<ETask>("select t.*,h.`Name`as TaskHeadName,h.Face as TaskHeadFace,e.`Name` as ExecutorName,e.Face as ExecutorFace,s.Name as StepName from task t LEFT JOIN `user` h ON t.TaskHeadID=h.ID LEFT JOIN `user` e ON t.ExecutorID=e.ID LEFT JOIN projecttaskstep s ON s.ProjectID=t.ProjectID and s.`Value`=t.State where t.ExecutorID=" + MySession.UserID);
            if (par1 != 0)
                plist = plist.Where(c => c.PlanID == par1).ToList();
            if (par2>0)
                plist = plist.Where(c => c.State == par2).ToList();
            if (par3.HasValue)
                plist = plist.Where(c => c.Priority == par3.Value).ToList();
            return ServiceResult(plist);
        }
    }
}
