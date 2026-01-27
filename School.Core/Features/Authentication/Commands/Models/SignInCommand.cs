using MediatR;
using School.Core.Base.ApiResponse;
using School.Domain.Responses;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class SignInCommand : IRequest<ApiResponse<JwtAuthResponse>>
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }
}
