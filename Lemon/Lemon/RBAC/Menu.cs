using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.RBAC
{
    /// <summary>
    /// 菜单项
    /// </summary>
    public class Menu
    {        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单域
        /// </summary>
        public string DN { get; set; }
        /// <summary>
        /// 功能号
        /// </summary>
        public string Privilege { get; set; }
        
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; } 
        /// <summary>
        /// 是否首页显示
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 菜单的级别
        /// </summary>
        public int Level
        {
            get
            {
                if (String.IsNullOrEmpty(DN) || DN.Trim().Length<=4)
                    return 0;
                return DN.Trim().ToUpper().Substring(4).Split('M').Length-1;
            }
        }
        /// <summary>
        /// 父级菜单
        /// </summary>
        public Menu Parent { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public IList<Menu> Children { get; set; }

        public IList<ActionEntity> ActionList { get; set; }

        public Menu()
        {           
            Children = new List<Menu>();
            ActionList = new List<ActionEntity>();
        }
    }
}
