using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Departement.Queries.QueriesResponse;

namespace School.Core.Features.Departement.Queries.Models
{
    public class GetDepartementByIdQuery : IRequest<ApiResponse<GetDepartmentByIdResponse>>
    {
        public int ID { get; set; }
        public int StudentPageSize { get; set; }
        public int StudentPageNumber { get; set; }


    }

}
