using FirstStep.Models.DTOs;

namespace FirstStep.Services.EmailSevices
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
        void SendEmailCompanyRegistration(string email, int type);
    }
}
