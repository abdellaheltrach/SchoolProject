using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Base.Wrappers;
using School.Core.Features.Users.Queries.Response;

namespace School.Core.Features.Users.Queries.Models
{
    public class GetPaginatedUsersListQuery : IRequest<ApiResponse<PaginatedResult<GetPaginatedUsersListResponse>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
