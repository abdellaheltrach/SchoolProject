using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Authentication.Queries.Models
{
    public class EmailConfirmationQuery : IRequest<ApiResponse<string>>
    {
        public int UserId { get; set; }
        public string code { get; set; }


    }


}
