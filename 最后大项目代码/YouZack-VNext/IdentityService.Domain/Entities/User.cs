using Microsoft.AspNetCore.Identity;
using Zack.DomainCommons.Models;

namespace IdentityService.Domain;
public class User : IdentityUser<Guid>, IHasCreationTime, IHasDeletionTime, ISoftDelete
{
    public DateTime CreationTime { get; init; }

    public DateTime? DeletionTime { get; private set; }

    public bool IsDeleted { get; private set; }

    public User(string userName) : base(userName)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.Now;
    }

    public void SoftDelete()
    {
        this.IsDeleted = true;
        this.DeletionTime = DateTime.Now;
    }
}
