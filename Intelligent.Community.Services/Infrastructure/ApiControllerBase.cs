using Intelligent.Community.DataObjects;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Intelligent.Community.Services
{
    /// <summary>
    ///     ApiController的基类
    ///         1、 用以实现基础场景功能。
    ///         2、 对WebApi请求的认证。
    /// </summary>
    [CustomAuthentication]
    [CustomExceptionFilter]
    public class ApiControllerBase : ApiController
    {
        protected override System.Web.Http.Results.JsonResult<T> Json<T>(T content, JsonSerializerSettings serializerSettings, Encoding encoding)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            serializerSettings.Converters.Add(timeConverter);
            serializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            return base.Json<T>(content, serializerSettings, encoding);
        }
    }
    /// <summary>
    ///     应用于Controller的基础异常处理。
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ///设置http响应状态码为200，确保正常返回。
            context.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, };

            HttpContent stringContent = null;
            if (GetInnerException(context.Exception) is NotImplementedException)
            {
                stringContent = new StringContent(JsonConvert.SerializeObject(
                    new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = 501,
                        Result = "未实现的方法体。。。"
                    }), Encoding.GetEncoding("UTF-8"), "application/json"

                );
            }
            if (GetInnerException(context.Exception) is System.Data.SqlClient.SqlException)
            {
                stringContent = new StringContent(JsonConvert.SerializeObject(
                    new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Result = "服务端数据库连接异常，请联系管理员。。。"
                    }), Encoding.GetEncoding("UTF-8"), "application/json"
                );
            }
            else
            {
                stringContent = new StringContent(
                    JsonConvert.SerializeObject(
                    new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Result = GetInnerException(context.Exception) == null ? "服务端异常。。。" : GetInnerException(context.Exception).Message
                    }), Encoding.GetEncoding("UTF-8"), "application/json"
                );
            }
            context.Response.Content = stringContent;
        }

        /// <summary>
        ///     获取内部异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Exception GetInnerException(Exception ex)
        {
            return (ex.InnerException == null) ? ex : GetInnerException(ex.InnerException);
        }
    }
    /// <summary>
    ///     应用于WebApi的Action授权验证。
    /// </summary>
    public class CustomAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var referrer = actionContext.Request.Headers.Referrer;
            ///1、 判断是否跨域Ajax请求
            ///     匹配跨域请求配置
            if (referrer != null && ConfigurationManager.AppSettings["CorsConfig"].Contains(referrer.Authority))
            {
                base.OnActionExecuting(actionContext);
                return;
            }
            //2、 检查web.config配置是否要求权限校验
            bool isNotRquired = (ConfigurationManager.AppSettings["AuthenticatedWebApi"].ToString() == "false");
            if (isNotRquired)
            {
                base.OnActionExecuting(actionContext);
                return;
            }
            //3、判断是否允许匿名调用
            bool isAnonymous = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()
                .OfType<AllowAnonymousAttribute>().Any(a => a is AllowAnonymousAttribute);
            if (isAnonymous)
            {
                //允许匿名调用，继续执行。
                base.OnActionExecuting(actionContext);
                return;
            }
            //不允许匿名调用，判断是否授权。
            var authorization = actionContext.Request.Headers.Authorization;
            if ((authorization != null) && (authorization.Parameter != null))
            {
                if (authorization.Scheme == "saylor" && authorization.Parameter == ConfigurationManager.AppSettings["saylor"])
                {
                    base.OnActionExecuting(actionContext);
                }
                else
                {
                    //不存在授权密钥，抛出“未授权访问”信息
                    actionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, };
                    actionContext.Response.Content = new StringContent(
                        JsonConvert.SerializeObject(
                        new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = 401,
                            Result = "未授权的服务请求，请联系管理员。。。"
                        }), Encoding.GetEncoding("UTF-8"), "application/json"
                    );
                }
            }
            else
            {
                //不存在授权密钥，抛出“未授权访问”信息
                actionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, };
                actionContext.Response.Content = new StringContent(
                    JsonConvert.SerializeObject(
                    new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = 401,
                        Result = "未授权的服务请求，请联系管理员。。。"
                    }), Encoding.GetEncoding("UTF-8"), "application/json"
                );
            }
        }
    }
}
