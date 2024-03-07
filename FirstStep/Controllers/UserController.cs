using FirstStep.Data;
using FirstStep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using System;
using FirstStep.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FirstStep.Models.DTOs;
using System.Security.AccessControl;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto userObj)
        {
            /*
            if (userObj is null)
                return BadRequest();*/

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.email == userObj.email);
            if (user == null)
                return NotFound(new { message = "Username Not Found" });

            if (!PasswordHasher.VerifyPassword(userObj.password, user.password_hash))
                return BadRequest(new { message = "Invalid Password" });


            user.token = CreateJwt(user); //create access token for session
            var newAccessToken = user.token; 
            var newRefreshToken = CreateRefreshToken(); //create refresh token
            user.refresh_token= newRefreshToken;
            await _authContext.SaveChangesAsync();// save changes to database

            return Ok(
                new TokenApiDto()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegRequestDto userObj)
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
            //userObj.token = CreateVerifyToken();

            User userNewObj = new User
            {
                email = userObj.email,
                password_hash = userObj.password_hash,
                first_name = userObj.first_name,
                last_name = userObj.last_name,
                user_type = "User",
                token =null,
                refresh_token = null
            };  

            _authContext.Users.Add(userNewObj);
            _authContext.SaveChanges();
            //Send email verification link
            //SendEmail(userObj.email, userObj.token);

            return Ok(
                new
                {
                    message = "Registration Succesfull!"
                });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authContext.Users.ToListAsync();
            return Ok(users);
        }

        //verify user 
        /*
        [HttpGet("/verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.token == token);

            if(user == null)
            {
                return BadRequest("Invalid or expired token");
            }

            user.token = (DateTime.Now).ToString("yyyy-MM-dd HH:mm");
            await _authContext.SaveChangesAsync();

            return Ok("User verified");
        }
        */

        private async Task<bool> CheckEmailExist(string Email)
        {
            return await _authContext.Users.AnyAsync(x => x.email == Email);
        }

        //User verify
        /*
        private async Task<bool> VerifyUser(string token)
        {
            return false;
        }   
        */

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


        private string CreateVerifyToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }




        //JWT token generator
        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.user_type),
                new Claim(ClaimTypes.Name,$"{user.first_name} {user.last_name}"),
                new Claim(ClaimTypes.Email, user.email)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        //Refresh token generator
        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _authContext.Users
                .Any(a => a.refresh_token == refreshToken);
            if (tokenInUser)//If refresh token exist generate new one otherwise return
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        //Fetch user prinicipals from the expired access token
        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;

        }

        //Refresh Token
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.email == username);
            if (user is null || user.refresh_token != refreshToken || user.refresh_token_expiry <= DateTime.Now)
                return BadRequest("Invalid Request");
            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.refresh_token = newRefreshToken;
            await _authContext.SaveChangesAsync();
            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }
        



    }
}
