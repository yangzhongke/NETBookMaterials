using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Domain;
using Users.Infrastructure;

namespace Users.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [UnitOfWork(typeof(UserDbContext))]
    public class UsersMgrController : ControllerBase
    {
        private readonly UserDbContext dbCtx;
        private readonly UserDomainService domainService;
        private readonly IUserDomainRepository repository;

        public UsersMgrController(UserDbContext dbCtx, UserDomainService domainService, IUserDomainRepository repository)
        {
            this.dbCtx = dbCtx;
            this.domainService = domainService;
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddNew(PhoneNumber req)
        {
            if ((await repository.FindOneAsync(req))!=null)
            {
                return BadRequest("手机号已经存在");
            }
            User user = new User(req);
            dbCtx.Users.Add(user);
            return Ok("成功");
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest req)
        {
            var user = await repository.FindOneAsync(req.Id);
            if(user == null)
            {
                return NotFound();
            }
            user.ChangePassword(req.Password);
            return Ok("成功");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Unlock(Guid id)
        {
            var user = await repository.FindOneAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            domainService.ResetAccessFail(user);
            return Ok("成功");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await dbCtx.Users.ToListAsync();
            return Ok(users);
        }
    }
}
