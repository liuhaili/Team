using System;
using System.Collections.Generic;

namespace Lemon.RBAC
{
    [Serializable]
    public class FunctionGroup
    {        
        public string Name
        {
            get;
            set;
        }
        public IList<Function> Functions
        {
            get;
            set;
        }
        public FunctionGroup()
        {
            Functions = new List<Function>();
        }
    }
}
