using FirstStep.Models.DTOs;

namespace FirstStep.Services.EmailSevices
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
        void OTP(string email, string recieverName, string message);//for signup, company registraion and person verification to allow pasword changin in company portal
        void SendEmailCompanyRegistration(string email, string company_name, string applicationEvaluationStatusLink);//email for company regisrtation process

        int GenerateOTP(); //OTP generation
        bool VerifyOTP(EmailVerifyDto request);// OTP verification

        void CreateOTPRequestRecord(string emailIn, string otpIn);//Create email & OTP record record
        void JobApplicationSuccessfullySentEmail(EmailDto request, string email, string jobseekerFName, string companyName,string jobAdvertisementTitle,string jobApplicationEvaluationStatusLink);//used in seeker portal
        void EvaluatedCompanyRegistraionApplicationEmail(EmailDto request, string email, bool HasAccepted, string comment, string link, string company_name);
    }
}
