
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FirstStep.Data;
using FirstStep.Models.DTOs;
using FirstStep.Models;
using FirstStep.Services;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        
        public UserController(DataContext authContext, IUserService userservice)
        {
            _context = authContext;
            _userService = userservice; 
        }

        [HttpPost]
        [Route("authenticate")]
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

        [Authorize]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("refresh")]
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

        [HttpGet]
        [Route("GetUser/userId:int")]
        public async Task<IActionResult> GetUserById(int user_id)
        {
            UpdateEmployeeDto? employee = await _userService.GetUserById(user_id);
            if(employee is null)
            {
                return NoContent();
            }
            return Ok(employee);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user)
        {
            if(user == null)
            {
                return NoContent();
            }
            await _userService.UpdateUser(user);
            return Ok();
        }
    }
}
