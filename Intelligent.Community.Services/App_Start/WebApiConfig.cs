using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Intelligent.Community.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            // Web API 路由
            config.MapHttpAttributeRoutes();
            // 配置跨域请求支持
            config.EnableCors(new EnableCorsAttribute(ConfigurationManager.AppSettings["CorsConfig"], "*", "*"));
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
