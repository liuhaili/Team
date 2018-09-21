using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class ENewTips
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public MessageType Type { get; set; }
        public int OwnerID { get; set; }
    }
}
