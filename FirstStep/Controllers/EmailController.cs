using Microsoft.AspNetCore.Mvc;
using FirstStep.Services;
using FirstStep.Models;

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
        [Route("RequestOTP/email={email}/fullname={fullName}")]
        public async Task<IActionResult> RequestOTP(string email, string fullName)
        {
            await _emailService.SendOTPEmail(email, fullName);
            return Ok();
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<ActionResult<bool>> OTPCheck(OTPRequest request)
        {
            if(await _emailService.VerifyOTP(request))
            {
                return Ok(true);
            }

            return Ok(false);
        }
    }
}
