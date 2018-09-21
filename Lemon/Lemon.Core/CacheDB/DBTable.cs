using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Lemon;
using Lemon.Extensions;

namespace Lemon.Core.CacheDB
{
    internal class DBTable
    {
        private readonly object tableLock = new object();
        private const string idname = "ID";
        private Dictionary<int, object> objectList;
        private Dictionary<string, Dictionary<string, object>> indexObjectList;

        public Type ObjectType { get; set; }

        private int simulateId = 0;

        public DBTable(Type objtype)
        {
            ObjectType = objtype;
            objectList = new Dictionary<int, object>();
            indexObjectList = new Dictionary<string, Dictionary<string, object>>();
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="pname"></param>
        public void CreateIndex(List<string> pname)
        {
            lock (tableLock)
            {
                string pnamekey = pname.Aggregate((f, n) => f + "_" + n);
                if (!indexObjectList.ContainsKey(pnamekey))
                    indexObjectList.Add(pnamekey, new Dictionary<string, object>());
                indexObjectList[pnamekey].Clear();
                foreach (var obj in objectList.Values.ToList())
                {
                    AddToIndexList(obj, pname, indexObjectList[pnamekey]);
                }
            }
        }

        public void AddToIndexList(object obj, List<string> pname, Dictionary<string, object> objectList)
        {
            string pvalekey = GetIndexValueKey(obj, pname);
            if (!objectList.ContainsKey(pvalekey))
                objectList.Add(pvalekey, obj);
        }

        private string GetIndexValueKey(object obj, List<string> pname)
        {
            string pvalekey = "";
            foreach (var p in pname)
            {
                pvalekey += obj.GetType().GetProperty(p).GetValue(obj, null).ToString() + "_";
            }
            pvalekey = pvalekey.TrimEnd('_');
            return pvalekey;
        }

        public void Load(List<object> data)
        {
            lock (tableLock)
            {
                objectList.Clear();
                foreach (object d in data)
                {
                    try
                    {
                        if (d.GetProperty(idname) == null)
                        {
                            simulateId++;
                            objectList.Add(simulateId, d);
                        }
                        else
                            objectList.Add(int.Parse(d.GetProperty(idname).ToString()), d);
                    }
                    catch
                    {
                        int idvalue = (int)d.GetProperty(idname);
                        throw new Exception("请检查id" + idvalue + "是否重复！");
                    }
                }
            }
        }

        public List<object> GetAll()
        {
            lock (tableLock)
            {
                List<object> result = objectList.Values.ToList();
                List<object> newt = CopyObj(result) as List<object>;
                return newt;
            }
        }

        public T Create<T>(T t) where T : class
        {
            lock (tableLock)
            {
                T newt = CopyObj(t) as T;

                int idvalue = (int)newt.GetType().GetProperty(idname).GetValue(newt, null);
                if (idvalue == 0)
                {
                    idvalue = DBTableIDManager.Instance.GetNextId<T>();
                    newt.GetType().GetProperty(idname).SetValue(t, idvalue, null);
                }
                objectList.Add(idvalue, newt);
                //为索引表添加
                foreach (var iol in indexObjectList)
                {
                    AddToIndexList(newt, iol.Key.Split('_').ToList(), iol.Value);
                }
                return newt;
            }
        }

        public object Create(object obj)
        {
            lock (tableLock)
            {
                object newt = CopyObj(obj);
                int idvalue = (int)newt.GetType().GetProperty(idname).GetValue(newt, null);
                if (objectList.ContainsKey(idvalue))
                    return objectList[idvalue];
                objectList.Add(idvalue, newt);
                //为索引表添加
                foreach (var iol in indexObjectList)
                {
                    AddToIndexList(newt, iol.Key.Split('_').ToList(), iol.Value);
                }
                return newt;
            }
        }

        public int Update<T>(T t) where T : class
        {
            lock (tableLock)
            {
                int idvalue = (int)t.GetType().GetProperty(idname).GetValue(t, null);
                if (!objectList.ContainsKey(idvalue))
                    return -1;
                var oldobj = objectList[idvalue];
                T newt = CopyObj(t) as T;
                //从索引表中移除对象
                foreach (var iol in indexObjectList)
                {
                    string pvalekey = GetIndexValueKey(oldobj, iol.Key.Split('_').ToList());
                    if (String.IsNullOrEmpty(pvalekey))
                        continue;
                    if (!iol.Value.ContainsKey(pvalekey))
                        continue;
                    iol.Value[pvalekey] = newt;
                }
                objectList[idvalue] = newt;
                return 0;
            }
        }

        public int Update(object obj)
        {
            lock (tableLock)
            {
                int idvalue = (int)obj.GetType().GetProperty(idname).GetValue(obj, null);
                if (!objectList.ContainsKey(idvalue))
                    return -1;
                var oldobj = objectList[idvalue];
                object newt = CopyObj(obj);
                //从索引表中移除对象
                foreach (var iol in indexObjectList)
                {
                    string pvalekey = GetIndexValueKey(oldobj, iol.Key.Split('_').ToList());
                    if (String.IsNullOrEmpty(pvalekey))
                        continue;
                    if (!iol.Value.ContainsKey(pvalekey))
                        continue;
                    iol.Value[pvalekey] = newt;
                }
                objectList[idvalue] = newt;
                return 0;
            }
        }

        public int Delete(int id)
        {
            lock (tableLock)
            {
                if (!objectList.ContainsKey(id))
                    return -1;
                var obj = objectList[id];
                objectList.Remove(id);
                //从索引表中移除对象
                foreach (var iol in indexObjectList)
                {
                    string pvalekey = GetIndexValueKey(obj, iol.Key.Split('_').ToList());
                    if (String.IsNullOrEmpty(pvalekey))
                        continue;
                    if (!iol.Value.ContainsKey(pvalekey))
                        continue;
                    iol.Value.Remove(pvalekey);
                }
                return 0;
            }
        }

        public int Delete<T>(T t) where T : class
        {
            int idvalue = (int)t.GetType().GetProperty(idname).GetValue(t, null);
            return Delete(idvalue);
        }

        public T Get<T>(int id) where T : class
        {
            lock (tableLock)
            {
                if (!objectList.ContainsKey(id))
                    return null;
                T newt = CopyObj(objectList[id]) as T;
                return newt;
            }
        }

        public object Get(Type type, int id)
        {
            lock (tableLock)
            {
                if (!objectList.ContainsKey(id))
                    return null;
                return CopyObj(objectList[id]);
            }
        }

        public T GetByIndex<T>(List<string> pname, List<string> pvalue) where T : class
        {
            lock (tableLock)
            {
                string pnamekey = pname.Aggregate((f, n) => f + "_" + n);
                string pvaluekey = pvalue.Aggregate((f, n) => f + "_" + n);

                if (!indexObjectList.ContainsKey(pnamekey))
                    return null;
                if (!indexObjectList[pnamekey].ContainsKey(pvaluekey))
                    return null;
                T newt = CopyObj(indexObjectList[pnamekey][pvaluekey]) as T;
                return newt;
            }
        }

        public List<T> Select<T>(Func<T, bool> predicate = null) where T : class
        {
            lock (tableLock)
            {
                List<T> result = null;
                if (predicate != null)
                    result = objectList.Values.Select(c => c as T).Where(predicate).ToList<T>();
                else
                    result = objectList.Values.Select(c => c as T).ToList<T>();
                List<T> newt = CopyObj(result) as List<T>;
                return newt;
            }
        }

        public int Count<T>(Func<T, bool> predicate = null) where T : class
        {
            lock (tableLock)
            {
                if (predicate == null)
                    return objectList.Values.Count();
                return objectList.Values.Select(c => c as T).Count(predicate);
            }
        }

        private object CopyObj(object obj)
        {
            //if (DBManager.Instance.SerializeTool == null)
            //    throw new Exception("SerializeTool is NULL,please Set for DBManager");
            //byte[] bs = DBManager.Instance.SerializeTool.Serialize(obj);
            //return DBManager.Instance.SerializeTool.Deserialize(bs, obj.GetType());
            return obj;
        }
    }
}
