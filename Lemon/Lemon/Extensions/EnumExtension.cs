using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Extensions
{
    public static class EnumExtension
    {
        public static int ToInt(this Enum obj)
        {
            return Convert.ToInt32(obj);
        }
    }
}
