using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity
{
    /// <summary>
    /// 反馈
    /// </summary>
    public class EFeedback
    {
        public int ID { get; set; }
        public int SendUserID { get; set; }
        public string Content { get; set; }
        public DateTime CrateTime { get; set; }
        /// <summary>
        /// 答复
        /// </summary>
        public string Reply { get; set; }
    }
}
