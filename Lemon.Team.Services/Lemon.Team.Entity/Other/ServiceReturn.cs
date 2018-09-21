using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class ServiceReturn
    {
        public bool IsSucceed { get; set; }
        public List<KeyValue> Data { get; set; }
        public string Message { get; set; }

        public ServiceReturn()
        {
            IsSucceed = false;
            Data = new List<KeyValue>();
        }
        public ServiceReturn(object data)
        {
            Data = new List<KeyValue>();
            SetData(data);
        }

        public void SetData(object data)
        {
            IsSucceed = true;
            if (Data.Any(c => c.Key == "[default]"))
            {
                Data.FirstOrDefault(c => c.Key == "[default]").Value = data;
            }
            else
                AddData("[default]", data);
        }
        public void SetMessage(string message)
        {
            IsSucceed = false;
            Message = message;
        }
        public void AddData(string key, object value)
        {
            IsSucceed = true;
            Data.Add(new KeyValue(key, value));
        }
        public object GetData(string key = null)
        {
            if (Data == null)
                return null;
            KeyValue kv = null;
            if (key == null)
                kv = Data.FirstOrDefault();
            else
                kv = Data.FirstOrDefault(c => c.Key == key);
            if (kv == null)
                return null;
            return kv.Value;
        }
    }
}
