
using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class Project_Service
    {
        public static void Create(EProject obj, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EProject>("Project/Create", callBack, obj);
        }

        public static void ListMyProject(Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EProject>>("Project/ListMyProject", callBack);
        }

        public static void ListTeamProject(Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EProject>>("Project/ListTeamProject", callBack);
        }
    }
}
