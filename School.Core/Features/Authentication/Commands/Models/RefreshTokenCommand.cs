using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Authentication.Commands.Response;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class RefreshTokenCommand : IRequest<ApiResponse<TokenResponse>>
    {
        public string AccessToken { get; set; }
    }
}
