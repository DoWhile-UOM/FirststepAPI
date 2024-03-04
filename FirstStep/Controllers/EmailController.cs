using FirstStep.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using FirstStep.Services.EmailSevices;
using Grpc.Core;

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
        [Route("SendEmail")]
        public IActionResult SendEmail(String email, int type)
        {
            if (type == 0)
            {
                // OTP Email
                EmailDto request = new();
                var builder = new BodyBuilder();
                Random random = new Random();
                int otp = random.Next(100000, 999999);
               using (StreamReader SourceReader = System.IO.File.OpenText("~/Template/EmailTemplate.html"))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                } 
               
                
                request.To = email;
                request.Subject = "FirstStep Verification OTP";
                builder.HtmlBody = builder.HtmlBody.Replace("{OTP}", otp.ToString());
                request.Body = builder.HtmlBody;

               // request.Body = "<p>Use this as the verification otp</p><br><h4>OTP :" + otp.ToString() + "</h4>";
               
                _emailService.SendEmail(request);
            }
            else if (type == 1)
            {
                // Registration Email
            }
            

            return Ok();
        }
    }
}
