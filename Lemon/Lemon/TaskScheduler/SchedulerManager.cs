using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Lemon.TaskScheduler
{
    /// <summary>
    /// 调度管理
    /// </summary>
    public class SchedulerManager : SingletonBase<SchedulerManager>
    {
        List<Plan> planList = new List<Plan>();
        readonly object planListLock = new object();
        Action<Exception> errorEvent = null;
        Timer timer = null;

        public void Start()
        {
            //启动计划
            timer = new System.Timers.Timer(1000);  //1秒钟执行一次
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerTick);
            timer.AutoReset = true;
            timer.Enabled = true;
            Console.WriteLine("计划任务已开启");
            timer.Start();
        }

        public void Stop()
        {
            if (timer != null)
                timer.Close();
        }

        public void Regist(Plan plan)
        {
            lock (planListLock)
            {
                planList.Add(plan);
            }
        }

        public void SetErrorEvent(Action<Exception> errorevent)
        {
            errorEvent = errorevent;
        }

        public void TimerTick(object source, System.Timers.ElapsedEventArgs e)
        {
            lock (planListLock)
            {
                foreach (Plan p in planList)
                {
                    try
                    {
                        p.Excute(null);
                    }
                    catch (Exception ex)
                    {
                        if (errorEvent != null)
                            errorEvent(ex);
                    }
                }
            }
        }
    }
}
