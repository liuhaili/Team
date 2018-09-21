
using Lemon.Team.Entity;
using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class User_Service
    {
        public static void Regist(EUser obj, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EUser>("User/Regist", callBack, obj);
        }

        public static void Login(string account, string psw, string pushClientID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EUser>("User/Login", callBack, account, psw, pushClientID);
        }

        public static void PlatformLogin(string openid, string name, string face, string pushClientID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EUser>("User/PlatformLogin", callBack, openid, name, face, pushClientID);
        }

        public static void SearchUser(string searchWord, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EUser>>("User/SearchUser", callBack, searchWord);
        }
    }
}
