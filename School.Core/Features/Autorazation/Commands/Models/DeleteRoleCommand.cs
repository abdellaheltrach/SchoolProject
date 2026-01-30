using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Autorazation.Commands.Models
{
    public class DeleteRoleCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
        public DeleteRoleCommand(int id)
        {
            Id = id;
        }
    }
}
