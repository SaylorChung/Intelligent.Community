using System;

namespace Saylor.Repositories
{
    /// <summary>
    ///     表示实现该接口的类型是仓储事物的上下文。
    /// </summary>
    public interface IRepositoryContext: IUnitOfWork, IDisposable
    {
        #region Properties
        /// <summary>
        ///     获取仓储上下文的ID。
        /// </summary>
        Guid ID { get; }
        #endregion

        #region Methods
        /// <summary>
        ///     将指定的领域对象标注为“新建”状态。
        /// </summary>
        /// <typeparam name="TEntity">需要标注状态的领域对象类型。</typeparam>
        /// <param name="obj">需要标注状态的领域对象。</param>
        void RegisterNew<TEntity>(TEntity obj)
            where TEntity : class, IEntity;
        /// <summary>
        ///     将指定的领域对象标注为“更改”状态。
        /// </summary>
        /// <typeparam name="TEntity">需要标注状态的领域对象类型。</typeparam>
        /// <param name="obj">需要标注状态的领域对象</param>
        void RegisterModified<TEntity>(TEntity obj)
            where TEntity : class, IEntity;
        /// <summary>
        ///     将指定的领域对象标注为“删除”状态。
        /// </summary>
        /// <typeparam name="TEntity">需要标注状态的领域对象类型</typeparam>
        /// <param name="obj">需要标注状态的领域对象</param>
        void RegisterDeleted<TEntity>(TEntity obj)
            where TEntity : class, IEntity;
        #endregion
    }
}
