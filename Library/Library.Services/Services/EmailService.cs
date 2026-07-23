using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Library.Services.Services
{
    public class EmailService : IEmailService
    {
        public EmailService() { }
        public void SeedEmail(string to, string subject, string body)
        {
            try
            {
                string senderEmail = "dimitrisheklashvili3@gmail.com";
                string appPassword = "kpavvgyqtlwlbrnv";

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, appPassword);

                    using (MailMessage mailMessage = new MailMessage(senderEmail, to, subject, body))
                    {
                        mailMessage.IsBodyHtml = true;
                        smtpClient.Send(mailMessage);
                    }
                }

                Console.WriteLine($"[EMAIL DELIVERED] Code sent to {to}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[SMTP ERROR] Could not send email: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[DETAILS] {ex.InnerException.Message}");
                }

                Console.WriteLine($"\n>>> TESTING FALLBACK CODE: {body} <<<\n");
            }
        }
        public string GetWelcomeEmailTemplate(string username, string verificationCode)
        {
            return $"""
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Welcome to Library System!</title>
    </head>
    <body style="margin: 0; padding: 0; font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; background-color: #f4f6f8; -webkit-font-smoothing: antialiased;">

        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: #f4f6f8; padding: 40px 0;">
            <tr>
                <td align="center">
                    <table border="0" cellpadding="0" cellspacing="0" width="600" style="background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05);">
                        
                        <tr>
                            <td align="center" style="background-color: #4F46E5; padding: 40px 20px;">
                                <h1 style="color: #ffffff; margin: 0; font-size: 28px; font-weight: 700; letter-spacing: -0.5px;">Welcome!</h1>
                            </td>
                        </tr>

                        <tr>
                            <td style="padding: 40px 30px; background-color: #ffffff;">
                                <h2 style="color: #111827; margin-top: 0; margin-bottom: 20px; font-size: 22px; font-weight: 600;">Hello, {username}! 👋</h2>
                                
                                <p style="color: #4B5563; font-size: 16px; line-height: 1.6; margin-bottom: 25px;">
                                    We are thrilled to have you join us! On behalf of our team, we would like to send you our warmest welcome and wish you a fantastic experience with us.
                                </p>
                                
                                <p style="color: #4B5563; font-size: 16px; line-height: 1.6; margin-bottom: 20px;">
                                    Your verification code is:
                                </p>

                                <!-- Verification Code Box -->
                                <div style="text-align: center; margin: 25px 0;">
                                    <span style="font-size: 32px; font-weight: bold; color: #4F46E5; letter-spacing: 6px; background-color: #EEF2FF; padding: 12px 24px; border-radius: 8px; border: 1px dashed #4F46E5; display: inline-block;">
                                        {verificationCode}
                                    </span>
                                </div>

                                <p style="color: #4B5563; font-size: 16px; line-height: 1.6; margin-top: 35px; margin-bottom: 0;">
                                    If you have any questions, feel free to reply directly to this email—our team is always here to help.
                                </p>
                            </td>
                        </tr>

                        <tr>
                            <td style="background-color: #F9FAFB; padding: 24px 30px; text-align: center; border-top: 1px solid #E5E7EB;">
                                <p style="color: #9CA3AF; font-size: 13px; margin: 0; line-height: 1.4;">
                                    © 2026 Library System. All rights reserved.
                                </p>
                                <p style="color: #9CA3AF; font-size: 13px; margin: 8px 0 0 0;">
                                    Tbilisi, Georgia
                                </p>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>

    </body>
    </html>
    """;
        }
        public void SendWelcomeVerificationEmail(string toEmail, string username, string verificationCode)
        {
            string htmlBody = GetWelcomeEmailTemplate(username, verificationCode);
            SeedEmail(toEmail, "Welcome & Account Verification", htmlBody);
        }
    }
}
    
