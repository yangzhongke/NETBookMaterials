using Microsoft.AspNetCore.Mvc;

namespace 领域事件发布的时机1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserDbContext context;

        public UsersController(UserDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserRequest req)
        {
            var user = new User(req.UserName, req.Email);
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id,UpdateUserRequest req)
        {
            User? user = context.Users.Find(id);
            if (user==null)
            {
                return NotFound($"id={id}的User不存在");
            }
            user.ChangeAge(req.Age);
            user.ChangeEmail(req.Email);
            user.ChangeNickName(req.NickName);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> Delete(Guid id)
        {
            User? user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound($"id={id}的User不存在");
            }
            user.SoftDelete();
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
