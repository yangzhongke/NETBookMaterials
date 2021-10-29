using System;

namespace Zack.DomainCommons.Models
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; }

    }
}
