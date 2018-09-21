using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Lemon.InvokeRoute
{
    public enum RequestDataType
    {
        /// <summary>
        /// 提供获取数据的接口
        /// </summary>
        UserIProvider=1,
        /// <summary>
        /// 直接给到数据
        /// </summary>
        GiveData=2
    }

    public class TrafficRequest
    {
        public string CommandId { get; set; }
        public object UserData { get; set; }

        private IDataProvider dataProvider;
        List<object> parsList = new List<object>();
        RequestDataType giveDataType;
        public TrafficRequest(string commandid,object udata, IDataProvider dataprovider)
        {
            giveDataType = RequestDataType.UserIProvider;
            CommandId = commandid;
            dataProvider = dataprovider;
            UserData = udata;
        }

        public TrafficRequest(string commandid, object udata, params object[] pars)
        {
            giveDataType = RequestDataType.GiveData;
            CommandId = commandid;
            parsList.Clear();
            foreach (var p in pars)
            {
                parsList.Add(p);
            }
            UserData = udata;
        }

        public object GetValue(int index,Type type)
        {
            if (giveDataType == RequestDataType.UserIProvider)
            {
                return dataProvider.GetBody(index, type);
            }
            else
            {
                if(index>=parsList.Count)
                    return null;
                return (Type)parsList[index];
            }
        }
    }
}
