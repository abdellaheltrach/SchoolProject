using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Users.Commands.Models
{
    public class DeleteUserCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
