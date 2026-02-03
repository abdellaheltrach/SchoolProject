using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Authentication.Queries.Models
{
    public class ConfirmResetPasswordQuery : IRequest<ApiResponse<string>>
    {
        public string Code { get; set; }
        public string Email { get; set; }
    }
}
