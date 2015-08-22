using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Saylor.Specifications;
using Saylor.Storage;

namespace Saylor.Repositories.EntityFramework
{
    /// <summary>
    ///     表示基于EntityFramework的仓储实现。
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityFrameworkRepository<TEntity> : Repository<TEntity>
        where TEntity : class, IEntity
    {
        #region Private Fields
        private readonly IEntityFrameworkRepositoryContext efContext;
        #endregion

        #region Ctor
        /// <summary>
        ///     Initializes a new instace of <c>EntityFrameworkRepository</c> class.
        /// </summary>
        /// <param name="context">The repository context.</param>
        public EntityFrameworkRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
        #endregion

        #region Private Methods
        private MemberExpression GetMemberInfo(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
        private string GetEagerLoadingPath(Expression<Func<TEntity, dynamic>> eagerLoadingProperty)
        {
            MemberExpression memberExpression = this.GetMemberInfo(eagerLoadingProperty);
            var parameterName = eagerLoadingProperty.Parameters.First().Name;
            var memberExpressionStr = memberExpression.ToString();
            var path = memberExpressionStr.Replace(parameterName + ".", "");
            return path;
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets the instance of the <see cref="IEntityFrameworkRepositoryContext"/>.
        /// </summary>
        protected IEntityFrameworkRepositoryContext EFContext
        {
            get { return efContext; }
        }
        #endregion

        #region Protected Members
        /// <summary>
        ///     添加一个对象到当前仓储。
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoInsert(TEntity entity)
        {
            efContext.RegisterNew<TEntity>(entity);
        }
        /// <summary>
        ///     移除当前仓储中的对象。
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoDelete(TEntity entity)
        {
            efContext.RegisterDeleted<TEntity>(entity);
        }
        /// <summary>
        ///     移除当前仓储中符合指定规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        protected override void DoDelete(ISpecification<TEntity> specification)
        {
            var toDelete = DoFindAll(specification);
            foreach (var obj in toDelete)
            {
                efContext.RegisterDeleted<TEntity>(obj);
            }
        }
        /// <summary>
        ///     更新当前仓储中的对象。
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoUpdate(TEntity entity)
        {
            efContext.RegisterModified<TEntity>(entity);
        }
        /// <summary>
        ///     返回一个<see cref="Boolean"/>值，该值表示符合指定规约条件的领域对象是否存在。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>如果符合指定规约条件的领域对象存在，则返回true，否则返回false。</returns>
        protected override bool DoExists(ISpecification<TEntity> specification)
        {
            var count = efContext.Context.Set<TEntity>().Count(specification.IsSatisfiedBy);
            return count != 0;
        }
        /// <summary>
        ///     根据主键从当前仓储获取对象实例。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override TEntity DoFind(object key)
        {
            Guid id = new Guid(key.ToString());
            return efContext.Context.Set<TEntity>().Where(p => p.ID == id).FirstOrDefault();
        }
        /// <summary>
        ///     饿加载方式根据主键从当前仓储获取对象实例。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        protected override TEntity DoFind(object key, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            Guid id = new Guid(key.ToString());
            if (eagerLoadingProperties != null && eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                return dbquery.Where(p => p.ID == id).FirstOrDefault();
            }
            else
                return dbset.Where(p => p.ID == id).FirstOrDefault();
        }
        /// <summary>
        ///     从当前仓储查询符合规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        protected override TEntity DoFind(ISpecification<TEntity> specification)
        {
            return efContext.Context.Set<TEntity>().Where(specification.IsSatisfiedBy).FirstOrDefault();
        }
        /// <summary>
        ///     饿加载方式从当前仓储查询符合规约条件的领域对象。
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        protected override TEntity DoFind(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                return dbquery.Where(specification.GetExpression()).FirstOrDefault();
            }
            else
                return dbset.Where(specification.GetExpression()).FirstOrDefault();
        }
        /// <summary>
        ///     饿加载方式获取所有领域对象
        /// </summary>
        /// <param name="eagerLoadingProperties"></param>
        /// <returns></returns>
        protected override IEnumerable<TEntity> DoFindAll(params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                return dbquery.ToList();
            }
            else
                return dbset.ToList();
        }
        /// <summary>
        ///     以指定的排序字段和排序方式，以饥加载方式获取所有领域对象。
        /// </summary>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns></returns>
        protected override IEnumerable<TEntity> DoFindAll(Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            IQueryable<TEntity> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery;
            }
            else
                queryable = dbset;

            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.asc:
                        return queryable.SortBy(sortPredicate).ToList();
                    case SortOrder.desc:
                        return queryable.SortByDescending(sortPredicate).ToList();
                    default:
                        break;
                }
            }
            return queryable.ToList();
        }
        /// <summary>
        ///     根据指定的规约，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns></returns>
        protected override IEnumerable<TEntity> DoFindAll(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            IQueryable<TEntity> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.GetExpression());
            }
            else
                queryable = dbset.Where(specification.GetExpression());

            return queryable.ToList();
        }
        /// <summary>
        ///     根据指定的规约，以指定的排序字段和排序方式，以饿加载方式获取所有领域对象。
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="eagerLoadingProperties">饿加载属性Lambda表达式</param>
        /// <returns>所有符合条件的已经排序的领域对象</returns>
        protected override IEnumerable<TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            IQueryable<TEntity> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.GetExpression());
            }
            else
                queryable = dbset.Where(specification.GetExpression());

            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.asc:
                        return queryable.SortBy(sortPredicate).ToList();
                    case SortOrder.desc:
                        return queryable.SortByDescending(sortPredicate).ToList();
                    default:
                        break;
                }
            }
            return queryable.ToList();
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
        protected override PagedResult<TEntity> DoFindAll(Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "页码必须大于或等于1。");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "每页大小必须大于或等于1。");

            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;

            var dbset = efContext.Context.Set<TEntity>();
            IQueryable<TEntity> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery;
            }
            else
                queryable = dbset;

            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.asc:
                        var pagedGroupAscending = queryable.SortBy(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                        if (pagedGroupAscending == null)
                            return null;
                        return new PagedResult<TEntity>(pagedGroupAscending.Key.Total, (pagedGroupAscending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupAscending.Select(p => p).ToList());
                    case SortOrder.desc:
                        var pagedGroupDescending = queryable.SortByDescending(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                        if (pagedGroupDescending == null)
                            return null;
                        return new PagedResult<TEntity>(pagedGroupDescending.Key.Total, (pagedGroupDescending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupDescending.Select(p => p).ToList());
                    default:
                        break;
                }
            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");
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
        protected override PagedResult<TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "页码必须大于或等于1。");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "每页大小必须大于或等于1。");

            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;

            var dbset = efContext.Context.Set<TEntity>();
            IQueryable<TEntity> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.GetExpression());
            }
            else
                queryable = dbset.Where(specification.GetExpression());

            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.asc:
                        var pagedGroupAscending = queryable.SortBy(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                        if (pagedGroupAscending == null)
                            return null;
                        return new PagedResult<TEntity>(pagedGroupAscending.Key.Total, (pagedGroupAscending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupAscending.Select(p => p).ToList());
                    case SortOrder.desc:
                        var pagedGroupDescending = queryable.SortByDescending(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                        if (pagedGroupDescending == null)
                            return null;
                        return new PagedResult<TEntity>(pagedGroupDescending.Key.Total, (pagedGroupDescending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupDescending.Select(p => p).ToList());
                    default:
                        break;
                }
            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");
        }
        /// <summary>
        ///     执行sql获取所有领域对象。
        /// </summary>
        /// <param name="query">T-SQL</param>
        /// <param name="parameters">T-SQL参数</param>
        /// <returns>领域对象集合</returns>
        protected override IEnumerable<TEntity> DoGetWithRawSql(string query, params object[] parameters)
        {
            return efContext.Context.Database.SqlQuery<TEntity>(query, parameters).ToList();
        }
        /// <summary>
        ///     获取指定的规约条件下的领域对象数量。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        protected override int DoCount(ISpecification<TEntity> specification)
        {
            var dbset = efContext.Context.Set<TEntity>();

            IQueryable<TEntity> queryable = null;

            queryable = dbset.Where(specification.GetExpression());
            return queryable.Count();
        }
        #endregion
    }
}
