using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain
{
    public interface IUserService
    {
        Task<User> FindByIdAsync(Guid userId);
        Task<User> FindByNameAsync(string userName);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AccessFailedAsync(User user);

        Task<User> FindByPhoneNumberAsync(string phoneNum);
        Task<SignInResult> CheckPhoneNumAndPwdAsync(string phoneNum, string password);
        Task<SignInResult> CheckUserNameAndPwdAsync(string userName, string password);

        Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber);
        /// <summary>
        /// 检查VCode，然后设置用户手机号为phoneNum
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phoneNum"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<SignInResult> ChangePhoneNumAsync(Guid userId, string phoneNum, string token);
        Task<IdentityResult> ChangePasswordAsync(Guid userId, string password);

        Task<IList<string>> GetRolesAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
    }
}
