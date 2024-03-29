using Azure;
using Azure.Communication.Email;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Threading;

namespace FirstStep.Services
{
    public class EmailService : IEmailService
    {
        private readonly DataContext _context;

        public EmailService(DataContext context)
        {
            _context = context;
        }

        public async void SendEmail(EmailDto request)
        {
            //Email details
            var subject = request.Subject;
            var htmlContent = request.Body;
            var sender = "DoNotReply@6e8e40e7-e2d3-4f38-952f-d6dd1bbc9bca.azurecomm.net";
            var recipient = request.To;

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

        public async Task SendOTPEmail(string email, string recieverName) //Send OTP to the email
        {
            OTPRequests OTPrequest = new OTPRequests
            {
                email = email,
                otp = GenerateOTP()
            };

            // check whether the email is already request an OTP
            var dbOtpRequest = await _context.OTPRequests.FirstOrDefaultAsync(x => x.email == OTPrequest.email);

            if (dbOtpRequest is not null)
            {
                // update the OTP
                dbOtpRequest.otp = OTPrequest.otp;
            }
            else
            {
                _context.OTPRequests.Add(OTPrequest);
            }

            //await _context.SaveChangesAsync();

            Thread thread = new Thread(() => DeleteOTPRequest(OTPrequest));
            thread.Start();

            var builder = new BodyBuilder();
            EmailDto otpBody = new EmailDto();

            using (StreamReader SourceReader = File.OpenText("Template/CommonOTPEmailTemplate.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            otpBody.To = email;
            otpBody.Subject = "FirstStep Verification OTP";
            builder.HtmlBody = builder.HtmlBody.Replace("{OTP}", OTPrequest.otp.ToString());
            builder.HtmlBody = builder.HtmlBody.Replace("{name}", recieverName);//reciever= seeker's firstName / company name / Employee firstName
            builder.HtmlBody = builder.HtmlBody.Replace("{message}", "This is the OTP to verfiy you Email");//message = "to proceed with the registration." / "to proceed with the changing password process"
            otpBody.Body = builder.HtmlBody;

            //SendEmail(otpBody);

            Console.WriteLine("Waiting for delete OTP Request Thread to finish.....");
            thread.Join();
        }

        public async Task<bool> VerifyOTP(OTPRequests request)
        {
            var otpRequest = await _context.OTPRequests.FirstOrDefaultAsync(e => e.email == request.email && e.otp == request.otp);

            if (otpRequest is not null)
            {
                _context.OTPRequests.Remove(otpRequest);
                //await _context.SaveChangesAsync();

                return true;
            };

            return false;
        }

        private async void DeleteOTPRequest(OTPRequests request)
        {
            Thread.Sleep(15000);

            OTPRequests? existingEntity = await _context.OTPRequests.FirstOrDefaultAsync(e => e.email == request.email)!;

            if (existingEntity is not null)
            {
                _context.OTPRequests.Remove(existingEntity);
                //await _context.SaveChangesAsync();
            }
        }

        public void JobApplicationSuccessfullySentEmail(EmailDto request, string email, string jobseekerFName, string companyName, string jobAdvertisementTitle, string jobApplicationEvaluationStatusLink)
        {
            EmailDto emailBody = new();
            var builder = new BodyBuilder();
           
            using (StreamReader SourceReader = File.OpenText("Template/JobApplicationSuccessfullySent.html"))
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

            SendEmail(emailBody);
        }

        public void EvaluatedCompanyRegistraionApplicationEmail(EmailDto request, string email, bool HasAccepted, string comment, string link, string company_name)
        {
      
            EmailDto emailBody = new();
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = File.OpenText("Template/EvaluatedCompanyRegistrationApplicationTemplate.html"))
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

            SendEmail(emailBody);
        }

        private int GenerateOTP()//Generate OTP code to store
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }
    }
}
