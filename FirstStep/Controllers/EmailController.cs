using FirstStep.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

using Email_Test.DTOs;
//using Email_Test.EmailService;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Security;

namespace FirstStep.Controllers
{
    public class EmailController : Controller
    {
        [HttpPost]
        public IActionResult SendEmail(string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("rene.lang56@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("rene.lang56@ethereal.email"));
            email.Subject = "Test Email Subject";
            email.Body= new TextPart(TextFormat.Html) { Text =body};

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("rene.lang56@ethereal.email", "Vj28jADTeWQ3GU2A6t");// username and password
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
