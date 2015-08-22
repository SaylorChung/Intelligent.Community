﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Saylor.Repositories
{
    /// <summary>
    ///     表示仓储上下文。
    /// </summary>
    public abstract class RepositoryContext: DisposableObject, IRepositoryContext
    {
        #region Private Fields
        private readonly Guid id = Guid.NewGuid();
        private readonly ThreadLocal<Dictionary<Guid, object>> localNewCollection = new ThreadLocal<Dictionary<Guid, object>>(() => new Dictionary<Guid, object>());
        private readonly ThreadLocal<Dictionary<Guid, object>> localModifiedCollection = new ThreadLocal<Dictionary<Guid, object>>(() => new Dictionary<Guid, object>());
        private readonly ThreadLocal<Dictionary<Guid, object>> localDeletedCollection = new ThreadLocal<Dictionary<Guid, object>>(() => new Dictionary<Guid, object>());
        private readonly ThreadLocal<bool> localCommitted = new ThreadLocal<bool>(() => true);
        #endregion

        #region Protected Properties
        /// <summary>
        ///     Gets an enumerator which iterates over the collection that contains all the objects need to be added to the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<Guid, object>> NewCollection
        {
            get { return localNewCollection.Value; }
        }
        /// <summary>
        /// G   ets an enumerator which iterates over the collection that contains all the objects need to be modified in the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<Guid, object>> ModifiedCollection
        {
            get { return localModifiedCollection.Value; }
        }
        /// <summary>
        ///     Gets an enumerator which iterates over the collection that contains all the objects need to be deleted from the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<Guid, object>> DeletedCollection
        {
            get { return localDeletedCollection.Value; }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        ///     Clears all the registration in the repository context.
        /// </summary>
        /// <remarks>Note that this can only be called after the repository context has successfully committed.</remarks>
        protected void ClearRegistrations()
        {
            this.localNewCollection.Value.Clear();
            this.localModifiedCollection.Value.Clear();
            this.localDeletedCollection.Value.Clear();
        }
        /// <summary>
        ///     Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.localCommitted.Dispose();
                this.localDeletedCollection.Dispose();
                this.localModifiedCollection.Dispose();
                this.localNewCollection.Dispose();
            }
        }
        #endregion

        #region IRepositoryContext Members
        /// <summary>
        ///     Gets the ID of the repository context.
        /// </summary>
        public Guid ID
        {
            get { return id; }
        }
        /// <summary>
        ///     Registers a new object to the repository context.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterNew<TEntity>(TEntity obj) where TEntity : class, IEntity
        {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            //if (modifiedCollection.ContainsKey(obj.ID))
            if (localModifiedCollection.Value.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object cannot be registered as a new object since it was marked as modified.");
            if (localNewCollection.Value.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object has already been registered as a new object.");
            localNewCollection.Value.Add(obj.ID, obj);
            localCommitted.Value = false;
        }
        /// <summary>
        ///     Registers a modified object to the repository context.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterModified<TEntity>(TEntity obj) where TEntity : class, IEntity
        {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            if (localDeletedCollection.Value.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object cannot be registered as a modified object since it was marked as deleted.");
            if (!localModifiedCollection.Value.ContainsKey(obj.ID) && !localNewCollection.Value.ContainsKey(obj.ID))
                localModifiedCollection.Value.Add(obj.ID, obj);
            localCommitted.Value = false;
        }
        /// <summary>
        ///     Registers a deleted object to the repository context.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterDeleted<TEntity>(TEntity obj) where TEntity : class, IEntity
        {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            if (localNewCollection.Value.ContainsKey(obj.ID))
            {
                if (localNewCollection.Value.Remove(obj.ID))
                    return;
            }
            bool removedFromModified = localModifiedCollection.Value.Remove(obj.ID);
            bool addedToDeleted = false;
            if (!localDeletedCollection.Value.ContainsKey(obj.ID))
            {
                localDeletedCollection.Value.Add(obj.ID, obj);
                addedToDeleted = true;
            }
            localCommitted.Value = !(removedFromModified || addedToDeleted);
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        ///     Gets a <see cref="System.Boolean"/> value which indicates
        ///     whether the Unit of Work could support Microsoft Distributed
        ///     Transaction Coordinator (MS-DTC).
        /// </summary>
        public virtual bool DistributedTransactionSupported
        {
            get { return false; }
        }
        /// <summary>
        ///     Gets a <see cref="System.Boolean"/> value which indicates
        ///     whether the Unit of Work was successfully committed.
        /// </summary>
        public virtual bool Committed
        {
            get { return localCommitted.Value; }
            protected set { localCommitted.Value = value; }
        }
        /// <summary>
        ///     Commits the transaction.
        /// </summary>
        public abstract void Commit();
        /// <summary>
        ///     Rollback the transaction.
        /// </summary>
        public abstract void Rollback();
        #endregion
    }
}
