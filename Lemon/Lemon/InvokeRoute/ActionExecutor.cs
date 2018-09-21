using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;

namespace Lemon.InvokeRoute
{
    public static class ActionExecutor
    {
        public static object ExecuteAction(TrafficContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            InvokeInfo vkInfo = ReflectionHelper.GetActionInvokeInfo(context.Request.CommandId);
            if (vkInfo == null)
                return null;
            var controller = vkInfo.Instance as IMyController;
            controller.UserData = context.Request.UserData;
            // 调用方法
            object result = ExecuteActionInternal(context, vkInfo);
            // 设置OutputCache
            // 处理方法的返回结果
            IActionResult executeResult = result as IActionResult;
            if (executeResult != null)
            {
                executeResult.Ouput(context);
                return executeResult;
            }
            else
            {
                return result;
            }
        }

        internal static object ExecuteActionInternal(TrafficContext context, InvokeInfo info)
        {
            // 准备要传给调用方法的参数
            object[] parameters = GetActionCallParameters(context, info.Action);

            // 调用方法
            if (info.Action.HasReturn)
                return info.Action.MethodInfo.Invoke(info.Instance, parameters);

            else
            {
                info.Action.MethodInfo.Invoke(info.Instance, parameters);
                return null;
            }
        }

        private static object[] GetActionCallParameters(TrafficContext context, ActionDescription action)
        {
            if (action.Parameters == null || action.Parameters.Length == 0)
                return null;
            object[] parameters = new object[action.Parameters.Length];
            for (int i = 0; i < action.Parameters.Length; i++)
            {
                ParameterInfo p = action.Parameters[i];
                if (p.IsOut)
                    continue;
                if (p.ParameterType == typeof(VoidType))
                    continue;
                Type paramterType = p.ParameterType.GetRealType();

                object val = context.Request.GetValue(i, paramterType);
                if (val != null)
                    parameters[i] = val;
                else
                {
                    if (p.ParameterType.IsValueType && p.ParameterType.IsNullableType() == false)
                        throw new ArgumentException("未能找到指定的参数值：" + p.Name);
                }
            }
            return parameters;
        }
    }
}
