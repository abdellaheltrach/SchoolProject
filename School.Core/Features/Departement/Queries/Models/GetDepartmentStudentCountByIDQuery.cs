using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Features.Departement.Queries.QueriesResponse;

namespace School.Core.Features.Departement.Queries.Models
{
    public class GetDepartmentStudentCountByIDQuery : IRequest<ApiResponse<GetDepartmentStudentCountByIDResponse>>
    {
        public int DID { get; set; }

    }
}
