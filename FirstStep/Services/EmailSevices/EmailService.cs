using Azure;
using Azure.Communication.Email;
using FirstStep.Models.DTOs;
using FirstStep.Models.ServiceModels;
using FirstStep.Template;
using MimeKit;

namespace FirstStep.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailClient _emailClient;

        private static HashSet<OTPRequest> OTPRequests = new HashSet<OTPRequest>();

        public EmailService(EmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        public async Task<string> SendEmail(EmailModel request)
        {
            //Email details
            var subject = request.Subject;
            var htmlContent = request.Body;
            var sender = "DoNotReply@6e8e40e7-e2d3-4f38-952f-d6dd1bbc9bca.azurecomm.net";
            var recipient = request.To;

            try
            {
                Console.WriteLine("Sending email...");
                EmailSendOperation emailSendOperation = await _emailClient.SendAsync(
                    Azure.WaitUntil.Completed,
                    sender,
                    recipient,
                    subject,
                    htmlContent);
                EmailSendResult statusMonitor = emailSendOperation.Value;

                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                string operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");

                return "Email Sent";
            }
            catch (RequestFailedException ex)
            {
                /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
                
                return ($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");//Return if error occurs
            }
        }

        // sending email in company registration process 
        public async Task SendEmailCompanyRegistration(string email,string company_name, string applicationEvaluationStatusLink)
        {
            // Registration Email
            applicationEvaluationStatusLink= " https://polite-forest-041105700.5.azurestaticapps.net/RegCheck?id=" + applicationEvaluationStatusLink;// this link will direct company to a page where the company can see its regirataion application evaluation status.
                
            EmailModel request = new();
            var builder = new BodyBuilder();

            builder.HtmlBody = EmailTemplates.CompanyRegustrationSuccessful;
            request.To = email;
            request.Subject = "Application was successfully sent";
            builder.HtmlBody = builder.HtmlBody.Replace("{Company Name}", company_name);
            builder.HtmlBody = builder.HtmlBody.Replace("{evaluation_link}", applicationEvaluationStatusLink); // here this applicationEvaluationStautsLink will direct company to a page where the company can see its regirataion application evaluation status.
            request.Body = builder.HtmlBody;
            
            await SendEmail(request);
        }

        public async Task<string> SendOTPEmail(VerifyEmailDto request) //Send OTP to the email
        {
            OTPRequest OTPrequest = new OTPRequest
            {
                email = request.email,
                otp = GenerateOTP(),
                expiry_date_time = DateTime.Now.AddMinutes(5)
            };

            // check whether the email is already request an OTP
            var dbOtpRequest = OTPRequests.FirstOrDefault(x => x.email == OTPrequest.email);

            if (dbOtpRequest is not null)
            {
                dbOtpRequest.otp = OTPrequest.otp;
                dbOtpRequest.expiry_date_time = OTPrequest.expiry_date_time;
            }
            else
            {
                OTPRequests.Add(OTPrequest);
            }

            EmailModel otpBody = new EmailModel();

            var builder = new BodyBuilder();

            builder.HtmlBody = EmailTemplates.CommonOTP;
            otpBody.To = request.email;
            otpBody.Subject = "FirstStep Verification OTP";
            builder.HtmlBody = builder.HtmlBody.Replace("{OTP}", OTPrequest.otp.ToString());
            builder.HtmlBody = builder.HtmlBody.Replace("{name}", "Test");//reciever= seeker's firstName / company name / Employee firstName
            builder.HtmlBody = builder.HtmlBody.Replace("{message}", "This is the OTP to verfiy you Email");//message = "to proceed with the registration." / "to proceed with the changing password process"
            otpBody.Body = builder.HtmlBody;

            return await SendEmail(otpBody);
        }

        public bool VerifyOTP(OTPRequest request)
        {
            var otpRequest = OTPRequests.FirstOrDefault(e => e.email == request.email && e.otp == request.otp);

            if (otpRequest is not null)
            {
                OTPRequests.FirstOrDefault(otpRequest);

                if (otpRequest.expiry_date_time < DateTime.Now)
                {
                    throw new InvalidDataException("OTP Expired");
                }

                OTPRequests.Remove(otpRequest);
                return true;
            };

            throw new InvalidDataException("OTP is invalid");
        }

        public void RemoveExpiredOTP()
        {
            OTPRequests.RemoveWhere(e => e.expiry_date_time < DateTime.Now);
        }

        public void JobApplicationSuccessfullySentEmail(EmailModel request, string email, string jobseekerFName, string companyName, string jobAdvertisementTitle, string jobApplicationEvaluationStatusLink)
        {
            EmailModel emailBody = new();
            var builder = new BodyBuilder();

            builder.HtmlBody = EmailTemplates.JobApplicationSuccessfullySent;
            emailBody.To = email;
            emailBody.Subject = "Job Application was successfully sent";
            builder.HtmlBody = builder.HtmlBody.Replace("{jobseeker}", jobseekerFName);//jobseekerFName= job seeker's first name
            builder.HtmlBody = builder.HtmlBody.Replace("{applicationtitle}", jobAdvertisementTitle); // jobAdvertisementTitle= title of the advertisement
            builder.HtmlBody = builder.HtmlBody.Replace("{companyName}", companyName);//companyName= comapny that offers that advertisement
            builder.HtmlBody = builder.HtmlBody.Replace("{evaluation_link}", jobApplicationEvaluationStatusLink);//jobApplicationEvaluationStatusLink= the link that directs job seekers to the page where she can see the status of the application evaluation
            emailBody.Body = builder.HtmlBody;

            _ = SendEmail(emailBody);
        }

        public void EvaluatedCompanyRegistraionApplicationEmail(string email, bool HasAccepted, string? comment, string link, string company_name)
        {
      
            EmailModel emailBody = new();
            var builder = new BodyBuilder();

            builder.HtmlBody = EmailTemplates.EvaluatedCompanyRegistrationApplication;
            emailBody.To = email;
            builder.HtmlBody = builder.HtmlBody.Replace("{company}", company_name);
            if (HasAccepted == true)
            {
                emailBody.Subject = "Company is successfully registered in the FirstStep Platform";
                builder.HtmlBody = builder.HtmlBody.Replace("{acceptedStatus}", "Your company has got successfully registered in the FirstStep job matching platform");
                builder.HtmlBody = builder.HtmlBody.Replace("{message}", "Use the link provided below to get started with First Step job matching platform.");
                builder.HtmlBody = builder.HtmlBody.Replace("{comment}", "");
                builder.HtmlBody = builder.HtmlBody.Replace("{link_here}", link);// link to company Admin dashboard
                builder.HtmlBody = builder.HtmlBody.Replace("{link_text}", link+" Please click this link");// link to company Admin dashboard

            }
            else
            {
                emailBody.Subject = "Company registration request got denied.";
                builder.HtmlBody = builder.HtmlBody.Replace("{acceptedStatus}", "Company registration request for FirstStep job matching platform got denied. ");
                builder.HtmlBody = builder.HtmlBody.Replace("{message}", "Use the link provided below to get started with First Step job matching platform. The following section provides the reasoning to registration request to get denined.");
                builder.HtmlBody = builder.HtmlBody.Replace("{comment}", comment);//comment that has been entered when rejecting an company registration application
                builder.HtmlBody = builder.HtmlBody.Replace("{link_here}", link);// link to document submission page
                builder.HtmlBody = builder.HtmlBody.Replace("{link_text}", link+" refer this link to start the registration process all over again.");
            }
            emailBody.Body = builder.HtmlBody;

            _ = SendEmail(emailBody);
        }

        public async Task<string> CARegIsSuccessfull(string email, string firstName, string lastName)
        {
            EmailModel emailBody = new();
            var builder = new BodyBuilder();

            builder.HtmlBody = EmailTemplates.CARegSucessfull;
            emailBody.To = email;
            emailBody.Subject = "Company Admin Registration Sucess";
            builder.HtmlBody = builder.HtmlBody.Replace("{first name}", firstName);
            builder.HtmlBody = builder.HtmlBody.Replace("{last name}", lastName);

            emailBody.Body = builder.HtmlBody;

            return await SendEmail(emailBody);
        }

        private int GenerateOTP()//Generate OTP code to store
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }
    }
}
