using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon
{
    /// <summary>
    /// 单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonBase<T> where T : class,new()
    {
        private static T _Instance = null;
        private readonly static object _lock = new object();

        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_lock)
                    {
                        if (_Instance == null)
                            _Instance = new T();
                    }
                }
                return _Instance;
            }
        }
    }
}
