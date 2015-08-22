using Intelligent.Community.Application;
using Intelligent.Community.Domain.Repositories.EntityFramewrok;
using System.Data.Entity;
using System.Web.Http;

namespace Intelligent.Community.Services
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //配置WebApi每次只返回JSON格式数据。
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //启动Autofac注入。
            AutofacConfig.bootstrapper();
            //初始化应用层服务。
            ApplicationServiceInitailizer.Initialize();
            ///初始化log4net组件
            log4net.Config.XmlConfigurator.Configure();
            //开启数据库自动迁移。
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CommunityDbContext, Intelligent.Community.Domain.Repositories.Migrations.Configuration>()); 
        }
    }
}
