using MimeKit;

namespace School.Service.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(MimeMessage message);
    }
}
