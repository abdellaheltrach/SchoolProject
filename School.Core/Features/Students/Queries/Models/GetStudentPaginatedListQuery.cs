using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Base.Wrappers;
using School.Core.Features.Students.Queries.QueriesResponse;
using School.Domain.enums;

namespace School.Core.Features.Students.Queries.Models
{
    public class GetStudentPaginatedListQuery : IRequest<ApiResponse<PaginatedResult<GetStudentPaginatedListResponse>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public StudentOrdringEnum OrderBy { get; set; }
        public bool SortDesc { get; set; } = false;

        public string? Search { get; set; }
    }
}
