using System;

namespace Zack.DomainCommons.Models
{
    public interface IHasDeletionTime
    {
        DateTime? DeletionTime { get; }
    }
}
