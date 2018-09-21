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
    public class TaskTransferController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="par0">taskID</param>
        /// <returns></returns>
        [SessionValidate]
        public MyResult ListByTaskID(int par0)
        {
            List<ETaskTransfer> plist = DBBase.QueryCustom<ETaskTransfer>("select t.*,h.`Name`as AppointName,h.Face as AppointFace,e.`Name` as AssignedName,e.Face as AssignedFace,s.Name as StepName from tasktransfer t LEFT JOIN `user` h ON t.AppointPersonID=h.ID LEFT JOIN `user` e ON t.AssignedPersonID=e.ID LEFT JOIN task k ON t.TaskID=k.ID LEFT JOIN projecttaskstep s ON s.ProjectID=k.ProjectID and s.`Value`=t.ToState where t.TaskID=" + par0+ " order by t.CreateTime desc");
            return ServiceResult(plist);
        }

        [SessionValidate]
        public MyResult ListByUserID(int par0, int par1, int par2)
        {
            List<ETaskTransfer> plist = DBBase.QueryCustom<ETaskTransfer>(String.Format("select t.*,k.Title as TaskName,h.`Name`as AppointName,h.Face as AppointFace,e.`Name` as AssignedName,e.Face as AssignedFace,s.Name as StepName from tasktransfer t LEFT JOIN `user` h ON t.AppointPersonID=h.ID LEFT JOIN `user` e ON t.AssignedPersonID=e.ID LEFT JOIN task k ON t.TaskID=k.ID LEFT JOIN projecttaskstep s ON s.ProjectID=k.ProjectID and s.`Value`=t.ToState and s.`Value`=t.ToState where t.AppointPersonID={0} order by t.CreateTime desc limit {1},{2}", par0, par1 * par2, par2));
            return ServiceResult(plist);
        }
    }
}
