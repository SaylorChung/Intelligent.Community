using Intelligent.Community.DataObjects;
using Intelligent.Community.Infrastructure;
using Saylor;
using Saylor.Storage;
using System;
using System.Collections.Generic;

namespace Intelligent.Community.Application
{
    /// <summary>
    ///     表示与“系统”相关的应用层服务
    /// </summary>
    public interface ISystemService
    {
        /// <summary>
        ///     获取登录用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        EmployeeDataObject GetLoginEmployee(string userName, string password);
        /// <summary>
        ///     根据ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EmployeeDataObject GetEmployeeByID(Guid id);
        /// <summary>
        ///     分页获取所有的用户信息。
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        //[Cache(ExpireTime = 60)]
        PagedResult<EmployeeDataObject> GetAllEmployee(string sort, SortOrder sortOrder, int page, int rows, EmployeeDataObject employee);
        /// <summary>
        ///     新增用户信息
        /// </summary>
        /// <param name="obj"></param>
        void InsertEmployee(EmployeeDataObject obj);
        /// <summary>
        ///     删除用户信息
        /// </summary>
        /// <param name="id"></param>
        void DeleteEmployee(Guid id);
        /// <summary>
        ///     更新用户信息
        /// </summary>
        /// <param name="obj"></param>
        void UpdateEmployee(EmployeeDataObject obj);
        /// <summary>
        ///     根据url获取菜单信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        MenuDataObject GetMenuByUrl(string url);
        /// <summary>
        ///     获取菜单以及递归的所有父级菜单
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        List<MenuDataObject> GetAllMenusByUrl(string url);
    }
}
