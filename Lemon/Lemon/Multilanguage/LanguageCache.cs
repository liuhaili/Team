using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Multilanguage
{
    /// <summary>
    /// 语言包的缓存
    /// </summary>
    public class LanguageCache
    {
        /// <summary>
        /// 语言包缓存
        /// </summary>
        private static IList<Language> Cache = new List<Language>();
        /// <summary>
        /// 缓存锁
        /// </summary>
        public static readonly object _lock = new object();
        /// <summary>
        /// 向缓存中添加语言包
        /// </summary>
        /// <param name="language"></param>
        public static void Add(Language language)
        {
            //判断语言包是否存在于缓存,不存在就添加
            var l=Cache.FirstOrDefault(c => c.Name == language.Name);
            if (l == null)
            {
                lock (_lock)
                {
                    Cache.Add(language);
                }
            }
            //判断缓存是否超过缓存的大小，如果超过那么清除超过部分
            if (Cache.Count > 100)
            {
                for (int i = 0; i < (Cache.Count - 100); i++)
                {
                    var temp = Cache[i];
                    Cache.RemoveAt(i);
                    //释放资源
                    temp = null;
                }
            }
        }
        /// <summary>
        /// 从缓存中换取语言包
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Language Get(string name)
        {
            var l = Cache.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            if (l != null)
                return l;
            return null;
        }
    }
}
