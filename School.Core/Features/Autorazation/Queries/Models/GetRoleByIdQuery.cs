using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Autorazation.Queries.QueriesResponse;

namespace School.Core.Features.Autorazation.Queries.Models
{
    public class GetRoleByIdQuery : IRequest<ApiResponse<GetRoleByIdResponse>>
    {
        public int Id { get; set; }
    }
}
