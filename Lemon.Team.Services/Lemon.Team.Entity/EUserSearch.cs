using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class EUserSearch
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public int PlanID { get; set; }
        public int TaskState { get; set; }
        public int TaskPriority { get; set; }
        public bool IsDefault { get; set; }

        [NotDataField]
        public string ProjectName { get; set; }
        [NotDataField]
        public string PlanName { get; set; }
        [NotDataField]
        public string StepName { get; set; }
    }
}
