using Microsoft.EntityFrameworkCore;
using Users.Domain;
using Users.Infrastructure;
using static Users.Infrastructure.ExpressionHelper;

namespace Users.WebAPI
{
    public class UserApplicationService
    {
        private readonly UserDbContext dbCtx;

        public UserApplicationService(UserDbContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        public Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            ////if(dbCtx.Users.Any(u=>u.PhoneNumber.RegionCode==req.RegionCode&&u.PhoneNumber.Numbe
            return dbCtx.Users.Include(u=>u.AccessFail).SingleOrDefaultAsync(MakeEqual((User u) => u.PhoneNumber, phoneNumber));
        }

        public Task<User?> FindOneAsync(Guid userId)
        {
            return dbCtx.Users.Include(u => u.AccessFail)
                .SingleOrDefaultAsync(u=>u.Id==userId);
        }

        public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber,string msg)
        {
            var user = await FindOneAsync(phoneNumber);
            UserLoginHistory history = new UserLoginHistory(user?.Id,
                phoneNumber, msg);
            dbCtx.LoginHistories.Add(history);
            //这里不保存
        }
    }
}
