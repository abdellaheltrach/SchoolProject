using School.Domain.Entities.Identity;

namespace School.Service.Services.Interfaces
{
    public interface IEmailsService
    {
        Task<bool> SendEmail(string email, string Message, string? reason);
        Task<bool> SendEmailConfirmationMail(User user);

    }
}
