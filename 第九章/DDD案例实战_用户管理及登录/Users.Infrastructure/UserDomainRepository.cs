using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Users.Domain;
using Users.Domain.Events;
using static Users.Infrastructure.ExpressionHelper;

namespace Users.Infrastructure
{
    public class UserDomainRepository : IUserDomainRepository
    {
        private readonly UserDbContext dbCtx;
        private readonly IDistributedCache distCache;
        private readonly IMediator mediator;

        public UserDomainRepository(UserDbContext dbCtx, IDistributedCache distCache, IMediator mediator)
        {
            this.dbCtx = dbCtx;
            this.distCache = distCache;
            this.mediator = mediator;
        }
        public Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            ////if(dbCtx.Users.Any(u=>u.PhoneNumber.RegionCode==req.RegionCode&&u.PhoneNumber.Numbe
            return dbCtx.Users.Include(u => u.AccessFail).SingleOrDefaultAsync(MakeEqual((User u) => u.PhoneNumber, phoneNumber));
        }

        public Task<User?> FindOneAsync(Guid userId)
        {
            return dbCtx.Users.Include(u => u.AccessFail)
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg)
        {
            var user = await FindOneAsync(phoneNumber);
            UserLoginHistory history = new UserLoginHistory(user?.Id,
                phoneNumber, msg);
            dbCtx.LoginHistories.Add(history);
            //这里不保存
        }

        public Task<string?> RetrievePhoneCodeAsync(PhoneNumber phoneNumber)
        {
            string fullNumber = phoneNumber.RegionCode + phoneNumber.Number;
            string cacheKey = $"LoginByPhoneAndCode_Code_{fullNumber}";
            string? code = distCache.GetString(cacheKey);
            distCache.Remove(cacheKey);
            return Task.FromResult(code);
        }

        public Task PublishEventAsync(UserAccessResultEvent eventData)
        {
            return mediator.Publish(eventData);
        }

        public Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code)
        {
            string fullNumber = phoneNumber.RegionCode + phoneNumber.Number;
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
            distCache.SetString($"LoginByPhoneAndCode_Code_{fullNumber}", code, options);
            return Task.CompletedTask;
        }
    }
}
