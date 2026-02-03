using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class ResetPasswordCommand : IRequest<ApiResponse<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
