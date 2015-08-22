using Intelligent.Community.Application;
using Intelligent.Community.DataObjects;
using Intelligent.Community.DataObjects.Custom;
using Intelligent.Community.Infrastructure;
using Saylor;
using Saylor.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace Intelligent.Community.Services.Controllers
{
    [RoutePrefix("api/system")]
    public class SystemController : ApiControllerBase
    {
        #region Private Properties
        private readonly ISystemService _systemServiceImpl;
        #endregion

        #region Ctor
        public SystemController(ISystemService systemServiceImpl)
        {
            _systemServiceImpl = systemServiceImpl;
        }
        #endregion

        #region Api Services

        #region Employee Methods
        /// <summary>
        ///     根据用户名密码，获取登录用户信息。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route("employee/{userName}/{password}")]
        public IHttpActionResult GetLoginEmployee(string userName, string password)
        {
            var employee = this._systemServiceImpl.GetLoginEmployee(userName, password);
            if (employee != null)
            {
                if (employee.Status == StatusDataObject.Enalbed)
                    return Json(new ResponseModel { IsSuccess = true, StatusCode = 200, Result = employee });
                else
                    return Json(new ResponseModel { IsSuccess = false, StatusCode = 200, Result = "账户已停用，无法登陆！" });
            }
            else
                return Json(new ResponseModel { IsSuccess = false, StatusCode = 200, Result = "登陆失败，请检查用户名密码是否匹配。" });
        }
        /// <summary>
        ///     根据用户ID，获取用户信息。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("employee/{id:guid}")]
        public IHttpActionResult GetEmployeeInfo(Guid id)
        {
            var employee = this._systemServiceImpl.GetEmployeeByID(id);
            if (employee != null)
                return Json(new ResponseModel { IsSuccess = true, StatusCode = 200, Result = employee });
            else
                return Json(new ResponseModel { IsSuccess = false, StatusCode = 200, Result = "当前用户信息为空。" });
        }
        /// <summary>
        ///     分页获取所有的用户信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [Route("getallemployee/")]
        public IHttpActionResult GetAllEmployees(int page, int rows, string sort, SortOrder order, [FromUri]EmployeeDataObject employee)
        {
            PagedResult<EmployeeDataObject> pagedResult = this._systemServiceImpl.GetAllEmployee(sort, order, page, rows, employee);

            if (pagedResult != null)
            {
                var result = new
                {
                    total = pagedResult.TotalRecords,
                    rows = pagedResult.Data.Select(entity => new
                    {
                        entity.ID,
                        entity.UserName,
                        entity.RealName,
                        entity.Mobile,
                        entity.Email,
                        Status = entity.Status == StatusDataObject.Enalbed ? "启用" : "停用"
                    })
                };
                return Json(new ResponseModel { IsSuccess = true, StatusCode = 200, Result = result });
            }
            else
            {
                return Json(new ResponseModel { IsSuccess = false, StatusCode = 200, Result = "未检索到数据。。。" });
            }
        }
        /// <summary>
        ///     新增用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("InsertEmployee")]
        public IHttpActionResult PutEmployee(EmployeeDataObject obj)
        {
            //密码加密。
            obj.Password = Utils.MD5Encrypt("123456", ConfigurationManager.AppSettings["MD5Key"]);
            this._systemServiceImpl.InsertEmployee(obj);
            return Json(new { StatusCode = 200, IsSuccess = true, Result = "新增用户信息成功！" });
        }
        /// <summary>
        ///     删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("DeleteEmployee/{id:guid}")]
        public IHttpActionResult DeleteEmployee(Guid id)
        {
            this._systemServiceImpl.DeleteEmployee(id);
            return Json(new { StatusCode = 200, IsSuccess = true, Result = "删除用户信息成功！" });
        }
        /// <summary>
        ///     更新用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("UpdateEmployee")]
        public IHttpActionResult PostEmployee(EmployeeDataObject obj)
        {
            this._systemServiceImpl.UpdateEmployee(obj);
            return Json(new { StatusCode = 200, IsSuccess = true, Result = "更新用户信息成功！" });
        }
        #endregion

        #region Menu Methods
        /// <summary>
        ///     获取授权用户菜单以及页面对应菜单子父级递归集合。
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        [Route("menu/{employeeId:guid}/{controllerName}/{actionName}")]
        public IHttpActionResult GetEmployeePageMenus(Guid employeeId, string controllerName, string actionName)
        {
            var employee = this._systemServiceImpl.GetEmployeeByID(employeeId);
            if (employee != null)
            {
                ///员工授权菜单
                var empMenus = employee.Menus.ToList();
                var pageMenus = new List<MenuDataObject>();
                if (controllerName != "Home" && actionName != "Index")
                    pageMenus = this._systemServiceImpl.GetAllMenusByUrl("/" + controllerName + "/" + actionName);

                return Json(new ResponseModel { IsSuccess = true, StatusCode = 200, Result = new CustomMenu { EmployeeMenus = empMenus, PageMenus = pageMenus } });
            }
            else
            {
                return Json(new ResponseModel { IsSuccess = false, StatusCode = 200, Result = "当前用户信息为空。" });
            }
        }
        /// <summary>
        ///     根据LinkPath获取menu信息
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        [Route("menu/{controllerName}/{actionName}")]
        public IHttpActionResult GetMenuByLinkPath(string controllerName, string actionName)
        {
            List<MenuDataObject> list = this._systemServiceImpl.GetAllMenusByUrl("/" + controllerName + "/" + actionName);
            if (list.Count > 0)
                return Json(new ResponseModel { IsSuccess = true, StatusCode = 200, Result = list });
            else
                return Json(new ResponseModel { IsSuccess = false, StatusCode = 200, Result = "当前路径对应菜单信息为空。" });
        }
        #endregion

        #endregion
    }
}
