using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Lemon.InvokeRoute
{
    public class NotificationCenter
    {
        static Dictionary<string, List<KeyValuePair<object, string>>> methodMap = new Dictionary<string, List<KeyValuePair<object, string>>>();
        static readonly object maplock = new object();

        public static void RegistMethod(string name, object receiver, string methodname)
        {
            lock (maplock)
            {
                if (methodMap.ContainsKey(name))
                {
                    if (methodMap[name].Any(c => c.Key == receiver && c.Value == methodname))
                        return;
                    methodMap[name].Add(new KeyValuePair<object, string>(receiver, methodname));
                }
                else
                {
                    List<KeyValuePair<object, string>> methodlist = new List<KeyValuePair<object, string>>();
                    methodlist.Add(new KeyValuePair<object, string>(receiver, methodname));
                    methodMap.Add(name, methodlist);
                }
            }
        }

        public static void ExecuteMethod(string name, params object[] pars)
        {
            lock (maplock)
            {
                if (!methodMap.ContainsKey(name))
                    return;
                foreach (KeyValuePair<object, string> method in methodMap[name])
                {
                    foreach (MethodInfo m in method.Key.GetType().GetMethods())
                    {
                        if (m.Name.ToLower() == method.Value.ToLower())
                        {
                            m.Invoke(method.Key, pars);
                        }
                    }
                }
            }
        }

        public static void UnloadMethod(object receiver)
        {
            lock (maplock)
            {
                foreach (var mlist in methodMap.Values)
                {
                    mlist.RemoveAll(c => c.Key == receiver);
                }
            }
        }

        public static void UnloadMethod(object receiver, string methodname)
        {
            lock (maplock)
            {
                foreach (var mlist in methodMap.Values)
                {
                    mlist.RemoveAll(c => c.Key == receiver && c.Value == methodname);
                }
            }
        }

        public static int CountMethod(string name)
        {
            lock (maplock)
            {
                if (!methodMap.ContainsKey(name))
                    return 0;
                return methodMap[name].Count;
            }
        }

        public static int CountMethod(object receiver)
        {
            lock (maplock)
            {
                int totalcount = 0;
                foreach (var mlist in methodMap.Values)
                {
                    totalcount += mlist.Count(c => c.Key == receiver);
                }
                return totalcount;
            }
        }
    }
}
