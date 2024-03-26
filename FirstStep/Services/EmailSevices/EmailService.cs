using Azure;
using Azure.Communication.Email;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace FirstStep.Services.EmailSevices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;
        private readonly DataContext _context;

        public EmailService(IConfiguration config, ILogger<EmailService> logger, DataContext context)
        {
            _config = config;
            _logger = logger;
            _context = context;
        }

        public async void SendEmail(EmailDto request)
        {

            //Email details
            var subject = request.Subject;
            var htmlContent = request.Body;
            var sender = "DoNotReply@6e8e40e7-e2d3-4f38-952f-d6dd1bbc9bca.azurecomm.net";
            var recipient =request.To;

            try
            {
                /*
                string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");

                if (connectionString == null)
                {
                    // Handle the case where the environment variable is not set
                    Console.WriteLine("Error: COMMUNICATION_SERVICES_CONNECTION_STRING environment variable is not set.");
                    return; // or throw an exception
                }
                */
                EmailClient emailClient = new EmailClient("endpoint=https://firsstepcom.unitedstates.communication.azure.com/;accesskey=BBgT2UTVnfRfWet5z9if14CDBjoKJdjA1VWjvRWi4jbAC6y46gZaBA0mZHbtrRDAodhVPXjWZ+yd2G119BuQzA==");


                Console.WriteLine("Sending email...");
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
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
            }
            catch (RequestFailedException ex)
            {
                /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }
        }

        // sending email in company registration process 
        public void SendEmailCompanyRegistration(string email,string company_name, string applicationEvaluationStatusLink)
        {
            
                // Registration Email
                
                EmailDto request = new();
                var builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText("Template/CompanyRegustrationSuccessfulTemplate.html"))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }
                request.To = email;
                request.Subject = "Application was successfully sent";
                builder.HtmlBody = builder.HtmlBody.Replace("{Company Name}", company_name);
                builder.HtmlBody = builder.HtmlBody.Replace("{evaluation_link}", applicationEvaluationStatusLink); // here this applicationEvaluationStautsLink will direct company to a page where the company can see its regirataion application evaluation status.
                request.Body = builder.HtmlBody;



                this.SendEmail(request);
            

            
        }
        public void OTP(string email, string recieverName, string message) //Send OTP to the email
        {
            EmailDto otpBody = new();

            var builder = new BodyBuilder();

            int otp = GenerateOTP();

            string otpStr=otp.ToString();

            CreateOTPRequestRecord(email, otpStr);

            using (StreamReader SourceReader = System.IO.File.OpenText("Template/CommonOTPEmailTemplate.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            otpBody.To = email;
            otpBody.Subject = "FirstStep Verification OTP";
            builder.HtmlBody = builder.HtmlBody.Replace("{OTP}", otp.ToString());
            builder.HtmlBody = builder.HtmlBody.Replace("{name}", recieverName);//reciever= seeker's firstName / company name / Employee firstName
            builder.HtmlBody = builder.HtmlBody.Replace("{message}", message);//message = "to proceed with the registration." / "to proceed with the changing password process"
            otpBody.Body = builder.HtmlBody;

            this.SendEmail(otpBody);
        }

        public void CreateOTPRequestRecord(string emailIn,string otpIn) //Create Email OTP Request Record
        {
            var emailCheck = _context.OTPRequests.FirstOrDefault(x => x.email == emailIn);

            if (emailCheck is not null)
            {
                emailCheck.otp = otpIn;
                emailCheck.otp_expiry_date = DateTime.Now.AddMinutes(5);
                _context.SaveChanges();
            }
            else
            {
                OTPRequests otpNewRequestObj = new OTPRequests
                {
                    email = emailIn,
                    otp = otpIn,
                    status = false,
                    otp_expiry_date = DateTime.Now.AddMinutes(5)

                };
                _context.OTPRequests.Add(otpNewRequestObj);
                _context.SaveChanges();

            }


        }



        public int GenerateOTP()//Generate OTP code to store
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }



        public bool VerifyOTP(EmailVerifyDto request) //Check Email with OTP if available return true
        {
            var otpRequest = _context.OTPRequests.FirstOrDefault(x => x.email == request.email && x.otp == request.otp && x.status == false && x.otp_expiry_date > DateTime.Now);

            if (otpRequest == null)
            {
                return false;
            }
            else
            {
                otpRequest.status = true;
                _context.SaveChanges();
                return true;
            }
        }














        public void JobApplicationSuccessfullySentEmail(EmailDto request, string email, string jobseekerFName, string companyName, string jobAdvertisementTitle, string jobApplicationEvaluationStatusLink)
        {
            EmailDto emailBody = new();
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText("Template/JobApplicationSuccessfullySent.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            emailBody.To = email;
            emailBody.Subject = "Job Application was successfully sent";
            builder.HtmlBody = builder.HtmlBody.Replace("{jobseeker}", jobseekerFName);//jobseekerFName= job seeker's first name
            builder.HtmlBody = builder.HtmlBody.Replace("{applicationtitle}", jobAdvertisementTitle); // jobAdvertisementTitle= title of the advertisement
            builder.HtmlBody = builder.HtmlBody.Replace("{companyName}", companyName);//companyName= comapny that offers that advertisement
            builder.HtmlBody = builder.HtmlBody.Replace("{evaluation_link}", jobApplicationEvaluationStatusLink);//jobApplicationEvaluationStatusLink= the link that directs job seekers to the page where she can see the status of the application evaluation
            emailBody.Body = builder.HtmlBody;



            this.SendEmail(emailBody);
        }

        public void EvaluatedCompanyRegistraionApplicationEmail(EmailDto request, string email, bool HasAccepted, string comment, string link, string company_name)
        {
      
            EmailDto emailBody = new();
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText("Template/EvaluatedCompanyRegistrationApplicationTemplate.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
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



            this.SendEmail(emailBody);

        }
    }
}
