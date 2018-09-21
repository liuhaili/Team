using System;
using System.Collections.Generic;
using System.Configuration;

namespace Lemon.RBAC
{
    public class RBACContext
    {        
        public string RoleProvider
        {
            get;
            set;
        }
        public string LoginPageUrl
        {
            get;
            set;
        }
        public string ErrorPageUrl
        {
            get;
            set;
        }
        public IList<IRole> Roles
        {
            get;
            set;
        }
        public IList<FunctionGroup> Functions
        {
            get;
            set;
        }
        public Menu Menu { get; set; }
        public RBACContext()
        {
            Menu = new Lemon.RBAC.Menu();
            Functions = new List<FunctionGroup>();
            Roles = new List<IRole>();
        }
    }
}
