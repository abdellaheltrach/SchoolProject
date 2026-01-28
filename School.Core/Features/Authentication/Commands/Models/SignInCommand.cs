using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Authentication.Commands.Response;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class SignInCommand : IRequest<ApiResponse<SignInResponse>>
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }
}
