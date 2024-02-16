using Email_Test.DTOs;
using FirstStep.Models.DTOs;

namespace Email_Test.EmailService
{
    public interface IEmailService
    {
        string SendEmail(RequestDTO request);
    }
}
