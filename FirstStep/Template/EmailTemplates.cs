﻿namespace FirstStep.Template
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

        public static string ApplicationSentEmail =
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
    }
}
