using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class SignOutCommand : IRequest<ApiResponse<string>>
    {
    }
}
