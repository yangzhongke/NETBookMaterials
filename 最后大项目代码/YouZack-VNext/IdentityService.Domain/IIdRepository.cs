using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain
{
    public interface IIdRepository
    {
        Task<User?> FindByIdAsync(Guid userId);//根据Id获取用户
        Task<User?> FindByNameAsync(string userName);//根据用户名获取用户
        Task<User?> FindByPhoneNumberAsync(string phoneNum);//根据手机号获取用户
        Task<IdentityResult> CreateAsync(User user, string password);//创建用户
        Task<IdentityResult> AccessFailedAsync(User user);//记录一次登陆失败

        /// <summary>
        /// 生成重置密码的令牌
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber);
        /// <summary>
        /// 检查VCode，然后设置用户手机号为phoneNum
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phoneNum"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<SignInResult> ChangePhoneNumAsync(Guid userId, string phoneNum, string token);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> ChangePasswordAsync(Guid userId, string password);

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IList<string>> GetRolesAsync(User user);

        /// <summary>
        /// 把用户user加入角色role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        /// <summary>
        /// 为了登录而检查用户名、密码是否正确
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="lockoutOnFailure">如果登录失败，则记录一次登陆失败</param>
        /// <returns></returns>
        public Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);
        /// <summary>
        /// 确认手机号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task ConfirmPhoneNumberAsync(Guid id);

        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public Task UpdatePhoneNumberAsync(Guid id, string phoneNum);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IdentityResult> RemoveUserAsync(Guid id);

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNum"></param>
        /// <returns>返回值第三个是生成的密码</returns>
        public Task<(IdentityResult, User?, string? password)> AddAdminUserAsync(string userName, string phoneNum);

        /// <summary>
        /// 重置密码。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回值第三个是生成的密码</returns>
        public Task<(IdentityResult, User?, string? password)> ResetPasswordAsync(Guid id);
    }
}
