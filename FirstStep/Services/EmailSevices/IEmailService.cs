using FirstStep.Models.DTOs;
using FirstStep.Models.ServiceModels;

namespace FirstStep.Services
{
    public interface IEmailService
    {
        Task<string> SendEmail(EmailModel request);
        
        Task<string> SendOTPEmail(VerifyEmailDto request);

        bool VerifyOTP(OTPRequest request);

        public void RemoveExpiredOTP();

        void JobApplicationSuccessfullySentEmail(EmailModel request, string email, string jobseekerFName, string companyName,string jobAdvertisementTitle,string jobApplicationEvaluationStatusLink);//used in seeker portal
        
        void EvaluatedCompanyRegistraionApplicationEmail( string email, bool HasAccepted, string? comment, string link, string company_name);//EmailDto request was removed

        Task SendEmailCompanyRegistration(string email, string company_name, string applicationEvaluationStatusLink);//email for company regisrtation process

        Task SendEmailInterviewBook(string email, string advertismentTitle, string company_name, int userid, int advertismentid, string comment);

        Task SendEmailInterviewBookConfirm(string email, string advertismentTitle, string company_name, string date, string time);

        Task SendPasswordReset(string email, string token);

        Task<string> CARegIsSuccessfull(string email, string firstName, string lastName);
    }
}
