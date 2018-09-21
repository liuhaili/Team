using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.ProtocolBuffers;
using com.gexin.rp.sdk.dto;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using System.Net;

namespace Lemon.Team.Controller.Logic
{
    public class PushMessageToList
    {
        //采用"C# SDK 快速入门"， "第二步 获取访问凭证 "中获得的应用配置,用户可自行替换
        private static String APPID = "vHs9i04tthA6snIsc9aQb1";
        private static String APPKEY = "ddl3e4fgNM6exylUbDC5p7";
        private static String MASTERSECRET = "USK8q7oFvYAtvfsvnC0H8A";
        //别名推送方式
        //private static String ALIAS1 = "";  
        //private static String ALIAS2 = "";
        //HOST：OpenService接口地址
        private static String HOST = "http://sdk.open.api.igexin.com/apiex.htm";

        //static void Main()
        //{
        //    //toList接口每个用户状态返回是否开启，可选
        //    Console.OutputEncoding = Encoding.GetEncoding(936);
        //    Environment.SetEnvironmentVariable("needDetails", "true");

        //    //2.PushMessageToList接口
        //    PushToList();
        //}


        //PushMessageToList接口测试代码
        public static void PushToList(string title, string content, List<string> clientList,bool istz)
        {
            // 推送主类（方式1，不可与方式2共存）
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
            // 推送主类（方式2，不可与方式1共存）此方式可通过获取服务端地址列表判断最快域名后进行消息推送，每10分钟检查一次最快域名
            //IGtPush push = new IGtPush("",APPKEY,MASTERSECRET);
            ListMessage message = new ListMessage();
            //NotificationTemplate template = NotificationTemplateDemo(title, content);
            if (istz)
            {
                NotificationTemplate template = NotificationTemplateDemo(title, content);
                message.Data = template;
            }
            else
            {
                TransmissionTemplate template = TransmissionTemplateDemo(title, content);
                message.Data = template;
            }
            
            // 用户当前不在线时，是否离线存储,可选
            message.IsOffline = false;
            // 离线有效时间，单位为毫秒，可选
            message.OfflineExpireTime = 1000 * 3600 * 12;
            
            message.PushNetWorkType = 0;        //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为不限制网络环境。
            //设置接收者
            List<com.igetui.api.openservice.igetui.Target> targetList = new List<com.igetui.api.openservice.igetui.Target>();

            foreach (var cid in clientList)
            {
                com.igetui.api.openservice.igetui.Target target1 = new com.igetui.api.openservice.igetui.Target();
                target1.appId = APPID;
                target1.clientId = cid;
                //target1.alias = ALIAS1;
                targetList.Add(target1);
            }

            String contentId = push.getContentId(message);
            String pushResult = push.pushMessageToList(contentId, targetList);
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("服务端返回结果:" + pushResult);
        }

        public static TransmissionTemplate TransmissionTemplateDemo(string title, string content)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            //应用启动类型，1：强制应用启动 2：等待应用启动
            template.TransmissionType = "2";
            //透传内容  
            template.TransmissionContent = title + "|" + content;
            return template;
        }

        //通知透传模板动作内容
        private static NotificationTemplate NotificationTemplateDemo(string title, string content)
        {
            NotificationTemplate template = new NotificationTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            //通知栏标题
            template.Title = title;
            //通知栏内容     
            template.Text = content;
            //通知栏显示本地图片
            template.Logo = "";
            //通知栏显示网络图标
            template.LogoURL = "";
            //应用启动类型，1：强制应用启动  2：等待应用启动
            template.TransmissionType = "1";
            //透传内容  
            template.TransmissionContent = content;
            //接收到消息是否响铃，true：响铃 false：不响铃   
            template.IsRing = true;
            //接收到消息是否震动，true：震动 false：不震动   
            template.IsVibrate = true;
            //接收到消息是否可清除，true：可清除 false：不可清除    
            template.IsClearable = true;
            //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
            //String begin = "2015-03-06 14:36:10";
            //String end = "2015-03-06 14:46:20";
            //template.setDuration(begin, end);

            return template;
        }
    }
}
