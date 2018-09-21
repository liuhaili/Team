using System;
using System.Collections.Generic;

namespace Lemon.RBAC
{
    public interface IRole
    {
        int Id{get;set;}
        string Name { get; set; }
        IList<string> FunctionIds { get; set; }

        IRole GetById(int id);
        IList<IRole> GetAll();
    }
}
