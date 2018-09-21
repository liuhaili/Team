
using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class Plan_Service
    {
        public static void ListMyProjectID(int projectID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EPlan>>("Plan/ListMyProjectID", callBack, projectID);
        }

        public static void GetPlan(int projectID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EPlan>("Plan/GetPlan", callBack, projectID);
        }
    }
}
