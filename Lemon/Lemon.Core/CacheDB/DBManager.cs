using Lemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lemon.Core.CacheDB
{
    /// <summary>
    /// 内存数据库管理者
    /// 1.需要调用DBTableIDManager.LoadIds加载最大id
    /// 2.需要调用LoadAreaTable加载缓存数据
    /// 3.调用SetSerialization设置序列化工具，可以不设，默认json序列化
    /// 
    /// 注意：如果一个表的数据上万，查询请使用CreateTableIndex，GetObjectByIndex，不然Linq查询会很慢
    /// </summary>
    public class DBManager : SingletonBase<DBManager>
    {
        private const string defaultDBName = "defaultdb";
        private const string defaultBDArea = "defaultArea";
        private readonly object dblistLock = new object();
        private Dictionary<string, DB> dbs = new Dictionary<string, DB>();

        public void SetDBTimeOutEvent(Action<string, string> onAreaDataTimeOutEvent, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                dbs[dbname].SetAreaDBTimeOutEvent(onAreaDataTimeOutEvent);
            }
        }

        public void SetAreaDBTimeOutTime(DateTime timeout, string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    return;
                dbs[dbname].SetAreaDBTimeOutTime(areakey, timeout);
            }
        }

        /// <summary>
        /// 加载一个共享数据表
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="data"></param>
        public void LoadAreaTable(Type type, List<object> data, string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                dbs[dbname].LoadAreaData(areakey, type, data);
            }
        }

        public void ClearArea(string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    return;
                dbs[dbname].ClearAllArea();
            }
        }

        public List<object> GetAreaALLData(Type type, string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    return null;
                DBTable dbtable = dbs[dbname].GetAreaData(areakey, type);
                return dbtable == null ? null : dbtable.GetAll();
            }
        }

        public List<Type> GetAreaAllType(string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    return null;
                return dbs[dbname].GetAreaAllType(areakey);
            }
        }

        public bool HaveAreaTable(string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    return false;
                return dbs[dbname].HaveArea(areakey);
            }
        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbname"></param>
        /// <param name="areakey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public T AddObject<T>(T t, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.Create<T>(t) as T;
            }
        }

        public object AddObject2(object obj, string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, obj.GetType());
                return table.Create(obj);
            }
        }

        /// <summary>
        /// 修改一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbname"></param>
        /// <param name="areakey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public int UpdateObject<T>(T t, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.Update<T>(t);
            }
        }

        public int UpdateObject2(object obj, string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, obj.GetType());
                return table.Update(obj);
            }
        }

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbname"></param>
        /// <param name="areakey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public int DeleteObject<T>(T t, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.Delete<T>(t);
            }
        }

        public int DeleteObject2(Type type, int id, string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, type);
                return table.Delete(id);
            }
        }

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbname"></param>
        /// <param name="areakey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteObject<T>(int id, string areakey=defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.Delete(id);
            }
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbname"></param>
        /// <param name="areakey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetObject<T>(int id, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.Get<T>(id);
            }
        }

        public object GetObject(Type type, int id, string areakey = defaultBDArea, string dbname = defaultDBName)
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, type);
                return table.Get(type, id);
            }
        }

        public void CreateTableIndex<T>(List<string> pname, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                table.CreateIndex(pname);
            }
        }

        public T GetObjectByIndex<T>(List<string> pname, List<string> pvalue, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.GetByIndex<T>(pname, pvalue);
            }
        }

        /// <summary>
        /// 查询对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbname"></param>
        /// <param name="areakey"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> SelectObject<T>(Func<T, bool> predicate = null, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.Select<T>(predicate);
            }
        }

        public int Count<T>(Func<T, bool> predicate = null, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.Count<T>(predicate);
            }
        }

        /// <summary>
        /// 查找对象列表的第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbname"></param>
        /// <param name="areakey"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault<T>(Func<T, bool> predicate = null, string areakey = defaultBDArea, string dbname = defaultDBName) where T : class
        {
            lock (dblistLock)
            {
                if (!dbs.ContainsKey(dbname))
                    dbs.Add(dbname, new DB(dbname, null));
                DBTable table = dbs[dbname].GetAreaData(areakey, typeof(T));
                return table.Select<T>(predicate).FirstOrDefault();
            }
        }
    }
}
