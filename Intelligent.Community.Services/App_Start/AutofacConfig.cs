using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.WebApi;
using Intelligent.Community.Domain.Repositories.EntityFramewrok;
using Intelligent.Community.Infrastructure;
using Saylor;
using Saylor.Repositories;
using Saylor.Repositories.EntityFramework;
using StackExchange.Redis;
using System.Configuration;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;

namespace Intelligent.Community.Services
{
    public class AutofacConfig
    {
        public static void bootstrapper()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //注册领域仓储上下文
            builder.RegisterType<EntityFrameworkRepositoryContext>().As<IRepositoryContext>().InstancePerRequest();

            //注册数据库上下文实例
            builder.RegisterType<CommunityDbContext>().As<IDbContext>();

            //注册泛型领域仓储
            builder.RegisterGeneric(typeof(EntityFrameworkRepository<>)).As(typeof(IRepository<>));

            //注册用于方法异常的拦截行为
            builder.RegisterType<ExceptionLoggingBehavior>().AsSelf();

            //判断是否启用缓存拦截行为
            if (ConfigurationManager.AppSettings["EnabledCache"] == "true")
            {
                //注册用于方法缓存的拦截行为
                builder.RegisterType<CacheBehavior>().AsSelf();
                //注册 Redis 链接
                builder.Register(i =>
                {
                    var connect = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisPath"]);
                    return connect;
                }).AsSelf().SingleInstance();

                //注册应用层服务
                builder.RegisterAssemblyTypes(Assembly.Load("Intelligent.Community.Application"))
                    .Where(t => t.Name.EndsWith("ServiceImpl"))
                    .AsImplementedInterfaces()
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(typeof(CacheBehavior))
                    .InterceptedBy(typeof(ExceptionLoggingBehavior));
            }
            else
            {
                //注册应用层服务
                builder.RegisterAssemblyTypes(Assembly.Load("Intelligent.Community.Application"))
                    .Where(t => t.Name.EndsWith("ServiceImpl"))
                    .AsImplementedInterfaces()
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(typeof(ExceptionLoggingBehavior));
            }

            //注册重写的领域仓储
            //builder.RegisterType<SampleRepository>().As<ISampleRepository>();

            //注册Api容器
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //配置WebApi的依赖解析器
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}