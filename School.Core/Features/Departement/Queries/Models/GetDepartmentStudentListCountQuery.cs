using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Departement.Queries.QueriesResponse;

namespace School.Core.Features.Departement.Queries.Models
{
    public class GetDepartmentStudentListCountQuery : IRequest<ApiResponse<List<GetDepartmentStudentListCountResponse>>>
    {
    }
}
