using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class EProject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Face { get; set; }
        public int CreaterID { get; set; }
        public int Progress { get; set; }
        public ProjectState State { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
