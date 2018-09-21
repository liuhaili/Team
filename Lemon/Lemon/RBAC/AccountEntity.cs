using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.RBAC
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    public class AccountEntity
    {        
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        public virtual string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public virtual string ConfirmPassword { get; set; }
        /// <summary>
        /// 安全邮箱
        /// </summary>
        public virtual string SafeEmail { get; set; }
        /// <summary>
        /// 接收信息的邮箱
        /// </summary>
        public virtual string MessageEmail { get; set; }
        /// <summary>
        /// 角色编号，适用于简单角色
        /// </summary>
        public virtual int RoleId { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public virtual bool IsOnline { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public virtual int LoginCount { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public virtual DateTime LastLoginTime { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual int State { get; set; }
    }
}
