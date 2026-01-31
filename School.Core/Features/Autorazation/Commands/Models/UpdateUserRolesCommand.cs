using MediatR;
using School.Core.Base.ApiResponse;
using School.Domain.Results;

namespace School.Core.Features.Autorazation.Commands.Models
{
    public class UpdateUserRolesCommand : IRequest<ApiResponse<string>>
    {

        public int UserId { get; set; }
        public List<UserRoles> userRoles { get; set; }


    }
}
