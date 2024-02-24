using FirstStep.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using FirstStep.Services.EmailSevices;

// this controller is built to have a breif idea on how to use the esnd email service
// this controller is more like a temp sample controller

namespace FirstStep.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService) {
            _emailService = emailService;
        }
        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);

            return Ok();
        }
    }
}
