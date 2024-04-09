using Microsoft.AspNetCore.Mvc;
using FirstStep.Services;
using FirstStep.Models;
using FirstStep.Models.DTOs;

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
        [Route("RequestOTP")]
        public async Task<IActionResult> RequestOTP(VerifyEmailDto request)
        {
            await _emailService.SendOTPEmail(request);
            return Ok();
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> OTPCheck(OTPRequest request)
        {
            try
            {
                var response = await _emailService.VerifyOTP(request);
                switch(response)
                {
                    case "Verification Successful":
                        return Ok(response);
                    case "OTP Expired":
                        return BadRequest(response);
                    default:
                        return BadRequest(response);
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
