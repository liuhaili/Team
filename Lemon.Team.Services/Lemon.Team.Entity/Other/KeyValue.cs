using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class KeyValue
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public KeyValue() { }
        public KeyValue(string key, object value) { Key = key; Value = value; }
    }
}
