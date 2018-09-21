using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Lemon.Core.Helper
{
    public class EnumHelper<T>
    {
        private Dictionary<T, string> _EnumMap = null;

        public EnumHelper()
        {
            if (_EnumMap == null)
            {
                _EnumMap = new Dictionary<T, string>();
                foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                    if (attrs == null || attrs.Length == 0)
                        continue;
                    _EnumMap.Add((T)field.GetValue(null), attrs[0].Description);
                }
            }
        }

        public string EnumToString(T enumValue)
        {
            return _EnumMap.ContainsKey(enumValue) ? _EnumMap[enumValue] : null;
        }

        public T EnumFromString(string enumString)
        {
            if (enumString != null)
            {
                foreach (var keyValue in _EnumMap)
                {
                    if (keyValue.Value == enumString)
                        return keyValue.Key;
                }
            }
            return default(T);
        }
    }

    public static class EnumHelper
    {
        private static Dictionary<Type, object> _EnumMap = new Dictionary<Type, object>();

        public static string EnumToString<T>(T enumValue)
        {
            var type = typeof(T);
            if (!_EnumMap.ContainsKey(type))
            {
                _EnumMap.Add(type, new EnumHelper<T>());
            }

            return ((EnumHelper<T>)_EnumMap[type]).EnumToString(enumValue);
        }

        public static T EnumFromString<T>(string enumString)
        {
            var type = typeof(T);
            if (!_EnumMap.ContainsKey(type))
            {
                _EnumMap.Add(type, new EnumHelper<T>());
            }

            return ((EnumHelper<T>)_EnumMap[type]).EnumFromString(enumString);
        }

        /// <summary>
        /// 枚举缓存池
        /// </summary>
        private static Dictionary<string, Dictionary<int, string>> _EnumList = new Dictionary<string, Dictionary<int, string>>();

        /// <summary>
        /// 装载枚举的值
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToDictionary(Type enumType)
        {
            string keyName = enumType.FullName;

            Dictionary<int, string> list = new Dictionary<int, string>();

            foreach (int i in Enum.GetValues(enumType))
            {
                string name = Enum.GetName(enumType, i);

                //取显示名称
                string showName = string.Empty;
                object[] atts = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (atts.Length > 0) showName = ((DescriptionAttribute)atts[0]).Description;

                list.Add(i, string.IsNullOrEmpty(showName) ? name : showName);
            }

            object syncObj = new object();

            if (!_EnumList.ContainsKey(keyName))
            {
                lock (syncObj)
                {
                    if (!_EnumList.ContainsKey(keyName))
                    {
                        _EnumList.Add(keyName, list);
                    }
                }
            }

            return _EnumList[keyName];
        }
    }
}
