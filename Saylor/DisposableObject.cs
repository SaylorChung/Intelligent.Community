using System;
using System.Runtime.ConstrainedExecution;

namespace Saylor
{
    /// <summary>
    ///     Represents that the derived classes are disposable objects.
    /// </summary>
    public abstract class DisposableObject : CriticalFinalizerObject, IDisposable
    {
        #region Finalization Constructs
        /// <summary>
        ///     Finalizes the object.
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(false);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        ///     Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected abstract void Dispose(bool disposing);
        #endregion

        #region IDisposable Members
        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or
        ///     resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
