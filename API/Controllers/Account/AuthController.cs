using API.Context;
using API.DOT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DbContextShop _contextShop;

        public AuthController(DbContextShop contextShop)
        {
            _contextShop = contextShop;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAccounts()
        {
            var query = from a in _contextShop.Accounts
                        join r in _contextShop.Roles on a.RoleId equals r.RoleId
                        select new
                        {
                            a.AccountId,
                            a.Username,
                            a.PasswordHash,
                            a.CreateAt,
                            RoleName = r.RoleName
                        };
            return Ok(await query.ToListAsync());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kiểm tra username đã tồn tại chưa
            if (await _contextShop.Accounts.AnyAsync(a => a.Username == model.Username))
                return BadRequest(new { message = "Username already exists!" });

            if (model.Password != model.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match!" });

            // Mã hóa mật khẩu bằng BCrypt
            string passwordHash = model.Password;

            var newAccount = new Models.Account
            {
                Username = model.Username,
                PasswordHash = passwordHash,
                RoleId = model.RoleId != 0 ? model.RoleId : 3,
                CreateAt = DateTime.UtcNow
            };

            _contextShop.Accounts.Add(newAccount);
            await _contextShop.SaveChangesAsync();

            return Ok(new { message = "Account registered successfully!" });
        }
    }
}
