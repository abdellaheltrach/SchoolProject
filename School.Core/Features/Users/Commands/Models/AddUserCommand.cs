using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Users.Commands.Models
{
    public class AddUserCommand : IRequest<ApiResponse<string>>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
