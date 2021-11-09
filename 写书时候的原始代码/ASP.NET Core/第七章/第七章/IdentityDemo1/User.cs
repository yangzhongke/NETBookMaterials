using Microsoft.AspNetCore.Identity;

namespace IdentityDemo1
{
    public class User : IdentityUser<long>
    {
        public DateTime CreationTime { get; set; }
        public string? NickName { get; set; }
        /// <summary>
        /// 用来解决JWT无法作废的问题，用long可以解决缓存的问题（允许比数据库中版本大的）
        /// </summary>
        public long JWTVersion { get; set; }
    }
}
