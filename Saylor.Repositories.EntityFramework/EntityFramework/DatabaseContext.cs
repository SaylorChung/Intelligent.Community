using System;
using System.Data.Entity;

namespace Saylor.Repositories.EntityFramework
{
    /// <summary>
    ///     数据库上下文。
    /// </summary>
    public class DatabaseContext : DbContext, IDbContext
    {
        #region Private Fields
        private readonly Guid _instanceID;
        #endregion

        #region Ctor
        public DatabaseContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            _instanceID = Guid.NewGuid();
        }
        #endregion

        #region Public Properties
        public Guid InstanceID
        {
            get { return _instanceID; }
        }
        #endregion
    }
}
