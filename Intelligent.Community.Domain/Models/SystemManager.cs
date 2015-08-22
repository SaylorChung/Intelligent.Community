using Intelligent.Community.Domain.Enums;
using Saylor;
using System;
using System.Collections.Generic;

namespace Intelligent.Community.Domain.Models
{
    /// <summary>
    ///     标示“员工”的领域模型
    /// </summary>
    public class Employee : IEntity
    {
        public Employee()
        {
            this.Menus = new HashSet<Menu>();
        }
        /// <summary>
        ///     uniq-identifier
        /// </summary>
        public virtual Guid ID { get; set; }
        /// <summary>
        ///     用户名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        ///     真实姓名
        /// </summary>
        public virtual string RealName { get; set; }
        /// <summary>
        ///     密码
        /// </summary>
        public virtual string Password { get; set; }
        /// <summary>
        ///     手机
        /// </summary>
        public virtual string Mobile { get; set; }
        /// <summary>
        ///     邮箱
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        ///     状态
        /// </summary>
        public virtual Status Status { get; set; }
        /// <summary>
        ///     备注
        /// </summary>
        public virtual string Memo { get; set; }
        /// <summary>
        ///     是否匿名用户
        /// </summary>
        public virtual bool IsAnonymous { get; set; }
        /// <summary>
        ///     关联菜单集合
        /// </summary>
        public virtual ICollection<Menu> Menus { get; set; }
    }

    /// <summary>
    ///     标识“菜单”的领域模型
    /// </summary>
    public class Menu : IEntity
    {
        public Menu()
        {
            this.Employees = new HashSet<Employee>();
        }
        /// <summary>
        ///     uniq-identifier
        /// </summary>
        public virtual Guid ID { get; set; }
        /// <summary>
        ///     资源名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        ///     资源图标
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        ///     上级节点
        /// </summary>
        public virtual Guid IdParent { get; set; }
        /// <summary>
        ///     是否模块
        /// </summary>
        public virtual bool IsModule { get; set; }
        /// <summary>
        ///     标示排序
        /// </summary>
        public virtual int Seq { get; set; }
        /// <summary>
        ///     是否叶子节点
        /// </summary>
        public virtual bool IsLeaf { get; set; }
        /// <summary>
        ///     资源路径
        /// </summary>
        public virtual string LinkPath { get; set; }
        /// <summary>
        ///     状态
        /// </summary>
        public virtual Status Stauts { get; set; }
        /// <summary>
        ///     关联用户集合
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
