using System;
using Saylor.Specifications;
using Saylor.Storage;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Saylor.Repositories
{
    /// <summary>
    ///     表示仓储的基类。
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        #region Private Fields
        private readonly IRepositoryContext context;
        #endregion

        #region Ctor
        /// <summary>
        ///     Initializes a new instance of <c>Repository&lt;TEntity&gt;</c> class.
        /// </summary>
        /// <param name="context">The repository context being used by the repository.</param>
        public Repository(IRepositoryContext context)
        {
            this.context = context;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        ///     添加一个对象到当前仓储。
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void DoInsert(TEntity entity);
        /// <summary>
        ///     移除当前仓储中的对象。
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void DoDelete(TEntity entity);
        /// <summary>
        ///     移除当前仓储中符合指定规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        protected abstract void DoDelete(ISpecification<TEntity> specification);
        /// <summary>
        ///     更新当前仓储中的对象。
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void DoUpdate(TEntity entity);


        /// <summary>
        ///     返回一个<see cref="Boolean"/>值，该值表示符合指定规约条件的领域对象是否存在。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>如果符合指定规约条件的领域对象存在，则返回true，否则返回false。</returns>
        protected abstract bool DoExists(ISpecification<TEntity> specification);
        /// <summary>
        ///     根据主键从当前仓储获取对象实例。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract TEntity DoFind(object key);
        /// <summary>
        ///     饿加载方式根据主键从当前仓储获取对象实例。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        protected abstract TEntity DoFind(object key, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     从当前仓储查询符合规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        protected abstract TEntity DoFind(ISpecification<TEntity> specification);
        /// <summary>
        ///     饿加载方式从当前仓储查询符合规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        protected abstract TEntity DoFind(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);


        /// <summary>
        ///     饿加载方式获取所有领域对象
        /// </summary>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        protected abstract IEnumerable<TEntity> DoFindAll(params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     以指定的排序字段和排序方式，以饥加载方式获取所有领域对象。
        /// </summary>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns></returns>
        protected abstract IEnumerable<TEntity> DoFindAll(Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     根据指定的规约，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns></returns>
        protected abstract IEnumerable<TEntity> DoFindAll(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     根据指定的规约，以指定的排序字段和排序方式，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns>所有符合条件的已经排序的领域对象</returns>
        protected abstract IEnumerable<TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     以指定的排序字段和排序方式，以及分页参数，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="pageNumber">分页页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns>带有分页信息的领域对象集合</returns>
        protected abstract PagedResult<TEntity> DoFindAll(Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
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
        protected abstract PagedResult<TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        ///     执行sql获取所有领域对象。
        /// </summary>
        /// <param name="query">T-SQL</param>
        /// <param name="parameters">T-SQL参数</param>
        /// <returns>领域对象集合</returns>
        protected abstract IEnumerable<TEntity> DoGetWithRawSql(string query, params object[] parameters);
        /// <summary>
        ///     获取指定的规约条件下的领域对象数量。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        protected abstract int DoCount(ISpecification<TEntity> specification);
        #endregion

        #region IRepository<TEntiy> Members
        /// <summary>
        ///     获取当前仓储上下文实例。
        /// </summary>
        public IRepositoryContext Context
        {
            get
            {
                return this.context;
            }
        }

        #region 新增/编辑/删除操作
        /// <summary>
        ///     添加一个对象到当前仓储。
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(TEntity entity)
        {
            this.DoInsert(entity);
        }
        /// <summary>
        ///     移除当前仓储中的对象。
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            this.DoDelete(entity);
        }
        /// <summary>
        ///      移除当前仓储中符合指定规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        public void Delete(ISpecification<TEntity> specification)
        {
            this.DoDelete(specification);
        }
        /// <summary>
        ///     更新当前仓储中的对象。
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            this.DoUpdate(entity);
        }
        #endregion

        #region 单条记录查询操作
        /// <summary>
        ///     返回一个<see cref="Boolean"/>值，该值表示符合指定规约条件的领域对象是否存在。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>如果符合指定规约条件的领域对象存在，则返回true，否则返回false。</returns>
        public bool Exists(ISpecification<TEntity> specification)
        {
           return this.DoExists(specification);
        }
        /// <summary>
        ///     根据主键从当前仓储获取对象实例。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TEntity Find(object key)
        {
            return this.DoFind(key);
        }
        /// <summary>
        ///     饿加载方式根据主键从当前仓储获取对象实例。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        public TEntity Find(object key, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFind(key, eagerLoadingProperties);
        }
        /// <summary>
        ///     从当前仓储查询符合规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public TEntity Find(ISpecification<TEntity> specification)
        {
            return this.DoFind(specification);
        }
        /// <summary>
        ///     饿加载方式从当前仓储查询符合规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        public TEntity Find(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFind(specification, eagerLoadingProperties);
        }
        #endregion

        #region 多条集合查询操作
        /// <summary>
        ///     饿加载方式获取所有领域对象
        /// </summary>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(eagerLoadingProperties);
        }
        /// <summary>
        ///     以指定的排序字段和排序方式，以饥加载方式获取所有领域对象。
        /// </summary>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(sortPredicate, sortOrder, eagerLoadingProperties);
        }
        /// <summary>
        ///     根据指定的规约，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, eagerLoadingProperties);
        }
        /// <summary>
        ///     根据指定的规约，以指定的排序字段和排序方式，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns>所有符合条件的已经排序的领域对象</returns>
        public IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification,sortPredicate, sortOrder, eagerLoadingProperties);
        }
        /// <summary>
        ///     以指定的排序字段和排序方式，以及分页参数，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="pageNumber">分页页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns>带有分页信息的领域对象集合</returns>
        public PagedResult<TEntity> FindAll(Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
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
        public PagedResult<TEntity> FindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification,sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        /// <summary>
        ///     执行sql获取所有领域对象。
        /// </summary>
        /// <param name="query">T-SQL</param>
        /// <param name="parameters">T-SQL参数</param>
        /// <returns>领域对象集合</returns>
        public IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return this.DoGetWithRawSql(query, parameters);
        }
        /// <summary>
        ///     获取指定的规约条件下的领域对象数量。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public int Count(ISpecification<TEntity> specification)
        {
            return this.DoCount(specification);
        }
        #endregion

        #endregion
    }
}
