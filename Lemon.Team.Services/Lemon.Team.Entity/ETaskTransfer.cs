using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class ETaskTransfer
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        /// <summary>
        /// 当前执行人
        /// </summary>
        public int AppointPersonID { get; set; }
        /// <summary>
        /// 转派
        /// </summary>
        public int AssignedPersonID { get; set; }
        public int ToState { get; set; }
        public DateTime CreateTime { get; set; }
        public string Note { get; set; }

        [NotDataField]
        public string TaskName { get; set; }

        [NotDataField]
        public string AppointName { get; set; }
        [NotDataField]
        public string AppointFace { get; set; }

        [NotDataField]
        public string AssignedName { get; set; }
        [NotDataField]
        public string AssignedFace { get; set; }

        [NotDataField]
        public string StepName { get; set; }
    }
}
