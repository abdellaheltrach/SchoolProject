using MailKit.Net.Smtp;
using MimeKit;
using School.Domain.Options;
using School.Service.Services.Interfaces;

namespace School.Service.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public MailKitEmailSender(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                client.Authenticate(_emailSettings.FromEmail, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
