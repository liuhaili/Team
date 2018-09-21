using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity.Enum
{
    public class EnumMapping
    {
        static Dictionary<Type, Dictionary<int, string>> EnumMappingDic = new Dictionary<Type, Dictionary<int, string>>();

        static EnumMapping()
        {
            //Dictionary<int, string> taskState = new Dictionary<int, string>();
            //taskState.Add((int)TaskState.New, "新任务");
            //taskState.Add((int)TaskState.Resolved, "已解决");
            //taskState.Add((int)TaskState.Closed, "已关闭");
            //taskState.Add((int)TaskState.NotResolved, "不解决");
            //EnumMappingDic.Add(typeof(TaskState), taskState);

            Dictionary<int, string> taskPriority = new Dictionary<int, string>();
            taskPriority.Add((int)TaskPriority.Normal, "一般");
            taskPriority.Add((int)TaskPriority.First, "优先");
            taskPriority.Add((int)TaskPriority.Hurry, "很急");
            taskPriority.Add((int)TaskPriority.Urgent, "紧急");
            EnumMappingDic.Add(typeof(TaskPriority), taskPriority);
        }

        public static List<string> ListAll<T>()
        {
            if (!EnumMappingDic.ContainsKey(typeof(T)))
                return null;
            return EnumMappingDic[typeof(T)].Values.ToList();
        }

        public static int GetValue<T>(string text)
        {
            if (!EnumMappingDic.ContainsKey(typeof(T)))
                return -1;
            foreach (var kv in EnumMappingDic[typeof(T)])
            {
                if (kv.Value == text)
                    return kv.Key;
            }
            return -1;
        }

        public static string GetText<T>(int key)
        {
            if (!EnumMappingDic.ContainsKey(typeof(T)))
                return null;
            if (!EnumMappingDic[typeof(T)].ContainsKey(key))
                return null;
            return EnumMappingDic[typeof(T)][key];
        }
    }
}
