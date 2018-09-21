
using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class ProjectTaskStep_Service
    {
        public static void ListByProjectID(int projectID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EProjectTaskStep>>("ProjectTaskStep/ListByProjectID", callBack, projectID);
        }
    }
}
