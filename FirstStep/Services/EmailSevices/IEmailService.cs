using FirstStep.Models.DTOs;

namespace FirstStep.Services.EmailSevices
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);

        void OTPRequestSignUp(EmailDto request,string email, string firstName);//OTP for signup

        void SendEmailCompanyRegistration(string email, int type, string company_name, string applicationEvaluationStatusLink);//email for company regisrtation process
        void OTPVerificationPasswordChange(EmailDto request, string firstName,  string email);// OTP for person veryfication before allowing 
    }
}
