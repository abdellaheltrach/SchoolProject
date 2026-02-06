using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Instructors.Queries.Models
{
    public class GetSummationSalaryOfInstructorQuery : IRequest<ApiResponse<object>>
    {
    }
}
