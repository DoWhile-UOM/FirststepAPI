using Microsoft.AspNetCore.Mvc;
using FirstStep.Services;
using FirstStep.Models.DTOs;
using FirstStep.Models.ServiceModels;
using FirstStep.Models;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService) {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("RequestOTP")]
        public async Task<IActionResult> RequestOTP(VerifyEmailDto request)
        {
            var response = await _emailService.SendOTPEmail(request);
            return response switch
            {
                "Email Sent" => Ok(response),
                _ => BadRequest(response),
            };
        }

        [HttpGet]
        [Route("TestEmail")]
        public async Task<IActionResult> RequestOTP()
        {
            await _emailService.SendEmailInterviewBookConfirm("ashanmatheesha@gmail.com", "Software Developer","Bistec Global","2023-07-20","1.00 PM");
            //Task SendEmailInterviewBook(string email, string advertismentTitle, string company_name, int userid, int advertismentid);
            return Ok("Email Sent");
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public IActionResult OTPCheck(OTPRequest request)
        {
            try
            {
                if (_emailService.VerifyOTP(request))
                {
                    return Ok("Email Verified");
                }

                return StatusCode(StatusCodes.Status406NotAcceptable, "Invalid OTP");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, e.Message);
            }
        }
    }
}
