using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Autorazation.Commands.Models
{
    public class AddRoleCommand : IRequest<ApiResponse<string>>
    {
        public string RoleName { get; set; }
    }
}
