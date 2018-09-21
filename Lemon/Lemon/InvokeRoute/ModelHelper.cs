using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Configuration;

namespace Lemon.InvokeRoute
{
    internal static class ModelHelper
    {
        static ModelHelper()
        {
        }

        public static object SafeChangeType(string value, Type conversionType)
        {
            if (conversionType == typeof(string))
                return value;
            if (value == null || value.Length == 0)
            {
                // 空字符串根本不能做任何转换，所以直接返回null
                return null;
            }

            try
            {
                if (conversionType == typeof(Guid))
                    return new Guid(value);
                if (conversionType.IsEnum)
                    return Enum.Parse(conversionType, value);
                // 为了简单，直接调用 .net framework中的方法。
                // 如果转换失败，则会抛出异常。
                return Convert.ChangeType(value, conversionType);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 判断指定的类型是否能从String类型做隐式类型转换，如果可以，则返回相应的方法
        /// </summary>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private static MethodInfo GetStringImplicit(Type conversionType)
        {
            MethodInfo m = conversionType.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public);

            if (m != null && m.IsStatic && m.IsSpecialName && m.ReturnType == conversionType)
            {
                ParameterInfo[] paras = m.GetParameters();
                if (paras.Length == 1 && paras[0].ParameterType == typeof(string))
                    return m;
            }
            return null;
        }
    }
}
