using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.InvokeRoute
{
    /// <summary>
    /// 调用入口
    /// 1.注册所有
    /// 2.通过命令id调用方法
    /// </summary>
    public class InvokeRouteHelper
    {
        public static void Init(string assemblyname)
        {
            if (String.IsNullOrEmpty(assemblyname))
                return;
            ReflectionHelper.InitControllers(assemblyname);
        }

        public static void RegistController(Type t)
        {
            ReflectionHelper.RegistController(t);
        }

        public static object ExecuteAction(string commandid, object userdata, IDataProvider dataProvider)
        {
            return ActionExecutor.ExecuteAction(new TrafficContext(new TrafficRequest(commandid, userdata,dataProvider)));
        }

        public static object ExecuteAction(string commandid, object userdata, params object[] pars)
        {
            return ActionExecutor.ExecuteAction(new TrafficContext(new TrafficRequest(commandid, userdata, pars)));
        }
    }
}
