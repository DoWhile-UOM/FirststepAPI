using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
        
        Task SendOTPEmail(string email, string recieverName);//for signup, company registraion and person verification to allow pasword changin in company portal
        
        Task<bool> VerifyOTP(OTPRequests request);
        
        void JobApplicationSuccessfullySentEmail(EmailDto request, string email, string jobseekerFName, string companyName,string jobAdvertisementTitle,string jobApplicationEvaluationStatusLink);//used in seeker portal
        
        void EvaluatedCompanyRegistraionApplicationEmail(EmailDto request, string email, bool HasAccepted, string comment, string link, string company_name);

        void SendEmailCompanyRegistration(string email, string company_name, string applicationEvaluationStatusLink);//email for company regisrtation process
    }
}
