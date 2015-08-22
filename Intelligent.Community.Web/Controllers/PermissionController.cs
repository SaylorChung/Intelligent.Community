using Intelligent.Community.DataObjects;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Mvc;

namespace Intelligent.Community.Web.Controllers
{
    public class PermissionController : CustomController
    {
        // GET: Permission/Employee
        public ActionResult Employee()
        {
            return View();
        }
        /// <summary>
        ///     新增|编辑用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public ActionResult EditEmployee(Guid? id)
        {
            var entity = new EmployeeDataObject();
            //默认启用
            entity.Status = StatusDataObject.Enalbed;
            if (id != null && id != new Guid())
            {
                var response = AsyncGetContent("api/system/employee/" + id).ReadAsAsync<ResponseModel>().Result;
                if (response.IsSuccess)
                    entity = JsonConvert.DeserializeObject<EmployeeDataObject>(response.Result.ToString());
            }
            return View(entity);
        }
    }
}