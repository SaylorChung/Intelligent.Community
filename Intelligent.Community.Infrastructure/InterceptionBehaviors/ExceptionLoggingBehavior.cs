using Castle.DynamicProxy;
using System;

namespace Intelligent.Community.Infrastructure
{
    /// <summary>
    ///     表示用于方法异常处理的拦截行为
    /// </summary>
    public class ExceptionLoggingBehavior : IInterceptor
    {
        #region Private Fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Intelligent.Community.Logger");
        #endregion

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                log.Error("Exception caught", ex);
                throw ;
            }
        }
    }
}
