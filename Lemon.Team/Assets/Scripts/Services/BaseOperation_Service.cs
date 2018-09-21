using Lemon.Team.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Lemon.Extensions;

namespace Assets.Scripts.Services
{
    public class BaseOperation_Service
    {
        public static void Create<T>(T obj, Action<ServiceReturn> callBack) where T : class
        {
            App.Instance.CallWebApi<T>("BaseOperation/Create", callBack, typeof(T).ToString(), obj);
        }

        public static void Change<T>(T obj, Action<ServiceReturn> callBack) where T : class
        {
            App.Instance.CallWebApi<T>("BaseOperation/Change", callBack, typeof(T).ToString(), obj);
        }

        public static void Delete<T>(T obj, Action<ServiceReturn> callBack) where T : class
        {
            App.Instance.CallWebApi<int>("BaseOperation/Delete", callBack, typeof(T).ToString(), obj.GetProperty("ID"));
        }

        public static void Delete<T>(int id, Action<ServiceReturn> callBack) where T : class
        {
            App.Instance.CallWebApi<int>("BaseOperation/Delete", callBack, typeof(T).ToString(), id);
        }

        public static void Get<T>(int id, Action<ServiceReturn> callBack) where T : class
        {
            App.Instance.CallWebApi<T>("BaseOperation/Get", callBack, typeof(T).ToString(), id);
        }
    }
}
