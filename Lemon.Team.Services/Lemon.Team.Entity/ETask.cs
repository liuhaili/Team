using Lemon.Team.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    public class ETask
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int PlanID { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskType Type { get; set; }
        public int State { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 当前执行人
        /// </summary>
        public int ExecutorID { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public int TaskHeadID { get; set; }
        public string Attachment { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreaterID { get; set; }
        public bool IsComplated { get; set; }
        /// <summary>
        /// 提醒的设置
        /// </summary>
        public string Remind { get; set; }
        public bool IsReminded { get; set; }

        [NotDataField]
        public string TaskHeadName { get; set; }
        [NotDataField]
        public string TaskHeadFace { get; set; }
        [NotDataField]
        public string ExecutorName { get; set; }
        [NotDataField]
        public string ExecutorFace { get; set; }
        [NotDataField]
        public string StepName { get; set; }

        public ETask()
        {
            State = 0;
            Priority = Lemon.Team.Entity.Enum.TaskPriority.Normal;
            BeginTime = System.DateTime.Now;
            EndTime = System.DateTime.Now;
        }
    }
}
