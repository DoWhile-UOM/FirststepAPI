using FirstStep.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using FirstStep.Services.EmailSevices;

/*Controller for Email Service*/

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
        [Route("OTPRequest")]// Request OTP
        public IActionResult OTPSend(string email,string fname)
        {
            _emailService.OTP(email, fname, "This is the OTP to verfiy you Email");
            return Ok(new { message = "OTP send succesfully"});
        }

        [HttpPost]
        [Route("EmailOTPCheck")]// Check Email with OTP
        public IActionResult OTPCheck(string email, string otp)
        {
            if(_emailService.VerifyOTP(new EmailVerifyDto { email = email, otp = otp }))
            {
                return Ok(new { message = "OTP is Valid" });
            }

            return BadRequest(new { message = "Expired or Invalid OTP" });
        }
    }
}
