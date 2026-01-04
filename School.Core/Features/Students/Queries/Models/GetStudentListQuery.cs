using MediatR;
using School.Core.ApiResponse;
using School.Core.Features.Students.Queries.Response;

namespace School.Core.Features.Students.Queries.Models
{
    public  class GetStudentListQuery: IRequest<ApiResponse< List<GetStudentListResponse>>>
    {
    }
}
