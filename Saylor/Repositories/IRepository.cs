using Saylor.Specifications;
using Saylor.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Saylor.Repositories
{
    /// <summary>
    ///     表示实现该接口的类型是应用于领域对象的仓储类型。
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        ///     获取当前仓储所使用的仓储上下文实例。
        /// </summary>
        IRepositoryContext Context { get; }

        #region 新增/编辑/删除操作
        /// <summary>
        ///     添加一个对象到当前仓储。
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);
        /// <summary>
        ///     移除当前仓储中的对象。
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        ///     移除当前仓储中符合指定规约条件的领域对象。
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(ISpecification<TEntity> specification);
        /// <summary>
        ///     更新当前仓储中的对象。
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        #endregion

        #region 单条记录查询操作
        /// <summary>
        ///     返回一个<see cref="Boolean"/>值，该值表示符合指定规约条件的领域对象是否存在。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>如果符合指定规约条件的领域对象存在，则返回true，否则返回false。</returns>
        bool Exists(ISpecification<TEntity> specification);
        /// <summary>
        ///     根据主键从当前仓储获取对象实例。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity Find(object key);
        /// <summary>
        ///     饿加载方式根据主键从当前仓储获取对象实例。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        TEntity Find(object key, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     从当前仓储查询符合规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        TEntity Find(ISpecification<TEntity> specification);
        /// <summary>
        ///     饿加载方式从当前仓储查询符合规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        TEntity Find(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        #endregion

        #region 多条集合查询操作
        /// <summary>
        ///     饿加载方式获取所有领域对象
        /// </summary>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     以指定的排序字段和排序方式，以饥加载方式获取所有领域对象。
        /// </summary>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="eagerLoadingProperties">饿加载属性表达式树</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     根据指定的规约，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     根据指定的规约，以指定的排序字段和排序方式，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns>所有符合条件的已经排序的领域对象</returns>
        IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     以指定的排序字段和排序方式，以及分页参数，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="pageNumber">分页页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns>带有分页信息的领域对象集合</returns>
        PagedResult<TEntity> FindAll(Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     根据指定的规约，以指定的排序字段和排序方式，以及分页参数，以饿加载方式查找所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="pageNumber">分页页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns>带有分页信息的领域对象集合</returns>
        PagedResult<TEntity> FindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     执行sql获取所有领域对象。
        /// </summary>
        /// <param name="query">T-SQL</param>
        /// <param name="parameters">T-SQL参数</param>
        /// <returns>领域对象集合</returns>
        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);
        /// <summary>
        ///     获取指定的规约条件下的领域对象数量。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        int Count(ISpecification<TEntity> specification);
        #endregion
    }
}
