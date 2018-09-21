using Lemon.Team.Controller.Logic;
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
    public class AppController : ControllerBase
    {
        public MyResult StartData()
        {
            List<EUser> allUser = DBBase.Query<EUser>();
            return ServiceResult(allUser);
        }

        public string TestPush(string id,string content)
        {
            List<string> ulist = new List<string>();
            ulist.Add(id);
            PushMessageToList.PushToList("haili:任务", content, ulist,true);
            return "ok";
        }
    }
}
