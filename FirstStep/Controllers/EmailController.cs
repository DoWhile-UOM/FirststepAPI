using Microsoft.AspNetCore.Mvc;
using FirstStep.Services;
using FirstStep.Models.DTOs;
using FirstStep.Models.ServiceModels;

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
