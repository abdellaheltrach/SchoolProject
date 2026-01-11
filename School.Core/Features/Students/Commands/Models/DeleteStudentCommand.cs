using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Students.Commands.Models
{
    public class DeleteStudentCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }

        public DeleteStudentCommand(int id)
        {
            Id = id;
        }
    }

}
