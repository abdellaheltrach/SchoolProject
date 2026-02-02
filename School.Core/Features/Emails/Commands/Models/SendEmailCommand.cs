using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Emails.Commands.Models
{
    public class SendEmailCommand : IRequest<ApiResponse<string>>
    {
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
