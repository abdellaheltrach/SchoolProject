using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class SignInCommand : IRequest<ApiResponse<string>>
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }
}
