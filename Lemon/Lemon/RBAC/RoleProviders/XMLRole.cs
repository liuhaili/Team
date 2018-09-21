using System;
using System.Collections.Generic;
using System.Configuration;

namespace Lemon.RBAC.RoleProviders
{
    public class XMLRole : IRole
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<string> FunctionIds { get; set; }

        public XMLRole()
        {
            FunctionIds = new List<string>();
        }

        public IRole GetById(int id)
        {
            if (RBACManager.RBACContext.Roles == null || RBACManager.RBACContext.Roles.Count == 0)
                return null;
            foreach (IRole role in RBACManager.RBACContext.Roles)
            {
                if (role.Id == id)
                    return role;
            }
            return null;
        }

        public IList<IRole> GetAll()
        {
            return RBACManager.RBACContext.Roles;
        }
    }
}
