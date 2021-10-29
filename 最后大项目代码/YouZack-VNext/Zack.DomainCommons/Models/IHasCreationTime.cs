using System;

namespace Zack.DomainCommons.Models
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; }
    }
}
