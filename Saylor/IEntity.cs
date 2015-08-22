using System;

namespace Saylor
{
    /// <summary>
    ///     Represents that the implemented classes are domain entities.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        ///     Get or set the identifier of the entity.
        /// </summary>
        Guid ID { get; set; }
    }
}
