using System;
using System.Collections.Generic;

namespace Intelligent.Community.DataObjects
{

    public class EmployeeDataObject
    {
        public EmployeeDataObject()
        {
            this.Menus = new HashSet<MenuDataObject>();
        }
        /// <summary>
        ///     uniq-identifier
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        ///     用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        ///     真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        ///     手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        ///     邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        ///     状态
        /// </summary>
        public StatusDataObject Status { get; set; }
        /// <summary>
        ///     备注
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        ///     是否匿名用户
        /// </summary>
        public bool IsAnonymous { get; set; }
        /// <summary>
        ///     关联菜单集合
        /// </summary>
        public ICollection<MenuDataObject> Menus { get; set; }
    }

    public class MenuDataObject
    {
        public MenuDataObject()
        {
            this.Employees = new HashSet<EmployeeDataObject>();
        }
        /// <summary>
        ///     uniq-identifier
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        ///     资源名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     资源图标
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        ///     上级节点
        /// </summary>
        public Guid IdParent { get; set; }
        /// <summary>
        ///     是否模块
        /// </summary>
        public bool IsModule { get; set; }
        /// <summary>
        ///     标示排序
        /// </summary>
        public int Seq { get; set; }
        /// <summary>
        ///     是否叶子节点
        /// </summary>
        public bool IsLeaf { get; set; }
        /// <summary>
        ///     资源路径
        /// </summary>
        public string LinkPath { get; set; }
        /// <summary>
        ///     状态
        /// </summary>
        public StatusDataObject Stauts { get; set; }
        /// <summary>
        ///     关联用户集合
        /// </summary>
        public ICollection<EmployeeDataObject> Employees { get; set; }
    }

}
