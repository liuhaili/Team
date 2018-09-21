
using Lemon.Team.Entity;
using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class TaskTransfer_Service
    {
        public static void ListByTaskID(int taskID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<ETaskTransfer>>("TaskTransfer/ListByTaskID", callBack, taskID);
        }

        public static void ListByUserID(int userID, int pageIndex, int pageSize, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<ETaskTransfer>>("TaskTransfer/ListByUserID", callBack, userID, pageIndex, pageSize);
        }
    }
}
