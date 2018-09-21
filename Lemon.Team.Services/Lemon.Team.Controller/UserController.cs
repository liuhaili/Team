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
    public class UserController : ControllerBase
    {
        public MyResult Regist(string par0)
        {
            EUser obj = JsonConvert.DeserializeObject(par0, typeof(EUser)) as EUser;
            if (string.IsNullOrEmpty(obj.Phone) || string.IsNullOrEmpty(obj.Password))
            {
                return ServiceErrorResult("账号密码不能为空");
            }
            else
            {
                EUser olduser = DBBase.Query<EUser>("Phone='" + obj.Phone + "'").FirstOrDefault();
                if (olduser != null)
                {
                    return ServiceErrorResult("账号已存在");
                }
                else
                {
                    EUser newObj = DBBase.Create(obj) as EUser;
                    InitUser(newObj);
                    return ServiceResult(newObj.ID);
                }
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="par0">账号</param>
        /// <param name="par1">密码</param>
        /// <returns></returns>
        public MyResult Login(string par0, string par1, string par2)
        {
            EUser user = DBBase.Query<EUser>("Phone='" + par0 + "'").FirstOrDefault();
            if (user == null)
                return ServiceErrorResult("用户名或密码不正确");
            else
            {
                if (user.Password != par1)
                    return ServiceErrorResult("用户名或密码不正确");
                else
                {
                    //修改客户端推送ID
                    user.PushClientID = par2;
                    DBBase.Change(user);

                    return ServiceResult(user);
                }
            }
        }

        public MyResult PlatformLogin(string par0, string par1, string par2, string par3)
        {
            EUser user = DBBase.Query<EUser>("OpenID='" + par0 + "'").FirstOrDefault();
            if (user == null)
            {
                user = new EUser() { OpenID = par0, Name = par1, Face = par2, PushClientID = par3 };
                user = (EUser)DBBase.Create(user);
                InitUser(user);
            }
            else
            {
                //修改客户端推送ID
                user.PushClientID = par3;
                DBBase.Change(user);
            }
            return ServiceResult(user);
        }

        private void InitUser(EUser user)
        {
            EProject defaultProject = new EProject() { Name = "默认项目", CreateTime = DateTime.Now, CreaterID = user.ID };
            defaultProject = ProjectController.CreateProject(defaultProject, user.ID);

            EPlan defaultPlan = new EPlan() { ProjectID = defaultProject.ID, Name = "默认计划", BeginTime = DateTime.Now, EndTime = DateTime.Now };
            defaultPlan = (EPlan)DBBase.Create(defaultPlan);
            EUserSearch userSearch = new EUserSearch() { IsDefault = true, Name = "默认项目计划", UserID = user.ID, ProjectID = defaultPlan.ProjectID, PlanID = defaultPlan.ID };
            DBBase.Create(userSearch);
        }

        /// <summary>
        /// 查询字符串
        /// </summary>
        /// <param name="par0"></param>
        /// <returns></returns>
        [SessionValidate]
        public MyResult SearchUser(string par0)
        {
            List<EUser> retUserList = null;
            if (!string.IsNullOrEmpty(par0))
                retUserList = DBBase.Query<EUser>("ID<>" + MySession.UserID + " and Name like '%" + par0 + "%' order by ID desc limit 20");
            else
                retUserList = DBBase.Query<EUser>("ID<>" + MySession.UserID + " order by ID desc limit 20");
            return ServiceResult(retUserList);
        }
    }
}
