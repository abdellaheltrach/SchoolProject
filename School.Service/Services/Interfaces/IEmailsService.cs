namespace School.Service.Services.Interfaces
{
    public interface IEmailsService
    {
        Task<bool> SendEmail(string email, string Message, string? reason);

    }
}
