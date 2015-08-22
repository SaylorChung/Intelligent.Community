using System.Data.Entity;

namespace Saylor.Repositories.EntityFramework
{
    /// <summary>
    ///     表示实现该接口的类型，是由EntityFramework提供功能的仓储上下文。
    /// </summary>
    public interface IEntityFrameworkRepositoryContext : IRepositoryContext
    {
        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> instance handled by Entity Framework.
        /// </summary>
        DatabaseContext Context { get; }
    }
}
