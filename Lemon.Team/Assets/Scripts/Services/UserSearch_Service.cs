
using Lemon.Team.Entity;
using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class UserSearch_Service
    {
        public static void ListMy(Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EUserSearch>>("UserSearch/ListMy", callBack);
        }

        public static void SetDefault(int id, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<string>("UserSearch/SetDefault", callBack, id);
        }

        public static void GetDefault(Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EUserSearch>("UserSearch/GetDefault", callBack);
        }

        public static void GetByID(int id, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EUserSearch>("UserSearch/GetByID", callBack, id);
        }
    }
}
