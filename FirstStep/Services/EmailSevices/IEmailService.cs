using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IEmailService
    {
        Task<string> SendEmail(EmailDto request);
        
        Task<string> SendOTPEmail(VerifyEmailDto request);//for signup, company registraion and person verification to allow pasword changin in company portal

        Task<string> VerifyOTP(OTPRequest request);


        void JobApplicationSuccessfullySentEmail(EmailDto request, string email, string jobseekerFName, string companyName,string jobAdvertisementTitle,string jobApplicationEvaluationStatusLink);//used in seeker portal
        
        void EvaluatedCompanyRegistraionApplicationEmail( string email, bool HasAccepted, string comment, string link, string company_name);//EmailDto request was removed

        void SendEmailCompanyRegistration(string email, string company_name, string applicationEvaluationStatusLink);//email for company regisrtation process
    }
}
