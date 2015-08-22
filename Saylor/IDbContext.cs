using System;

namespace Saylor
{
    public interface IDbContext
    {
        Guid InstanceID { get; }
    }
}
