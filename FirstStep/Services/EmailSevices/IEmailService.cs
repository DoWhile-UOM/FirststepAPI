using FirstStep.Models.DTOs;

namespace FirstStep.Services.EmailSevices
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
