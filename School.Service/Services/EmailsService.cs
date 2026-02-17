using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Service.Services.Interfaces;

namespace School.Service.Services
{
    public class EmailsService : IEmailsService
    {
        #region Fields
        private readonly EmailSettings _emailSettings;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;
        private readonly IEmailSender _emailSender;


        #endregion
        #region Constructors
        public EmailsService(EmailSettings emailSettings, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelper urlHelper,
            IEmailSender emailSender)
        {
            _emailSettings = emailSettings;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
            _emailSender = emailSender;
        }

        #endregion
        #region Handle Functions
        public async Task<bool> SendEmail(string email, string Message, string? reason)
        {
            //needs MailKit Nuget Package
            try
            {
                //sending the Message of passwordResetLink
                var bodybuilder = new BodyBuilder
                {
                    HtmlBody = $"{Message}",
                    TextBody = "wellcome",
                };
                var message = new MimeMessage
                {
                    Body = bodybuilder.ToMessageBody()
                };
                message.From.Add(new MailboxAddress("School Team", _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress("testing", email));
                message.Subject = reason == null ? "No Submitted" : reason;

                await _emailSender.SendAsync(message);
                //end of sending email
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendEmailConfirmationMail(User user)
        {
            try
            {
                // Generate email confirmation token
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Create confirmation link (adjust your domain/port as needed)


                var requestAccessor = _httpContextAccessor.HttpContext.Request;

                var scheme = requestAccessor.Scheme;                    // "https"
                var host = requestAccessor.Host.ToString();             // "localhost:7124"
                var actionUrl = _urlHelper.Action(
                    "ConfirmEmail",                                      // Action name
                    "Authentication",                                    // Controller name
                    new { userId = user.Id, code = confirmationToken }  // Route parameters
                );                                                       // "/api/v1/authentication/confirm-email?userId=3&code=..."

                // Collect them into confirmation link
                var confirmationLink = $"{scheme}://{host}{actionUrl}";


                //$"/Api/V1/Authentication/ConfirmEmail?userId={user.Id}&code={code}";
                //message or body

                // Build email body
                var message = $@"
            <h2>Welcome to School System</h2>
            <p>Hello {user.UserName},</p>
            <p>Please confirm your email address by clicking the link below:</p>
            <a href='{confirmationLink}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>
                Confirm Email
            </a>
            <p>Or copy and paste this link in your browser:</p>
            <p>{confirmationLink}</p>
            <p>This link expires in 24 hours.</p>
            <p>Best regards,<br/>School Team</p>
        ";

                // Send email using the first method
                var result = await SendEmail(user.Email, message, "Email Confirmation");

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}

