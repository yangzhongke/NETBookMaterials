namespace DI魅力渐显_依赖注入
{
    class UserBiz : IUserBiz
    {
        private readonly IUserDAO userDao;

        public UserBiz(IUserDAO userDao)
        {
            this.userDao = userDao;
        }

        public bool CheckLogin(string userName, string password)
        {
            var user = userDao.GetByUserName(userName);
            if (user == null)
            {
                return false;
            }
            else
            {
                return user.Password == password;
            }
        }
    }
}
