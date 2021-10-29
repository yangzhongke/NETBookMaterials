using IdentityService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace IdentityService.Infrastructure;
public class IdUserManager : UserManager<User>
{
    public IdUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) :
        base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    /// <summary>
    /// 尝试登录，如果lockoutOnFailure为true，则登录失败还会自动进行lockout计数
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="lockoutOnFailure"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public async Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure)
    {
        if(await IsLockedOutAsync(user))
        {
            return SignInResult.LockedOut;
        }
        var success = await CheckPasswordAsync(user, password);
        if(success)
        {
            return SignInResult.Success;
        }
        else
        {
            if(lockoutOnFailure)
            {
                var r = await AccessFailedAsync(user);
                if (!r.Succeeded)
                {
                    throw new ApplicationException("AccessFailed failed");
                }
            }            
            return SignInResult.Failed;
        }
    }

    public Task<User?> FindByPhoneNumberAsync(string phoneNumber)
    {
        return this.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task ConfirmPhoneNumberAsync(Guid id)
    {
        var user = await this.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new ArgumentException($"用户找不到，id={id}", nameof(id));
        }
        user.PhoneNumberConfirmed = true;
        await this.UpdateAsync(user);
    }

    public async Task UpdatePhoneNumberAsync(Guid id, string phoneNum)
    {
        var user = await this.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new ArgumentException($"用户找不到，id={id}", nameof(id));
        }
        user.PhoneNumber = phoneNum;
        await this.UpdateAsync(user);
    }

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IdentityResult> RemoveUserAsync(Guid id)
    {
        User user = await FindByIdAsync(id.ToString());
        var userLoginStore = (IUserLoginStore<User>)Store;
        var noneCT = default(CancellationToken);
        //一定要删除aspnetuserlogins表中的数据，否则再次用这个外部登录登录的话
        //就会报错：The instance of entity type 'IdentityUserLogin<Guid>' cannot be tracked because another instance with the same key value for {'LoginProvider', 'ProviderKey'} is already being tracked.
        //而且要先删除aspnetuserlogins数据，再软删除User
        var logins = await userLoginStore.GetLoginsAsync(user, noneCT);
        foreach (var login in logins)
        {
            await userLoginStore.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey, noneCT);
        }
        user.SoftDelete();
        var result = await UpdateAsync(user);
        return result;
    }

    public string GeneratePassword()
    {
        var options = Options.Password;
        int length = options.RequiredLength;
        bool nonAlphanumeric = options.RequireNonAlphanumeric;
        bool digit = options.RequireDigit;
        bool lowercase = options.RequireLowercase;
        bool uppercase = options.RequireUppercase;
        StringBuilder password = new StringBuilder();
        Random random = new Random();
        while (password.Length < length)
        {
            char c = (char)random.Next(32, 126);
            password.Append(c);
            if (char.IsDigit(c))
                digit = false;
            else if (char.IsLower(c))
                lowercase = false;
            else if (char.IsUpper(c))
                uppercase = false;
            else if (!char.IsLetterOrDigit(c))
                nonAlphanumeric = false;
        }

        if (nonAlphanumeric)
            password.Append((char)random.Next(33, 48));
        if (digit)
            password.Append((char)random.Next(48, 58));
        if (lowercase)
            password.Append((char)random.Next(97, 123));
        if (uppercase)
            password.Append((char)random.Next(65, 91));
        return password.ToString();
    }
}