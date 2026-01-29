using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class EditRoleCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
        public String NewRoleName { get; set; }
    }


}
