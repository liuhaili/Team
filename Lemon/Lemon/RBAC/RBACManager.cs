using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace Lemon.RBAC
{
    public class RBACManager
    {
        /// <summary>
        /// 一般不对外公开
        /// </summary>
        public static RBACContext RBACContext = RBACConfig.Load();
        public static Menu GetNavigation(string rolePrivilege)
        {
            Menu menu = new Menu();
            FindMenu(menu, RBACContext.Menu.Children, rolePrivilege);
            return menu;
        }
        public static IList<ActionEntity> ActionList
        {
            get
            {
                return RBACContext.Menu.ActionList;
            }
        }
        public static Function FindFunction(string functionId)
        {
            foreach (FunctionGroup funcGroup in RBACContext.Functions)
            {
                foreach (Function func in funcGroup.Functions)
                {
                    if (func.Id == functionId)
                        return func;
                }
            }
            return null;
        }
        public static IList<FunctionGroup> GetFunctionsByRoleId(int roleId)
        {
            IList<FunctionGroup> funcList = new List<FunctionGroup>();
            IList<IRole> roleList = RoleFactory.Instance.GetAll();
            IRole role=null;
            foreach (IRole rl in roleList)
            {
                if (rl.Id == roleId)
                    role = rl;
            }

            foreach (FunctionGroup funcGroup in RBACContext.Functions)
            {
                FunctionGroup funcg = new FunctionGroup();
                funcg.Name = funcGroup.Name;
                foreach (Function func in funcGroup.Functions)
                {
                    if (role.FunctionIds.Contains(func.Id))
                        funcg.Functions.Add(func);
                }
                if (funcg.Functions.Count > 0)
                    funcList.Add(funcg);
            }
            return funcList;
        }
        private static bool FindMenu(Menu nav, IList<Menu> children, string rolePrivilege)
        {
            bool haveChild = false;
            if (children == null || children.Count == 0)
                return haveChild;
            foreach (Menu m in children)
            {
                Menu newm = new Menu();
                newm.DN = m.DN;
                newm.Name = m.Name;
                newm.Url = m.Url;
                newm.Icon = m.Icon;
                newm.Index = m.Index;
                newm.Privilege = m.Privilege;
                newm.Parent = nav;
                newm.ActionList = m.ActionList;

                if (m.Children.Count > 0)
                {
                    if (FindMenu(newm, m.Children, rolePrivilege))
                    {
                        haveChild = true;
                        nav.Children.Add(newm);
                    }
                    else
                        newm = null;
                }
                else
                {
                    if (string.IsNullOrEmpty(rolePrivilege))
                        newm = null;
                    else
                    {
                        if ((!String.IsNullOrEmpty(m.Privilege) && rolePrivilege.ToLower().Split('|').Any(c => c.StartsWith(m.Privilege.ToLower()))) ||
                            m.Url.ToLower() == "/home/index")
                        {
                            haveChild = true;
                            nav.Children.Add(newm);
                        }
                        else
                            newm = null;
                    }
                }
            }
            return haveChild;
        }
        /// <summary>
        /// 根据url和菜单的级别查找出菜单
        /// </summary>
        /// <param name="parenturl"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Menu GetMenuByUrl(string parenturl, int level, string rolePrivilege)
        {
            var nav = GetNavigation(rolePrivilege);
            Menu m = GetMenusByUrl(nav, parenturl, nav);
            if (m != null)
                return GetMenuByUrl(m, level);
            return null;
        }
        /// <summary>
        /// 向菜单的上级递归查找指定级别的菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private static Menu GetMenuByUrl(Menu menu, int level)
        {
            if (menu == null)
                return null;
            if (menu.Level == level - 1)
                return menu;
            else if (menu.Level == level)
                return menu.Parent;
            else
                return GetMenuByUrl(menu.Parent, level);
        }
        private static Menu GetMenusByUrl(Menu parentmenu, string parenturl, Menu nav)
        {
            if (nav == null || nav.Children == null || nav.Children.Count == 0)
                return null;

            ActionEntity act1 = new ActionEntity();
            act1.Url = parenturl;

            foreach (Menu m in parentmenu.Children)
            {
                //看看url是否在action里面
                if (m.ActionList != null)
                {
                    foreach (var act in m.ActionList)
                    {

                        if (act.IsMatch(act1))
                            return m;
                    }
                }
                //看和本节点是否匹配
                if (parenturl.ToLower() == m.Url.ToLower())
                    return m;
                else
                {
                    Menu menu = GetMenusByUrl(m, parenturl, nav);
                    if (menu != null)
                        return menu;
                }
            }
            return null;
        }
    }
}
