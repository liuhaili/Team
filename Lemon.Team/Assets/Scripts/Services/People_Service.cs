
using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class People_Service
    {
        public static void Create(EPeople obj, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EUser>>("People/Create", callBack, obj);
        }

        public static void ListMyPeople(Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EPeople>>("People/ListMyPeople", callBack);
        }

        public static void GetMyOnePeople(int peopleid, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EPeople>("People/GetMyOnePeople", callBack, peopleid);
        }

        public static void AskConnect(int userid, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<string>("People/AskConnect", callBack, userid);
        }

        public static void ConfirmConnect(int pid, bool connect, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<string>("People/ConfirmConnect", callBack, pid, connect);
        }

        public static void DisConnect(int pid, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<string>("People/DisConnect", callBack, pid);
        }
    }
}
