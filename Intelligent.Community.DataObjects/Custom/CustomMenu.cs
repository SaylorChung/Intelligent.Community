using System.Collections.Generic;

namespace Intelligent.Community.DataObjects.Custom
{
    /// <summary>
    ///     页面菜单数据传输对象。
    /// </summary>
    public class CustomMenu
    {
        /// <summary>
        ///     员工授权菜单集合
        /// </summary>
        public List<MenuDataObject> EmployeeMenus { get; set; }
        /// <summary>
        ///     页面菜单递归集合
        /// </summary>
        public List<MenuDataObject> PageMenus { get; set; }
    }
}
