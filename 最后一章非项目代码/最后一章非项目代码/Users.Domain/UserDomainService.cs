namespace Users.Domain
{
    public class UserDomainService
    {
        //参数需要是User，而不是phoneNum，因为领域层不碰数据库
        public UserAccessResult CheckLogin(User user,string password)
        {
            if(IsLockOut(user))
            {
                this.AccessFail(user);
                return UserAccessResult.Lockout;
            }
            if(user.HasPassword()==false)
            {
                this.AccessFail(user);
                return UserAccessResult.NoPassword;
            }
            if(user.CheckPassword(password))
            {
                this.ResetAccessFail(user);
                return UserAccessResult.OK;
            }
            else
            {
                this.AccessFail(user);
                return UserAccessResult.PasswordError;
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
