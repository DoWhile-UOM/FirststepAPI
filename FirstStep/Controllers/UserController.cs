using FirstStep.Data;
using FirstStep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(User user)
        {
            if (user.email == null || user.password_hash == null)
            {
                return BadRequest("Email or password is null.");
            }

            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.email == user.email);
            if (dbUser == null)
            {
                return BadRequest("User not found.");
            }

            if (dbUser.password_hash != user.password_hash)
            {
                return BadRequest("Password is incorrect.");
            }

            return Ok(dbUser);
        }
    }
}
