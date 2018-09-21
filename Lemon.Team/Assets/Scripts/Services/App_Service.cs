using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class App_Service
    {
        public static void StartData(Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EUser>>("App/StartData", callBack);
        }
    }
}
