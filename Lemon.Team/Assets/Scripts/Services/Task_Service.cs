
using Lemon.Team.Entity;
using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class Task_Service
    {
        public static void Get(int taskID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<ETask>("Task/Get", callBack, taskID);
        }

        public static void TaskHandle(int taskID, int excuterID, string note, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<int>("Task/TaskHandle", callBack, taskID, excuterID, note);
        }

        public static void TaskProcess(int taskID, int excuterID,int state, string note, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<int>("Task/TaskProcess", callBack, taskID, excuterID, state, note);
        }

        public static void SetComplated(int taskID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<int>("Task/SetComplated", callBack, taskID);
        }

        public static void ListByPlanID(int planID, int handleID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<ETask>>("Task/ListByPlanID", callBack, planID, handleID);
        }

        public static void ListMyHomeTask(int projectID, int planID, int taskState, TaskPriority? taskPriority, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<ETask>>("Task/ListMyHomeTask", callBack, projectID, planID, taskState, taskPriority);
        }
    }
}
