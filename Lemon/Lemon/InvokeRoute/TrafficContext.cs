using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.InvokeRoute
{
    public class TrafficContext
    {
        public TrafficRequest Request { get; set; }
        public TrafficResponse Response { get; set; }

        public TrafficContext(TrafficRequest request)
        {
            Request = request;
        }

        public string[] GetParams()
        {
            return null;
        }

        public string GetValue(string name)
        {
            return null;
        }

        public string GetValue(int index)
        {
            return null;
        }

    }
}
