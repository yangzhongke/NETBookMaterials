using IdentityService.Domain;
using IdentityService.Infrastructure;
using IdentityService.WebAPI.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using Zack.Commons;
using Zack.EventBus;

namespace IdentityService.WebAPI.Controllers.UserAdmin;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdUserManager userManager;
    private readonly IEventBus eventBus;

    public UserAdminController(IdUserManager userManager, IEventBus eventBus)
    {
        this.userManager = userManager;
        this.eventBus = eventBus;
    }

    [HttpGet]
    public Task<UserDTO[]> FindAllUsers()
    {
        return userManager.Users.Select(u => UserDTO.Create(u)).ToArrayAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<UserDTO> FindById(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        return UserDTO.Create(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddAdminUser(AddAdminUserRequest req)
    {
        if (await userManager.FindByNameAsync(req.UserName) != null)
        {
            return StatusCode((int)HttpStatusCode.Conflict, $"已经存在用户名【{req.UserName}】");
        }
        if (await userManager.FindByPhoneNumberAsync(req.PhoneNum) != null)
        {
            return StatusCode((int)HttpStatusCode.Conflict, $"已经存在手机号【{req.PhoneNum}】");
        }
        User user = new User(req.UserName);
        user.PhoneNumber = req.PhoneNum;
        user.PhoneNumberConfirmed = true;
        string password = userManager.GeneratePassword();
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return BadRequest("用户创建失败，" + result.Errors.SumErrors());
        }
        result = await userManager.AddToRoleAsync(user, "Admin");
        if (!result.Succeeded)
        {
            return BadRequest("用户增加Admin角色失败，" + result.Errors.SumErrors());
        }
        //生成的密码短信发给对方
        var userCreatedEvent = new UserCreatedEvent(user.Id, req.UserName, password, req.PhoneNum);
        eventBus.Publish("IdentityService.User.Created", userCreatedEvent);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteAdminUser(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound("用户没找到");
        }
        user.SoftDelete();
        await userManager.UpdateAsync(user);
        return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> UpdateAdminUser(Guid id, EditAdminUserRequest req)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound("用户没找到");
        }
        user.PhoneNumber = req.PhoneNum;
        await userManager.UpdateAsync(user);
        return Ok();
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<ActionResult> ResetAdminUserPassword(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound("用户没找到");
        }
        string password = userManager.GeneratePassword();
        string token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded)
        {
            return BadRequest("重置密码失败" + result.Errors.SumErrors());
        }
        //生成的密码短信发给对方
        var eventData = new ResetPasswordEvent(user.Id, user.UserName, password, user.PhoneNumber);
        eventBus.Publish("IdentityService.User.PasswordReset", eventData);
        return Ok();
    }
}