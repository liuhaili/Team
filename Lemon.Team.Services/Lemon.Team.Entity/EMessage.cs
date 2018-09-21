using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class EMessage
    {
        public int ID { get; set; }
        public MessageType Type { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsRead { get; set; }

        [NotDataField]
        public string SenderName { get; set; }
        [NotDataField]
        public string SenderFace { get; set; }
    }
}
