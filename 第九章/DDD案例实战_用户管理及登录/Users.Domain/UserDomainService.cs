using Users.Domain.Events;

namespace Users.Domain
{
    public class UserDomainService
    {
        private readonly IUserDomainRepository repository;
        private readonly ISmsCodeSender smsSender;

        public UserDomainService(IUserDomainRepository repository, ISmsCodeSender smsSender)
        {
            this.repository = repository;
            this.smsSender = smsSender;
        }

        public async Task<UserAccessResult> CheckLoginAsync(PhoneNumber phoneNum,
            string password)
        {
            User? user = await repository.FindOneAsync(phoneNum);
            UserAccessResult result;
            if (user == null)//找不到用户
            {
                result = UserAccessResult.PhoneNumberNotFound;
            }
            else if (IsLockOut(user))//用户被锁定
            {
                result = UserAccessResult.Lockout;
            }
            else if(user.HasPassword()==false)//没设密码
            {
                result = UserAccessResult.NoPassword;
            }
            else if(user.CheckPassword(password))//密码正确
            {
                result = UserAccessResult.OK;
            }
            else//密码错误
            {
                result = UserAccessResult.PasswordError;
            }
            if(user!=null)
            {
                if (result == UserAccessResult.OK)
                {
                    this.ResetAccessFail(user);//重置
                }
                else
                {
                    this.AccessFail(user);//处理登录失败
                }
            }            
            UserAccessResultEvent eventItem = new(phoneNum, result);
            await repository.PublishEventAsync(eventItem);
            return result;
        }

        public async Task<UserAccessResult> SendCodeAsync(PhoneNumber phoneNum)
        {
            var user = await repository.FindOneAsync(phoneNum);
            if (user == null)
            {
                return UserAccessResult.PhoneNumberNotFound;
            }
            if (IsLockOut(user))
            {
                return UserAccessResult.Lockout;
            }
            string code = Random.Shared.Next(1000, 9999).ToString();
            await repository.SavePhoneCodeAsync(phoneNum, code);
            await smsSender.SendCodeAsync(phoneNum, code);
            return UserAccessResult.OK;
        }

        public async Task<CheckCodeResult> CheckCodeAsync(PhoneNumber phoneNum,string code)
        {
            var user = await repository.FindOneAsync(phoneNum);
            if (user == null)
            {
                return CheckCodeResult.PhoneNumberNotFound;
            }
            if (IsLockOut(user))
            {
                return CheckCodeResult.Lockout;
            }
            string? codeInServer = await repository.RetrievePhoneCodeAsync(phoneNum);
            if (string.IsNullOrEmpty(codeInServer))
            {
                return CheckCodeResult.CodeError;
            }
            if (code == codeInServer)
            {
                return CheckCodeResult.OK;
            }
            else
            {
                AccessFail(user);
                return CheckCodeResult.CodeError;
            }
        }

        public void ResetAccessFail(User user)
        {
            user.AccessFail.Reset();
        }

        public bool IsLockOut(User user)
        {
            return user.AccessFail.IsLockOut();
        }

        public void AccessFail(User user)
        {
            user.AccessFail.Fail();
        }
    }
}
