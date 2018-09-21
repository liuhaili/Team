using Lemon.Team.Controller.Logic;
using Lemon.Team.DAL;
using Lemon.Team.Entity;
using Lemon.Team.Entity.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Lemon.Team.Controllers
{
    public class MessageController : ControllerBase
    {
        [SessionValidate]
        public MyResult ListByTargetID(MessageType par0, int par1)
        {
            List<EMessage> plist = null;
            if (par0 == MessageType.System)
                plist = DBBase.QueryCustom<EMessage>("select m.*,u.`Name`as SenderName,u.Face as SenderFace from message m LEFT JOIN `user` u ON m.SenderID=u.ID where (ReceiverID=" + MySession.UserID + " or ReceiverID= -100) and Type=" + (int)par0+" order by m.ID desc limit 20");
            else if (par0 == MessageType.Project)
                plist = DBBase.QueryCustom<EMessage>("select m.*,u.`Name`as SenderName,u.Face as SenderFace from message m LEFT JOIN `user` u ON m.SenderID=u.ID where ReceiverID=" + par1 + " and Type=" + (int)par0+ " order by m.ID desc limit 20");
            else
                plist = DBBase.QueryCustom<EMessage>("select m.*,u.`Name`as SenderName,u.Face as SenderFace from message m LEFT JOIN `user` u ON m.SenderID=u.ID where ((SenderID=" + par1 + " and ReceiverID=" + MySession.UserID + ") or (ReceiverID=" + par1 + " and SenderID=" + MySession.UserID + ")) and Type=" + (int)par0+ " order by m.ID desc limit 20");
            return ServiceResult(plist);
        }

        [SessionValidate]
        public MyResult SendMessage(string par0)
        {
            Type type = typeof(EMessage);
            object obj = JsonConvert.DeserializeObject(par0, type);
            EMessage newObj = DBBase.Create(obj) as EMessage;

            //发送推送
            EUser myuser = DBBase.Get<EUser>(newObj.SenderID);
            if (newObj.Type == MessageType.OneToOne)
            {
                //接收者加个提示
                ENewTips tips = new ENewTips() { UserID = newObj.ReceiverID, Type = newObj.Type, OwnerID = newObj.SenderID };
                DBBase.Create(tips);
                //推送给接收者
                EUser excuteuser = DBBase.Get<EUser>(newObj.ReceiverID);
                List<string> ulist = new List<string>();
                ulist.Add(excuteuser.PushClientID);
                PushMessageToList.PushToList(newObj.Content, "发送人" + myuser.Name, ulist, false);
                PushMessageToList.PushToList(newObj.Content, "发送人" + myuser.Name, ulist, true);
            }
            else if (newObj.Type == MessageType.Project)
            {
                List<EUser> teamUsers = DBBase.QueryCustom<EUser>("select * from user where ID in(select UserID from projectteam where ProjectID=" + newObj.ReceiverID + ")");
                List<string> ulist = new List<string>();
                foreach (var u in teamUsers)
                {
                    ulist.Add(u.PushClientID);
                    //每个人加个提示
                    ENewTips tips = new ENewTips() { UserID = u.ID, Type = newObj.Type, OwnerID = newObj.ReceiverID };
                    DBBase.Create(tips);
                }
                //推送给所有接收者
                PushMessageToList.PushToList(newObj.Content, "发送人" + myuser.Name, ulist, false);
                PushMessageToList.PushToList(newObj.Content, "发送人" + myuser.Name, ulist, true);
            }

            return ServiceResult("ok");
        }

        [SessionValidate]
        public MyResult ListMyTips()
        {
            List<ENewTips> plist = DBBase.Query<ENewTips>("UserID=" + MySession.UserID);
            return ServiceResult(plist);
        }

        [SessionValidate]
        public MyResult ClearMyTips(MessageType par0, int par1)
        {
            DBBase.ExcuteCustom(string.Format("delete from newtips where UserID={0} and `Type`={1} and OwnerID={2}", MySession.UserID, (int)par0, par1));
            List<ENewTips> plist = DBBase.Query<ENewTips>("UserID=" + MySession.UserID);
            return ServiceResult(plist);
        }
    }
}
