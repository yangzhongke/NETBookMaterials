using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Users.Domain;
using Users.Domain.Events;
using Users.Infrastructure;

namespace Users.WebAPI.Controllers.Login
{
    [Route("[controller]/[action]")]
    [ApiController]
    [UnitOfWork(typeof(UserDbContext))]
    public class LoginController : ControllerBase
    {
        private readonly UserDomainService domainService;
        private readonly UserApplicationService appService;
        private readonly ISmsCodeSender smsSender;
        private readonly IDistributedCache distCache;
        private readonly IMediator mediator;

        public LoginController(UserDomainService domainService,
            UserApplicationService appService,
            ISmsCodeSender smsSender,
            IDistributedCache distCache,  IMediator mediator)
        {
            this.domainService = domainService;
            this.smsSender = smsSender;
            this.distCache = distCache;
            this.appService = appService;
            this.mediator = mediator;
        }

        [HttpPut]
        public async Task<IActionResult> LoginByPhoneAndPwd(LoginByPhoneAndPwdRequest req)
        {
            var phoneNum = req.PhoneNumber;
            var user = await appService.FindOneAsync(req.PhoneNumber);
            if (user == null)
            {
                await mediator.Publish(new UserAccessResultEvent(phoneNum, 
                    UserAccessResult.PhoneNumberNotFound));
                return BadRequest("手机号或者密码错误");//避免泄密,不能404
            }
            var result = domainService.CheckLogin(user, req.Password);
            switch(result)
            {
                case UserAccessResult.OK:
                    await mediator.Publish(new UserAccessResultEvent(phoneNum,
                         UserAccessResult.OK));
                    return Ok("登录成功");
                case UserAccessResult.Lockout:
                    await mediator.Publish(new UserAccessResultEvent(phoneNum,
                         UserAccessResult.Lockout));
                    return BadRequest("用户被锁定，请稍后再试");
                case UserAccessResult.NoPassword:
                case UserAccessResult.PasswordError:
                    await mediator.Publish(new UserAccessResultEvent(phoneNum,result));
                    return BadRequest("手机号或者密码错误");
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendLoginByPhoneAndCode(SendLoginByPhoneAndCodeRequest req)
        {
            var user = await appService.FindOneAsync(req.PhoneNumber);
            if (user == null)
            {
                return BadRequest("请求错误");//避免泄密
            }
            if(domainService.IsLockOut(user))
            {
                return BadRequest("用户被锁定，请稍后再试");
            }
            string code = Random.Shared.Next(1000, 9999).ToString();
            var phoneNum = req.PhoneNumber;
            string fullNumber = phoneNum.RegionCode + phoneNum.Number;
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
            distCache.SetString($"LoginByPhoneAndCode_Code_{fullNumber}", code, options);
            await smsSender.SendCodeAsync(phoneNum, code);
            return Ok("验证码已发出");
        }

        [HttpPost]
        public async Task<IActionResult> CheckLoginByPhoneAndCode(CheckLoginByPhoneAndCodeRequest req)
        {
            var user = await appService.FindOneAsync(req.PhoneNumber);
            if (user == null)
            {
                return BadRequest("请求错误");//避免泄密
            }
            if (domainService.IsLockOut(user))
            {
                return BadRequest("用户被锁定，请稍后再试");
            }
            var phoneNum = req.PhoneNumber;
            string fullNumber = phoneNum.RegionCode + phoneNum.Number;
            string cacheKey = $"LoginByPhoneAndCode_Code_{fullNumber}";
            string code = distCache.GetString(cacheKey);
            if(string.IsNullOrEmpty(code))
            {
                return BadRequest("验证码已过期");
            }
            if(code==req.Code)
            {
                return Ok("登录成功");
            }
            else
            {
                distCache.Remove(cacheKey);
                domainService.AccessFail(user);
                return BadRequest("验证码错误，请重启验证");
            }
        }
    }
}
