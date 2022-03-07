using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
