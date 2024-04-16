using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FirstStep.Helper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services.UserServices;

namespace FirstStep.Controllers
{

    //Authentication Result return types
    public class UserRegRequest
    {
        public required string email { get; set; }

        public required string password_hash { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public string? type { get; set; }
        public string? company_id { get; set; }



    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        public UserController(DataContext authContext,IUserService userservice)
        {
            _context = authContext;
            _userService = userservice;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto userObj)
        {
            try
            {
                var response = await _userService.Authenticate(userObj);

                return response switch
                {
                    { IsSuccessful: true } => Ok(response.Token),
                    { IsSuccessful: false } => BadRequest(response.ErrorMessage),
                    _ => BadRequest(response.ErrorMessage),
                };

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegRequest userObjfull)
        {

            UserRegRequestDto userObj = new()
            {
                email = userObjfull.email,
                password_hash = userObjfull.password_hash,
                first_name = userObjfull.first_name,
                last_name = userObjfull.last_name
            };

            try
            {
                var response = await _userService.RegisterUser(userObj, userObjfull.type, userObjfull.company_id);// UserRegRequestDto must modify 

                return response switch
                {
                    "User Registered Successfully" => Ok(response),
                    "Null User" => BadRequest(response),
                    "Email Already exist" => BadRequest(response),
                    _ => BadRequest(response),
                };
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        //Test comments
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        //Refresh Token
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            try
            {
                var response = await _userService.RefreshToken(tokenApiDto);

                return response switch
                {
                    { IsSuccessful: true } => Ok(response.Token),
                    { IsSuccessful: false } => BadRequest(response.ErrorMessage),
                    _ => BadRequest(response.ErrorMessage),
                };

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
