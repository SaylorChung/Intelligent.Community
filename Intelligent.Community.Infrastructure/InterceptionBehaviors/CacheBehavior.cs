using Castle.DynamicProxy;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;

namespace Intelligent.Community.Infrastructure
{
    /// <summary>
    ///     表示用于方法缓存功能的拦截行为。
    /// </summary>
    public class CacheBehavior : IInterceptor
    {
        #region private Fields
        private ConnectionMultiplexer RedisConnection { get; set; }
        #endregion

        #region Ctor
        public CacheBehavior(ConnectionMultiplexer connection)
        {
            this.RedisConnection = connection;
        }
        #endregion

        #region Public Methods
        public void Intercept(IInvocation invocation)
        {
            //// 判断目前方法是否有需要启用Cache，若有加上标记表示需要
            var attributes = invocation.Method.GetCustomAttributes(typeof(CacheAttribute), true);
            if (attributes.Count() > 0)
            {
                //// Cache key
                var key = string.Format("{0}.{1}.{2}", invocation.TargetType.FullName,
                                                       invocation.MethodInvocationTarget.Name,
                                                       JsonConvert.SerializeObject(invocation.Arguments));
                //// Expire Time
                var expireTime = (attributes.First() as CacheAttribute).ExpireTime;
                IDatabase cache = this.RedisConnection.GetDatabase();
                //// Cache是否存在，不存在产生新的
                var result = cache.Get(key);
                if (result != null)
                {
                    invocation.ReturnValue = result;
                    return;
                }
                invocation.Proceed();
                if (invocation.ReturnValue != null)
                {
                    //存在返回值，添加到缓存。
                    cache.Set(key, invocation.ReturnValue, TimeSpan.FromSeconds(expireTime));
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
        #endregion
    }
}
