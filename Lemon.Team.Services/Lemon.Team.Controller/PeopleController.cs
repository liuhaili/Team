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
    public class PeopleController : ControllerBase
    {
        /// <summary>
        /// is old
        /// </summary>
        /// <param name="par0"></param>
        /// <returns></returns>
        [SessionValidate]
        public MyResult Create(string par0)
        {
            EPeople p = JsonConvert.DeserializeObject<EPeople>(par0);
            DBBase.Create(p);
            List<EUser> alluser = DBBase.Query<EUser>();
            return ServiceResult(alluser);
        }
        [SessionValidate]
        public MyResult AskConnect(int par0)
        {
            EPeople people = new EPeople();
            people.UserID = par0;
            people.PeopleID = MySession.UserID;
            people.State = Lemon.Team.Entity.Enum.PeopleState.NeedConfirm;

            EPeople people2 = new EPeople();
            people2.UserID = MySession.UserID;
            people2.PeopleID = par0;
            people2.State = Lemon.Team.Entity.Enum.PeopleState.Request;

            DBBase.Create(people);
            DBBase.Create(people2);

            return ServiceResult("ok");
        }
        [SessionValidate]
        public MyResult ConfirmConnect(int par0, bool par1)
        {
            EPeople people = DBBase.Get<EPeople>(par0);
            if (par1)
            {
                people.State = Entity.Enum.PeopleState.Normal;
                EPeople other = DBBase.Query<EPeople>(string.Format("UserID={0} and PeopleID={1}", people.PeopleID, people.UserID)).FirstOrDefault();
                if (other != null)
                {
                    other.State = Entity.Enum.PeopleState.Normal;
                    DBBase.Change(other);
                }
                DBBase.Change(people);
            }
            else
            {
                EPeople other = DBBase.Query<EPeople>(string.Format("UserID={0} and PeopleID={1}", people.PeopleID, people.UserID)).FirstOrDefault();
                if (other != null)
                    DBBase.Delete<EPeople>(other.ID);
                DBBase.Delete<EPeople>(people.ID);
            }
            return ServiceResult("ok");
        }

        [SessionValidate]
        public MyResult DisConnect(int par0)
        {
            EPeople people = DBBase.Query<EPeople>(string.Format("UserID={0} and PeopleID={1}", MySession.UserID, par0)).FirstOrDefault();
            EPeople other = DBBase.Query<EPeople>(string.Format("UserID={0} and PeopleID={1}", par0, MySession.UserID)).FirstOrDefault();
            if (people != null)
                DBBase.Delete<EPeople>(people.ID);
            if (other != null)
                DBBase.Delete<EPeople>(other.ID);
            return ServiceResult("ok");
        }
        [SessionValidate]
        public MyResult ListMyPeople()
        {
            List<EPeople> plist = DBBase.QueryCustom<EPeople>(@"select p.*,u.`Name`as PeopleName,u.Face as PeopleFace from people p LEFT JOIN `user` u ON p.PeopleID=u.ID where UserID=" + MySession.UserID);
            return ServiceResult(plist);
        }

        [SessionValidate]
        public MyResult GetMyOnePeople(int par0)
        {
            EPeople people = DBBase.QueryCustom<EPeople>("select p.*,u.`Name`as PeopleName,u.Face as PeopleFace from people p LEFT JOIN `user` u ON p.PeopleID=u.ID where p.UserID=" + MySession.UserID + " and p.PeopleID=" + par0).FirstOrDefault();
            return ServiceResult(people);
        }
    }
}
