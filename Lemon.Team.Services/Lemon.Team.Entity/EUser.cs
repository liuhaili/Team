using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class EUser
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Face { get; set; }
        public string Password { get; set; }
        public string OpenID { get; set; }
        public string PushClientID { get; set; }
    }
}
