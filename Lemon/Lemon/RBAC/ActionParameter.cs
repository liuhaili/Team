using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.RBAC
{
    public class ActionParameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可忽略
        /// </summary>
        public bool Ignore
        {
            get;
            set;
        }
        /// <summary>
        /// 参数值，可以是正则表达式
        /// </summary>
        public string Value
        {
            get;
            set;
        }
        public bool IsMatch(ActionParameter input)
        {
            if (input == null)
                return false;
            return Name.Equals(input.Name) && new System.Text.RegularExpressions.Regex(Value).IsMatch(input.Value);
        }
    }
}
