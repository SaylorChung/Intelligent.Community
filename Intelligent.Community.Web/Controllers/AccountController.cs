using Intelligent.Community.DataObjects;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Web.Mvc;

namespace Intelligent.Community.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : CustomController
    {
        public ActionResult LogOn()
        {
            return View();
        }
        /// <summary>
        ///     登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isExpire"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOn(string userName, string password, bool isExpire)
        {
            var response = AsyncGetContent("api/system/employee/" + userName + "/" + password).ReadAsAsync<ResponseModel>().Result;
            if (response.IsSuccess)
            {
                var loginEmployee =JsonConvert.DeserializeObject<EmployeeDataObject>(response.Result.ToString());
                var effectiveHours = int.Parse(ConfigurationManager.AppSettings["LoginEffectiveHours"]);
                FormsAuth.SignIn(loginEmployee.RealName, loginEmployee, isExpire == true ? 60 * effectiveHours : 0);
                //登陆后处理
                return Json(new { IsSuccess = true, StatusCode = 200, Result = "登陆成功！" });
            }
            else
            {
                return Json(new { IsSuccess = false, StatusCode = 200, Result = response.Result });
            }
        }
        /// <summary>
        ///     登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            FormsAuth.SignOut();
            return Redirect("~/Account/LogOn");
        }
    }
}