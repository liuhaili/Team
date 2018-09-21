using Lemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Core.CacheDB
{
    public class DBTableIDManager : SingletonBase<DBTableIDManager>
    {
        Dictionary<Type, int> tableids = new Dictionary<Type, int>();
        readonly object tableidslock = new object();

        public void LoadIds(Dictionary<Type, int> tids)
        {
            lock (tableidslock)
            {
                tableids.Clear();
                tableids = tids;
            }
        }

        public int GetNextId<T>() where T : class
        {
            lock (tableidslock)
            {
                if (!tableids.ContainsKey(typeof(T)))
                    return 0;
                return ++tableids[typeof(T)];
            }
        }

        public int GetNextId(Type type)
        {
            lock (tableidslock)
            {
                if (!tableids.ContainsKey(type))
                    return 0;
                return ++tableids[type];
            }
        }
    }
}
