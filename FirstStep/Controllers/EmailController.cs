using FirstStep.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using FirstStep.Services.EmailSevices;

// this controller is built to have a breif idea on how to use the esnd email service
// this controller is more like a temp sample controller

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
        [Route("OTPRequest")]
        public IActionResult OTPSend(string email,string fname)
        {
            _emailService.OTP(email, fname, "This is the OTP to verfiy you Email");
            return Ok();
        }

        [HttpPost]
        [Route("EmailOTPCheck")]
        public IActionResult OTPCheck(string email, string otp)
        {
            if(_emailService.VerifyOTP(new EmailVerifyDto { email = email, otp = otp }))
            {
                return Ok();
            }

            return BadRequest(new { message = "Invalid OTP" });
        }
    }
}
