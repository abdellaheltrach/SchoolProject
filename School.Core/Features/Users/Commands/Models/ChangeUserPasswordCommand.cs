using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Users.Commands.Models
{
    public class ChangeUserPasswordCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}
