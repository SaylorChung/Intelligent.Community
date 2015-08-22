using System.Data.Entity;

namespace Saylor.Repositories.EntityFramework
{
    /// <summary>
    ///     表示EntityFramework提供功能的仓储上下文。
    /// </summary>
    public class EntityFrameworkRepositoryContext : RepositoryContext, IEntityFrameworkRepositoryContext
    {
        #region Private Fields
        private readonly DatabaseContext efContext;
        private readonly object sync = new object();
        #endregion

        #region Ctor
        /// <summary>
        ///     初始化EntityFramework仓储上下文实例
        /// </summary>
        /// <param name="efContext">The <see cref="DatabaseContext"/> object that is used when initializing the <c>EntityFrameworkRepositoryContext</c> class.</param>
        public EntityFrameworkRepositoryContext(IDbContext efContext)
        {
            this.efContext = efContext as DatabaseContext;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        ///     Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                efContext.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region IEntityFrameworkRepositoryContext Members
        /// <summary>
        ///     Gets the <see cref="DatabaseContext"/> instance handled by Entity Framework.
        /// </summary>
        public DatabaseContext Context
        {
            get { return this.efContext; }
        }

        #endregion

        #region IRepositoryContext Members
        /// <summary>
        ///     Registers a new object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterNew<Tentity>(Tentity obj)
        {
            this.efContext.Entry<Tentity>(obj).State = System.Data.Entity.EntityState.Added;
            Committed = false;
        }
        /// <summary>
        ///     Registers a modified object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterModified<Tentity>(Tentity obj)
        {
            this.efContext.Entry<Tentity>(obj).State = System.Data.Entity.EntityState.Modified;
            Committed = false;
        }
        /// <summary>
        ///     Registers a deleted object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterDeleted<Tentity>(Tentity obj)
        {
            this.efContext.Entry<Tentity>(obj).State = System.Data.Entity.EntityState.Deleted;
            Committed = false;
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get { return true; }
        }
        /// <summary>
        ///     Commits the transaction.
        /// </summary>
        public override void Commit()
        {
            if (!Committed)
            {
                lock (sync)
                {
                    efContext.SaveChanges();
                }
                Committed = true;
            }
        }
        /// <summary>
        ///     Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            Committed = false;
        }

        #endregion
    }
}
