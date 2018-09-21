using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Team.Entity.Enum
{
    /// <summary>
    /// 任务优先级
    /// </summary>
    public enum TaskPriority
    {
        Normal = 1,
        /// <summary>
        /// 优先
        /// </summary>
        First = 2,
        /// <summary>
        /// 很急
        /// </summary>
        Hurry = 3,
        /// <summary>
        /// 紧急
        /// </summary>
        Urgent = 4
    }
}
