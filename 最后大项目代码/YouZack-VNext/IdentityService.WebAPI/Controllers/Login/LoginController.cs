using IdentityService.Domain;
using IdentityService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace IdentityService.WebAPI.Controllers.Login;

[Route("[controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IIdRepository repository;
    private readonly IdDomainService idService;

    public LoginController(IdDomainService idService, IIdRepository repository)
    {
        this.idService = idService;
        this.repository = repository;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> CreateWorld()
    {
        if (await repository.FindByNameAsync("admin") != null)
        {
            return StatusCode((int)HttpStatusCode.Conflict, "已经初始化过了");
        }
        User user = new User("admin");
        var r = await repository.CreateAsync(user, "123456");
        Debug.Assert(r.Succeeded);
        var token = await repository.GenerateChangePhoneNumberTokenAsync(user, "18918999999");
        var cr = await repository.ChangePhoneNumAsync(user.Id, "18918999999", token);
        Debug.Assert(cr.Succeeded);
        r = await repository.AddToRoleAsync(user, "User");
        Debug.Assert(r.Succeeded);
        r = await repository.AddToRoleAsync(user, "Admin");
        Debug.Assert(r.Succeeded);
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetUserInfo()
    {
        string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await repository.FindByIdAsync(Guid.Parse(userId));
        if (user == null)//可能用户注销了
        {
            return NotFound();
        }
        //出于安全考虑，不要机密信息传递到客户端
        //除非确认没问题，否则尽量不要直接把实体类对象返回给前端
        return new UserResponse(user.Id, user.PhoneNumber, user.CreationTime);
    }

    //书中的项目只提供根据用户名登录的功能，以及管理员增删改查，像用户主动注册、手机验证码登录等功能都不弄。

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<string?>> LoginByPhoneAndPwd(LoginByPhoneAndPwdRequest req)
    {
        //todo：要通过行为验证码、图形验证码等形式来防止暴力破解
        (var checkResult, string? token) = await idService.LoginByPhoneAndPwdAsync(req.PhoneNum, req.Password);
        if (checkResult.Succeeded)
        {
            return token;
        }
        else if (checkResult.IsLockedOut)
        {
            //尝试登录次数太多
            return StatusCode((int)HttpStatusCode.Locked, "此账号已经锁定");
        }
        else
        {
            string msg = "登录失败";
            return StatusCode((int)HttpStatusCode.BadRequest, msg);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<string>> LoginByUserNameAndPwd(
        LoginByUserNameAndPwdRequest req)
    {
        (var checkResult, var token) = await idService.LoginByUserNameAndPwdAsync(req.UserName, req.Password);
        if (checkResult.Succeeded) return token!;
        else if (checkResult.IsLockedOut)//尝试登录次数太多
            return StatusCode((int)HttpStatusCode.Locked, "用户已经被锁定");
        else
        {
            string msg = checkResult.ToString();
            return BadRequest("登录失败" + msg);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangeMyPassword(ChangeMyPasswordRequest req)
    {
        Guid userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var resetPwdResult = await repository.ChangePasswordAsync(userId, req.Password);
        if (resetPwdResult.Succeeded)
        {
            return Ok();
        }
        else
        {
            return BadRequest(resetPwdResult.Errors.SumErrors());
        }
    }
}