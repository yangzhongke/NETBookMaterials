using Microsoft.AspNetCore.Mvc;
using Users.Domain;
using Users.Infrastructure;

namespace Users.WebAPI.Controllers.Login
{
    [Route("[controller]/[action]")]
    [ApiController]
    [UnitOfWork(typeof(UserDbContext))]
    public class LoginController : ControllerBase
    {
        private readonly UserDomainService domainService;

        public LoginController(UserDomainService domainService)
        {
            this.domainService = domainService;
        }

        [HttpPut]
        public async Task<IActionResult> LoginByPhoneAndPwd(LoginByPhoneAndPwdRequest req)
        {
            if(req.Password.Length<3)
            {
                return BadRequest("密码的长度不能小于3");
            }
            var phoneNum = req.PhoneNumber;
            var result = await domainService.CheckLoginAsync(phoneNum, req.Password);
            switch(result)
            {
                case UserAccessResult.OK:
                    return Ok("登录成功");
                case UserAccessResult.PhoneNumberNotFound:
                    return BadRequest("手机号或者密码错误");//避免泄密,不能404
                case UserAccessResult.Lockout:
                    return BadRequest("用户被锁定，请稍后再试");
                case UserAccessResult.NoPassword:
                case UserAccessResult.PasswordError:
                    return BadRequest("手机号或者密码错误");
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendCodeByPhone(SendLoginByPhoneAndCodeRequest req)
        {
            var result = await domainService.SendCodeAsync(req.PhoneNumber);
            switch(result)
            {
                case UserAccessResult.OK:
                    return Ok("验证码已发出");
                case UserAccessResult.Lockout:
                    return BadRequest("用户被锁定，请稍后再试");
                default:
                    return BadRequest("请求错误");//避免泄密，不说细节
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckCode(CheckLoginByPhoneAndCodeRequest req)
        {
            var result = await domainService.CheckCodeAsync(req.PhoneNumber, req.Code);
            switch(result)
            {
                case CheckCodeResult.OK:
                    return Ok("登录成功");
                case CheckCodeResult.PhoneNumberNotFound:
                    return BadRequest("请求错误");//避免泄密
                case CheckCodeResult.Lockout:
                    return BadRequest("用户被锁定，请稍后再试");
                case CheckCodeResult.CodeError:
                    return BadRequest("验证码错误");
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
