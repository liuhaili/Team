
using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class ProjectTeam_Service
    {
        public static void GetByProjectID(int userID, int projectID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<EProjectTeam>("ProjectTeam/GetByProjectID", callBack, userID, projectID);
        }

        public static void ListByProjectID(int projectID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EProjectTeam>>("ProjectTeam/ListByProjectID", callBack, projectID);
        }
    }
}
