using Saylor.Repositories;
using System;

namespace Saylor.Application
{
    /// <summary>
    ///     表示应用层服务。
    /// </summary>
    public class ApplicationService
    {
        #region Private Fields
        private readonly IRepositoryContext context;
        #endregion

        #region Ctor
        /// <summary>
        ///     初始化一个<c>ApplicationService</c>类型的实例。
        /// </summary>
        /// <param name="context">用来初始化<c>ApplicationService</c>类型的仓储上下文实例。</param>
        public ApplicationService(IRepositoryContext context)
        {
            this.context = context;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        ///     判断指定的<see cref="String"/>值是否表示一个<see cref="Guid"/>类型的空值。
        /// </summary>
        /// <param name="s"><see cref="String"/>值</param>
        /// <returns>如果该值表示一个<see cref="Guid"/>类型的空值，则返回true，否则返回false。</returns>
        protected bool IsEmptyGuidString(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return true;
            Guid guid = new Guid(s);
            return guid == Guid.Empty;
        }
        #endregion

        #region Protected Properties
        /// <summary>
        ///     获取当前应用层服务所使用的仓储上下文实例。
        /// </summary>
        protected IRepositoryContext Context
        {
            get { return this.context; }
        }
        #endregion
    }
}
