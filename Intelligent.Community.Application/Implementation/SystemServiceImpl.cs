using AutoMapper;
using Intelligent.Community.DataObjects;
using Intelligent.Community.Domain.Models;
using Intelligent.Community.Domain.Repositories.Specifications;
using Saylor;
using Saylor.Application;
using Saylor.Repositories;
using Saylor.Specifications;
using Saylor.Storage;
using System;
using System.Collections.Generic;

namespace Intelligent.Community.Application
{
    /// <summary>
    ///     表示与“系统”相关的应用层服务实现
    /// </summary>
    public class SystemServiceImpl : ApplicationService, ISystemService
    {
        #region Private Fields
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Menu> _menuRepository;
        #endregion

        #region Ctor
        public SystemServiceImpl(IRepositoryContext context,
            IRepository<Employee> employeeRepository,
            IRepository<Menu> menuRepository)
            : base(context)
        {
            _employeeRepository = employeeRepository;
            _menuRepository = menuRepository;
        }
        #endregion

        #region ISystemService Members

        #region Employee Methods
        /// <summary>
        ///     获取登录用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public EmployeeDataObject GetLoginEmployee(string userName, string password)
        {
            ISpecification<Employee> specification = new AndSpecification<Employee>(new EmployeeUserNameEqualSpecification(userName),
                new EmployeePasswordEqualSpecification(password));
            
            Employee entity = this._employeeRepository.Find(specification);
            return Mapper.Map<Employee, EmployeeDataObject>(entity);
        }
        /// <summary>
        ///     根据ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeeDataObject GetEmployeeByID(Guid id)
        {
            Employee entity = this._employeeRepository.Find(id, elp => elp.Menus);
            return Mapper.Map<Employee, EmployeeDataObject>(entity);
        }
        /// <summary>
        ///     分页获取所有的用户信息。
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        public PagedResult<EmployeeDataObject> GetAllEmployee(string sort, SortOrder sortOrder, int page, int rows, EmployeeDataObject employee)
        {
            ISpecification<Employee> specification = new EmployeeCombinedQuerySpecifition(employee);

            PagedResult<Employee> pagedResult = this._employeeRepository.FindAll(specification, ExpressionExtension.Sort<Employee>(sort), sortOrder, page, rows);

            if (pagedResult != null)
                return new PagedResult<EmployeeDataObject>(pagedResult.TotalRecords, pagedResult.TotalPages, page, rows,
                Mapper.Map<List<Employee>, List<EmployeeDataObject>>(pagedResult.Data));
            else
                return null;
        }
        /// <summary>
        ///     新增用户信息
        /// </summary>
        /// <param name="obj"></param>
        public void InsertEmployee(EmployeeDataObject obj)
        {
            Employee entity = Mapper.Map<EmployeeDataObject, Employee>(obj);
            this._employeeRepository.Insert(entity);
            this.Context.Commit();
        }
        /// <summary>
        ///     删除用户信息
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEmployee(Guid id)
        {
            Employee entity = this._employeeRepository.Find(id);
            this._employeeRepository.Delete(entity);
            this.Context.Commit();
        }
        /// <summary>
        ///     更新用户信息
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateEmployee(EmployeeDataObject obj)
        {
            Employee entity = Mapper.Map<EmployeeDataObject, Employee>(obj);
            this._employeeRepository.Update(entity);
            this.Context.Commit();
        }
        #endregion

        #region Menu Methods
        /// <summary>
        ///     根据url获取菜单信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MenuDataObject GetMenuByUrl(string url)
        {
            Menu entity = this._menuRepository.Find(Specification<Menu>.Eval(m => m.LinkPath == url));

            return Mapper.Map<Menu, MenuDataObject>(entity);
        }
        /// <summary>
        ///     获取菜单以及递归的所有父级菜单
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<MenuDataObject> GetAllMenusByUrl(string url)
        {
            Menu entity = this._menuRepository.Find(Specification<Menu>.Eval(m => m.LinkPath == url));

            List<Menu> list = new List<Menu>();
            list.Add(entity);
            if (entity != null)
                return Mapper.Map<List<Menu>, List<MenuDataObject>>(RecursiveGetMenus(list, entity.IdParent));
            else
                return null;
        }
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        ///     递归获取菜单以及对应父级菜单
        /// </summary>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        private List<Menu> RecursiveGetMenus(List<Menu> menuList, Guid idParent)
        {
            var entity = this._menuRepository.Find(Specification<Menu>.Eval(m => m.ID == idParent));
            if (entity != null)
            {
                menuList.Add(entity);
            }
            return (entity == null) ? menuList : RecursiveGetMenus(menuList, entity.IdParent);
        }
        #endregion
    }
}
