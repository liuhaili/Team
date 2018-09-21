using System;
using System.Reflection;
using System.IO;
using System.Web;
using Lemon.RBAC.RoleProviders;

namespace Lemon.RBAC
{
    public class RoleFactory
    {       
        private static IRole provider = null;
        private static object _lock = new object();

        public static IRole Instance
        {
            get
            {
                if (provider == null)
                {
                    lock (_lock)
                    {
                        if (provider == null)
                        {
                            if (RBACManager.RBACContext.RoleProvider == null)
                                return new XMLRole();
                            //string rootPath = HttpUtility.UrlDecode(Path.GetDirectoryName(new Uri(typeof(RoleFactory).Assembly.CodeBase).AbsolutePath));
                            string rootPath = Path.GetDirectoryName(new Uri(typeof(RoleFactory).Assembly.CodeBase).AbsolutePath);
                            provider = Assembly.LoadFrom(rootPath + "\\" + RBACManager.RBACContext.RoleProvider.Split(',')[1].Trim() + ".dll")
                                .GetType(RBACManager.RBACContext.RoleProvider.Split(',')[0].Trim())
                                .GetConstructor(Type.EmptyTypes).Invoke(new object[0]) as IRole;
                        }
                    }
                }
                return provider;
            }
        }
    }
}
