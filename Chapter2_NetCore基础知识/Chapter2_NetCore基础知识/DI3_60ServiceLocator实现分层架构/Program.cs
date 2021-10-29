using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DI3_60ServiceLocator实现分层架构
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddScoped<IDbConnection>(sp=> {
                string connStr = "Data Source=.;Initial Catalog=DI_DB;Integrated Security=true";
                var conn = new SqlConnection(connStr);
                conn.Open();
                return conn;
            });
            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IUserBiz, UserBiz>();
            using (ServiceProvider sp = services.BuildServiceProvider())
            {
                var userBiz = sp.GetService<IUserBiz>();
                bool b = userBiz.CheckLogin("yzk", "123456");
                Console.WriteLine(b);
            }
        }
    }

    class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    interface IUserDAO
    {
        /// <summary>
        /// 查询用户名为userName的用户信息
        /// </summary>
        /// <param name="userName">待查询的用户名</param>
        /// <returns></returns>
        public User GetByUserName(string userName);
    }

    interface IUserBiz
    {
        /// <summary>
        /// 检查用户名、密码是否匹配
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>如果用户名不存在或者密码错误，则返回false，否则返回true</returns>
        public bool CheckLogin(string userName, string password);
    }

    class UserDAO : IUserDAO
    {
        private readonly IServiceProvider serviceProvider;
        public UserDAO(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public User GetByUserName(string userNameToFind)
        {
            //如果提示没有GetService<IDbConnection>()方法，说明没有using Microsoft.Extensions.DependencyInjection;
            IDbConnection conn = serviceProvider.GetService<IDbConnection>();
            using(var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from T_Users where UserName=@UserName";
                var paraUserName = cmd.CreateParameter();
                paraUserName.ParameterName = "@UserName";
                paraUserName.Value = userNameToFind;
                cmd.Parameters.Add(paraUserName);
                using (var reader = cmd.ExecuteReader())
                {
                    //如果存在，则转换为User对象
                    if(reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Id"));
                        string userName = reader.GetString(reader.GetOrdinal("UserName"));
                        string pwd = reader.GetString(reader.GetOrdinal("Password"));
                        User user = new User() { Id=id,UserName=userName,Password=pwd};
                        return user;
                    }
                    else//no such userName
                    {
                        return null;
                    }
                }
            }
        }
    }

    class UserBiz : IUserBiz
    {
        private readonly IServiceProvider serviceProvider;
        public UserBiz(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public bool CheckLogin(string userName, string password)
        {
            IUserDAO userDao = serviceProvider.GetService<IUserDAO>();
            var user = userDao.GetByUserName(userName);
            if(user==null)
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