using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Users.Queries.Response;

namespace School.Core.Features.Users.Queries.Models
{
    public class GetUserByIdQuery : IRequest<ApiResponse<GetUserByIdQueryResponse>>
    {
        public int Id { get; set; }
        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
