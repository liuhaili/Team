using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class Configure_Service
    {
        public static void GetValue(string key, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EConfigure>("Configure/GetValue", callBack, key);
        }
    }
}
