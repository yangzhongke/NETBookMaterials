using IdentityService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure
{
    public class UserServiceImpl : IUserService
    {
        private readonly IdUserManager userManager;
        private readonly ILogger<UserServiceImpl> logger;
        private readonly RoleManager<Role> roleManager;

        public UserServiceImpl(IdUserManager userManager,
            ILogger<UserServiceImpl> logger, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.roleManager = roleManager;
        }
        public async Task<User> FindByPhoneNumberAsync(string phoneNum)
        {
            var user = await userManager.FindByPhoneNumberAsync(phoneNum);
            return user;
        }

        public async Task<SignInResult> CheckUserNameAndPwdAsync(string userName, string password)
        {
            var user = await FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            //CheckPasswordSignInAsync会对于多次重复失败进行账号禁用
            var result = await userManager.CheckForSignInAsync(user, password, true);
            return result;
        }
        public async Task<SignInResult> CheckPhoneNumAndPwdAsync(string phoneNum, string password)
        {
            var user = await FindByPhoneNumberAsync(phoneNum);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            var result = await userManager.CheckForSignInAsync(user, password, true);
            return result;
        }


        public async Task<SignInResult> ChangePhoneNumAsync(Guid userId, string phoneNum, string token)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new ArgumentException($"{userId}的用户不存在");
            }
            var changeResult = await this.userManager.ChangePhoneNumberAsync(user, phoneNum, token);
            if (!changeResult.Succeeded)
            {
                await this.userManager.AccessFailedAsync(user);
                string errMsg = changeResult.Errors.SumErrors();
                this.logger.LogWarning($"{phoneNum}ChangePhoneNumberAsync失败，错误信息{errMsg}");
                return SignInResult.Failed;
            }
            else
            {
                await userManager.ConfirmPhoneNumberAsync(user.Id);//确认手机号
                return SignInResult.Success;
            }
        }
        public async Task<IdentityResult> ChangePasswordAsync(Guid userId, string password)
        {
            if (password.Length < 6)
            {
                IdentityError err = new IdentityError();
                err.Code = "Password Invalid";
                err.Description = "密码长度不能少于6";
                return IdentityResult.Failed(err);
            }
            var user = await userManager.FindByIdAsync(userId.ToString());
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetPwdResult = await userManager.ResetPasswordAsync(user, token, password);
            return resetPwdResult;
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return this.userManager.CreateAsync(user, password);
        }

        public Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber)
        {
            return this.userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        }

        public Task<User> FindByIdAsync(Guid userId)
        {
            return userManager.FindByIdAsync(userId.ToString());
        }

        public Task<IdentityResult> AccessFailedAsync(User user)
        {
            return userManager.AccessFailedAsync(user);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                Role role = new Role { Name = roleName };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded == false)
                {
                    return result;
                }
            }
            return await userManager.AddToRoleAsync(user, roleName);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return userManager.FindByNameAsync(userName);
        }
    }
}
