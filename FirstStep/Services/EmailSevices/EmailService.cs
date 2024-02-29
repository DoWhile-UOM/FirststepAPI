using FirstStep.Models.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace FirstStep.Services.EmailSevices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public void SendEmail(EmailDto request)
        {
            try
            {
                _logger.LogInformation("Sending email");
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);// username and password
                smtp.Send(email);
                smtp.Disconnect(true);
                _logger.LogInformation("Emal Sent");
                _logger.LogInformation(request.To);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogInformation("Error Occured");
                _logger.LogError(ex, "An error occurred while sending email: {ErrorMessage}", ex.Message);
            }
        }
    }
}
