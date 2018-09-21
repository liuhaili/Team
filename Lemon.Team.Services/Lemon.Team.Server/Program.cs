using Lemon.Team.Controller.Logic;
using Lemon.Team.DAL;
using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Team.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("服务开始");
            DBBase.ConnectStr = "Server=60.205.210.198;Database=team;uid=root;pwd=team119505;";
            // 在应用程序启动时运行的代码 这里设置34个小时间隔 122400000 300000
            System.Timers.Timer myTimer = new System.Timers.Timer(1000 * 10);//修改时间间隔
            //关联事件
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(SendRemind);
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
            Console.Read();
        }


        static void SendRemind(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<ETask> taskList = DBBase.QueryCustom<ETask>("select * from task where IsReminded=0 and Remind<>'' and Remind is not null");
            foreach (var t in taskList)
            {
                try
                {
                    DateTime remindTime = t.BeginTime.Date.AddHours(int.Parse(t.Remind));
                    if (DateTime.Now > remindTime)
                    {
                        t.IsReminded = true;
                        DBBase.Change(t);

                        EUser excuteuser = DBBase.Get<EUser>(t.ExecutorID);
                        List<string> ulist = new List<string>();
                        ulist.Add(excuteuser.PushClientID);
                        PushMessageToList.PushToList("你有一个任务需要办理", t.Title, ulist, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(t.Title + " 任务提醒出错" + ex.StackTrace);
                }
            }
        }
    }
}
