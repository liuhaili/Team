using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class EPeople
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PeopleID { get; set; }
        [NotDataField]
        public string PeopleName { get; set; }
        [NotDataField]
        public string PeopleFace { get; set; }
        public PeopleState State { get; set; }
    }
}
