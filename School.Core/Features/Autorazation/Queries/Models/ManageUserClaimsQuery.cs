using MediatR;
using School.Core.Base.ApiResponse;
using School.Domain.Results;

namespace School.Core.Features.Autorazation.Queries.Models
{
    public class ManageUserClaimsQuery : IRequest<ApiResponse<ManageUserClaimsResult>>
    {
        public int UserId { get; set; }
    }
}
