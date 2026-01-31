using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Autorazation.Queries.QueriesResponse;

namespace School.Core.Features.Autorazation.Queries.Models
{
    public class GetRolesListQuery : IRequest<ApiResponse<List<GetRolesListResponse>>>
    {
    }
}
