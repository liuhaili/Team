using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Lemon.Extensions;

namespace Lemon.RBAC
{
    public class PrivilegeManager
    {
        public static ActionEntity FindAction(string url,IDictionary<string,ActionParameter> pars)
        {
            ActionEntity input = new ActionEntity() { Url = url, Constraints = pars };
            if (RBACManager.ActionList == null || RBACManager.ActionList.Count == 0)
                return null;
            foreach (ActionEntity section in RBACManager.ActionList)
            {
                if (section.IsMatch(input))
                    return section;
            }
            return null;
        }

        public static bool VerifyAction(ActionEntity action,string rolePrivilege)
        {
            if (RBACManager.ActionList == null || rolePrivilege.IsNullOrEmpty())
                return false;
            //验证此角色是否有访问该页面的权限
            return rolePrivilege.ToLower().Contains(action.Privilege.ToLower());
        }
        /// <summary>
        /// 验证控件是否有权限
        /// </summary>
        /// <param name="privilege">功能号</param>
        /// <returns></returns>
        public static bool VerifyPrivilege(string privilege, string rolePrivilege)
        {
            if (RBACManager.ActionList == null || String.IsNullOrEmpty(rolePrivilege))
                return false;
            return rolePrivilege.ToLower().Contains(privilege.ToLower());
        }
    }
}
