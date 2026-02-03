using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class SendResetPasswordCommand : IRequest<ApiResponse<string>>
    {
        public string Email { get; set; }
    }
}
