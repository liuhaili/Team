using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lemon.Core.CacheDB
{
    /// <summary>
    /// 一个数据库
    /// </summary>
    internal class DB
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区域数据
        /// </summary>
        private Dictionary<string, DBArea> areaData;

        private readonly object arealistLock = new object();

        private System.Threading.Thread t;

        private Action<string, string> OnAreaDataTimeOutEvent;

        public DB(string name, Action<string, string> onAreaDataTimeOutEvent)
        {
            this.Name = name;
            OnAreaDataTimeOutEvent = onAreaDataTimeOutEvent;
            areaData = new Dictionary<string, DBArea>();

            t = new Thread(Run);
            t.IsBackground = true;
            t.Start();
        }

        public void SetAreaDBTimeOutEvent(Action<string, string> onAreaDataTimeOutEvent)
        {
            OnAreaDataTimeOutEvent = onAreaDataTimeOutEvent;
        }

        public void SetAreaDBTimeOutTime(string areakey, DateTime timeout)
        {
            lock (arealistLock)
            {
                if (!areaData.ContainsKey(areakey))
                    return;
                areaData[areakey].DeadTime = timeout;
            }
        }

        /// <summary>
        /// 每1分钟会对缓存用户做一次检测
        /// </summary>
        private void Run()
        {
            while (true)
            {
                lock (arealistLock)
                {
                    foreach (DBArea area in areaData.Values)
                    {
                        //如果缓存超时将卸载缓存数据
                        if (DateTime.Now >= area.DeadTime)
                        {
                            if (OnAreaDataTimeOutEvent != null)
                                OnAreaDataTimeOutEvent(Name, area.Key);//执行卸载缓存事件
                            areaData.Remove(area.Key);
                            Console.WriteLine("区域数据" + area.Key + "被清除出缓存...");
                            break;
                        }
                    }
                }
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (t != null)
                t.Abort();
        }

        /// <summary>
        /// 加载区域数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="areakey"></param>
        /// <param name="data"></param>
        public void LoadAreaData(string areakey, Type type, List<object> data)
        {
            lock (arealistLock)
            {
                DBTable dbtable = new DBTable(type);
                dbtable.Load(data);

                if (!areaData.ContainsKey(areakey))
                    areaData.Add(areakey, new DBArea(areakey));
                areaData[areakey].Load(dbtable);
            }
        }

        public bool HaveArea(string areakey)
        {
            lock (arealistLock)
            {
                if (!areaData.ContainsKey(areakey))
                    return false;
                return true;
            }
        }

        /// <summary>
        /// 获得区域下的某个类型的表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="areakey"></param>
        /// <returns></returns>
        public DBTable GetAreaData(string areakey, Type type)
        {
            lock (arealistLock)
            {
                if (!areaData.ContainsKey(areakey))
                {
                    DBArea area = new DBArea(areakey);
                    area.Load(new DBTable(type));
                    areaData.Add(areakey, area);
                }
                return areaData[areakey].GetTable(type);
            }
        }

        public List<Type> GetAreaAllType(string areakey)
        {
            lock (arealistLock)
            {
                if (!areaData.ContainsKey(areakey))
                    return new List<Type>();
                return areaData[areakey].GetAllType();
            }
        }

        public void ClearAllArea()
        {
            lock (arealistLock)
            {
                areaData.Clear();
            }
        }
    }
}
