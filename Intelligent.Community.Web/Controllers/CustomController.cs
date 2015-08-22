using Intelligent.Community.DataObjects;
using Intelligent.Community.Web.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace Intelligent.Community.Web.Controllers
{
    /// <summary>
    ///     自定义控制器基类
    /// </summary>
    public class CustomController : Controller
    {
        #region Protected Members
        protected readonly HttpClient httpClient = new HttpClient();
        #endregion

        #region Protected Methods
        /// <summary>
        ///     异步获取Http Get 请求响应正文。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        protected HttpContent AsyncGetContent(string requestUri)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("saylor", "c2F5bG9y");
            return httpClient.GetAsync(ConfigurationManager.AppSettings["ApiPrefix"].ToString() + requestUri)
                .Result.Content;
        }
        /// <summary>
        ///     异步获取Http Post 请求响应正文。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="requestJson"></param>
        /// <returns></returns>
        protected HttpContent AsyncPostContent(string requestUri, string requestJson)
        {
            HttpContent httpContent = new StringContent(requestJson);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("saylor", "c2F5bG9y");

            return httpClient.PostAsync(ConfigurationManager.AppSettings["ApiPrefix"].ToString() + requestUri, httpContent)
                .Result.Content;
        }
        /// <summary>
        ///     异步获取Http Post 请求响应正文
        ///         返回一个JsonResult结果。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="requestJson"></param>
        /// <returns></returns>
        protected JsonResult AsyncPostResponse(string requestUri, string requestJson)
        {
            HttpContent httpContent = new StringContent(requestJson);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("saylor", "c2F5bG9y");
            ResponseModel obj = JsonConvert.DeserializeObject<ResponseModel>(httpClient.PostAsync(ConfigurationManager.AppSettings["ApiPrefix"].ToString() + requestUri, httpContent)
                .Result.Content.ReadAsStringAsync().Result);

            return Json(obj);
        }
        /// <summary>
        ///     异步获取Http Put 请求响应正文
        ///         返回一个JsonResult结果。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="requestJson"></param>
        /// <returns></returns>
        protected JsonResult AsyncPutResponse(string requestUri, string requestJson)
        {
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("saylor", "c2F5bG9y");
            ResponseModel obj = JsonConvert.DeserializeObject<ResponseModel>(httpClient.PutAsync(ConfigurationManager.AppSettings["ApiPrefix"].ToString() + requestUri, httpContent)
                .Result.Content.ReadAsStringAsync().Result);

            return Json(obj);

        }
        /// <summary>
        ///     异步获取Http Delete 请求响应正文
        ///         返回一个JsonResult结果。
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        protected JsonResult AsyncDeleteResponse(string requestUri)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("saylor", "c2F5bG9y");

            ResponseModel obj = JsonConvert.DeserializeObject<ResponseModel>(httpClient.DeleteAsync(ConfigurationManager.AppSettings["ApiPrefix"].ToString() + requestUri)
                .Result.Content.ReadAsStringAsync().Result);

            return Json(obj);
        }
        #endregion

        #region Protected Override Methodse
        /// <summary>
        ///     重写的异常处理。
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            //if the request is AJAX return JSON else view
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        StatusCode = 200,
                        IsSuccess = false,
                        Result = filterContext.Exception.Message
                    }
                };
            }
            else
            {
                var ex = filterContext.Exception ?? new Exception("no further information exists");
                filterContext.ExceptionHandled = true;
                var errorModel = new UnknownModel { Message = ex.Message };
                filterContext.Result = View("/Views/Page/Unknown.cshtml", errorModel);
            }
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
        #endregion
    }
}
