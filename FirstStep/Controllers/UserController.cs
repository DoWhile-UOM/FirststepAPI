using FirstStep.Data;
using FirstStep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using System;
using FirstStep.Helpers;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _authContext;
        public UserController(DataContext authContext)
        {
            _authContext = authContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.email == userObj.email);
            if (user == null)
                return NotFound(new { message = "Username Not Found" });

            if (!PasswordHasher.VerifyPassword(userObj.password_hash, user.password_hash))
                return BadRequest(new { message = "Invalid Password" });


            return Ok(
                new
                {
                    message = "Login Successfull",
                });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            //check if email already exists
            if (await CheckEmailExist(userObj.email))
                return BadRequest(new { message = "Email already exists" });


            //password strength check
            var passCheck = PasswordStrengthCheck(userObj.password_hash);

            if (!string.IsNullOrEmpty(passCheck))
                return BadRequest(new { Message = passCheck.ToString() });

            userObj.password_hash = PasswordHasher.Hasher(userObj.password_hash);//Hash password before saving to database
            //userObj.Role = "User";
            //userObj.Token = "";

            _authContext.Users.Add(userObj);
            _authContext.SaveChanges();
            return Ok(
                new
                {
                    message = "Registration Succesfull!"
                });
        }

        private async Task<bool> CheckEmailExist(string Email)
        {
            return await _authContext.Users.AnyAsync(x => x.email == Email);
        }

        private static string PasswordStrengthCheck(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special charcter" + Environment.NewLine);
            return sb.ToString();
        }






    }
}
