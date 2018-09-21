using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class EProjectTeam
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int UserID { get; set; }
        [NotDataField]
        public string UserName { get; set; }
        [NotDataField]
        public string UserFace { get; set; }
    }
}
