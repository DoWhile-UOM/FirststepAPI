using Azure;
using Azure.Communication.Email;
using FirstStep.Models.DTOs;
using MimeKit;

namespace FirstStep.Services.EmailSevices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async void SendEmail(EmailDto request)
        {
            // This code demonstrates how to fetch your connection string
            // from an environment variable.


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
        public async void SendEmailCompanyRegistration(string email,string company_name, string applicationEvaluationStatusLink)
        {
            
            
                // Registration Email
                //
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

        

       

        public async void OTP(EmailDto request, string reciever, string email, string message)
        {
            EmailDto otpBody = new();
            var builder = new BodyBuilder();
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            using (StreamReader SourceReader = System.IO.File.OpenText("Template/CommonOTPEmailTemplate.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            otpBody.To = email;
            otpBody.Subject = "FirstStep Verification OTP";
            builder.HtmlBody = builder.HtmlBody.Replace("{OTP}", otp.ToString());
            builder.HtmlBody = builder.HtmlBody.Replace("{name}", reciever);//reciever= seeker's firstName / company name / Employee firstName
            builder.HtmlBody = builder.HtmlBody.Replace("{message}", message);//message = "to proceed with the registration." / "to proceed with the changing password process"
            otpBody.Body = builder.HtmlBody;

            this.SendEmail(otpBody);
        }
    }
}
