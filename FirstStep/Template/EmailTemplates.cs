namespace FirstStep.Template
{
    public static class EmailTemplates
    {
        public static string CommonOTP = 
            @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Verify Your Email</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <h2 style=""color: #333;"">FirstStep</h2>
                    <h3 style=""color: #333;"">Hello {name}!</h3>
                    <h3 style=""color: #333;"">Verify your Email</h3>
                    <p>Below is your one-time password. Use that code to verify your email {message}.</p>
                    <div style=""background-color: #f4f4f4; padding: 10px; border-radius: 5px;"">
                        <h2 style=""font-size: 22px; color: #333; margin: 0;"">OTP Code:</h2>
                        <p style=""font-size: 20px; color: #333; margin: 10px 0;""><strong>{OTP}</strong></p>
                    </div>
                    <p style=""color: #333; margin-top: 20px;"">- FirstStep</p>
                </div>
            </body>
            </html>";

        public static string ApplicationSent =
            @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Verify Your Email</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <h2 style=""color: #333;"">FirstStep</h2>

        
                    <div style=""background-color: #f4f4f4; padding: 10px; border-radius: 5px;"">
                        <h3 style=""color: #333;"">Hi {username}</h3>
                        <p>Your Application has been successfully sent.</p>
                    </div>
                    <p style=""color: #333; margin-top: 20px;"">- FirstStep</p>
                </div>
            </body>
            </html>";

        public static string CARegSucessfull =
            @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Company Admin Registration is Successful</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <h2 style=""color: #333;"">Company Admin Registration is Successful</h2>


                    <div style=""background-color: #f4f4f4; padding: 10px; border-radius: 5px;"">
                        <h3 style=""color: #333;"">Hi {first name} {last name}</h3>
                        <p>You has been succesfully registered as the company admin.</p>
                        <p>Please <a href={evaluation_link} style=""color: #007bff; text-decoration: none;"">click here</a> to check the status of your application evaluation.</p>
                    </div>
                    <p style=""color: #333; margin-top: 20px;"">Thank you for choosing FirstStep.</p>
                </div>
            </body>
            </html>";

        public static string CompanyRegustrationSuccessful =
            @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Company Registration is Successful</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <h2 style=""color: #333;"">Company Registration Successful</h2>


                    <div style=""background-color: #f4f4f4; padding: 10px; border-radius: 5px;"">
                        <h3 style=""color: #333;"">Hi {Company Name}</h3>
                        <p>Your company registration process with FirstStep has been successfully completed. We are delighted to welcome you to our platform.</p>
                        <p>Please <a href={evaluation_link} style=""color: #007bff; text-decoration: none;"">click here</a> to check the status of your application evaluation.</p>
                    </div>
                    <p style=""color: #333; margin-top: 20px;"">Thank you for choosing FirstStep.</p>
                </div>
            </body>
            </html>";

        public static string EvaluatedCompanyRegistrationApplication =
            @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"">
                <title>Email Template</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <p><strong>Dear {company},</strong></p>
                    <p>{acceptedStatus}. Please follow the link provided below to {message}<a>.</p>

        
                    <div style=""margin-top: 20px; background-color: #f8d7da; color: #721c24; padding: 10px; border-radius: 5px; display: {display_comment};"">
                        <p>{comment}</p>
                    </div>

       
                    <p>Link: <a href=""{link_here}"">{link_text}</a></p>
                </div>
            </body>
            </html>";

        public static string JobApplicationSuccessfullySent =
            @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Job Application Sent</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <h2 style=""color: #333;"">Job Application Sent</h2>


                    <div style=""background-color: #f4f4f4; padding: 10px; border-radius: 5px;"">
                        <p>Dear {jobseeker},</p>
                        <p>Your application for the {applicationtitle} position, advertised by {companyName}, has been successfully sent.</p>
                        <p>Please refer to the following link to see the evaluation status of your application: <a href=""{evaluation_link}"" style=""color: #007bff; text-decoration: none;"">Check Status</a></p>
                    </div>
                    <p style=""color: #333; margin-top: 20px;"">Thank you for your interest.</p>
                </div>
            </body>
            </html>";

        public static string SeekerInterviewSlotBooking =
            @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Schedule Your Interview</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <h2 style=""color: #333;"">Schedule Your Interview</h2>


                    <div style=""background-color: #f4f4f4; padding: 10px; border-radius: 5px;"">
                        <h3 style=""color: #333;"">Dear Applicant</h3>
                        <p>We are pleased to invite you to an interview for the [Job Position] position at {Company Name}. Please use the link below to book a time slot that works best for you.</p>
                        <p>Please <a href={booking_link} style=""color: #007bff; text-decoration: none;"">click here</a> to schedule your interview.</p>
                        <p style=""font-weight: bold;"">{comments}</p>
                    </div>
                    <p style=""color: #333; margin-top: 20px;"">Thank you for choosing FirstStep.</p>
                </div>
            </body>
            </html>";

        public static string InterviewBookConfirm =
            @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Interview Confirmation</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <h2 style=""color: #333;"">Interview Confirmation</h2>


                    <div style=""background-color: #f4f4f4; padding: 10px; border-radius: 5px;"">
                        <h3 style=""color: #333;"">Dear Applicant</h3>
                        <p>We are pleased to confirm your interview for the [Job Position] position at {Company Name}. Here are the details of your scheduled interview:</p>
                        <p><strong>Date:</strong> [Interview Date]</p>
                        <p><strong>Time:</strong> [Interview Time]</p>
                        <p>If you have any questions or need to reschedule, please do not hesitate to contact the Company.</p>
                    </div>
                    <p style=""color: #333; margin-top: 20px;"">Thank you for choosing FirstStep.</p>
                </div>
            </body>
            </html>";

        public static string SendPassWordResetLink =
        @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Password Reset Request</title>
            </head>
            <body>
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
                    <h2 style=""color: #333;"">Password Reset Request</h2>


                    <div style=""background-color: #f4f4f4; padding: 10px; border-radius: 5px;"">
                        <h3 style=""color: #333;"">Dear User,</h3>
                        <p>We received a request to reset your password for your account. Please use the link below to reset your password.</p>
                        <p>Please <a href={booking_link} style=""color: #007bff; text-decoration: none;"">click here</a> to reset your password.</p>
                        <p>If you did not request a password reset, please ignore this email or contact support if you have any questions.</p>
                    </div>
                    <p style=""color: #333; margin-top: 20px;"">Thank you for choosing FirstStep.</p>
                </div>
            </body>
            </html>";
    }
}
