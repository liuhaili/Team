using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Core.CacheDB
{
    /// <summary>
    /// 区域数据
    /// </summary>
    internal class DBArea
    {
        public string Key { get; set; }
        private Dictionary<Type, DBTable> Tables { get; set; }
        private readonly object tablesLock = new object();
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime DeadTime { get; set; }

        public DBArea(string key)
        {
            Key = key;
            DeadTime = DateTime.Now.AddYears(100);
            Tables = new Dictionary<Type, DBTable>();
        }

        /// <summary>
        /// 加载区域下的某个类型的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        public void Load(DBTable table)
        {
            lock (tablesLock)
            {
                Type t = table.ObjectType;
                if (!Tables.ContainsKey(t))
                    Tables.Add(t, table);
                else
                    Tables[t] = table;
            }
        }

        /// <summary>
        /// 获取区域下某个类型的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DBTable GetTable(Type type)
        {
            lock (tablesLock)
            {
                return Tables[type] as DBTable;
            }
        }

        public List<Type> GetAllType()
        {
            lock (tablesLock)
            {
                return new List<Type>(Tables.Keys);
            }
        }
    }
}
