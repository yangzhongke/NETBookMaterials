namespace DI魅力渐显_依赖注入
{
    interface IUserDAO
    {
        public User? GetByUserName(string userName);//查询用户名为userName的用户信息
    }
}
