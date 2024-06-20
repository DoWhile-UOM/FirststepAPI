
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

        [Authorize]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUser/userId={userId:int}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            UserDto? user = await _userService.GetUserById(userId);
            if (user is null)
            {
                return NoContent();
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("Authenticate")]
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

        [HttpPost]
        [Route("Refresh")]
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
        
        [HttpPost]
        [Route("RestPasswordRequest/{userEmail}")]
        public async Task<IActionResult> ResetPassReuest(string userEmail)
        {
            try
            {
                await _userService.ResetPasswordRequest(userEmail);
                return Ok("Password Reset Link Sent");

            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPost]
        [Route("RestPassword")]
        public async Task<IActionResult> ResetPass([FromBody] PasswordResetDto userObj)
        {
            try
            {
                await _userService.ResetPassword(userObj);
                return Ok("Password Reset Was Succesful");


            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
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
        
        private ActionResult ReturnStatusCode(Exception e)
        {
            if (e is InvalidDataException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            else if (e is NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
