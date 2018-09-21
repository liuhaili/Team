
using Lemon.Team.Entity;
using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class Message_Service
    {
        public static void ListByTargetID(MessageType type, int targetID, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<EMessage>>("Message/ListByTargetID", callBack, type, targetID);
        }

        public static void SendMessage(EMessage msg, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<string>("Message/SendMessage", callBack, msg);
        }

        public static void ListMyTips(Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<ENewTips>>("Message/ListMyTips", callBack);
        }

        public static void ClearMyTips(MessageType type, int ownerid, Action<ServiceReturn> callBack)
        {
            App.Instance.CallWebApi<List<ENewTips>>("Message/ClearMyTips", callBack, type, ownerid);
        }
    }
}
