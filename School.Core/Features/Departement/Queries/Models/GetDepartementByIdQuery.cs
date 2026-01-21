using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Departement.Queries.QueriesResponse;

namespace School.Core.Features.Departement.Queries.Models
{
    public class GetDepartementByIdQuery : IRequest<ApiResponse<GetDepartmentByIdResponse>>
    {
        public readonly int ID;
        public GetDepartementByIdQuery(int Id)
        {
            ID = Id;
        }
    }

}
