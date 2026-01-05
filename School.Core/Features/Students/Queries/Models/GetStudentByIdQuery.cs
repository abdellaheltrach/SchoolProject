using MediatR;
using School.Core.ApiResponse;
using School.Core.Features.Students.Queries.QueriesResponse;
using School.Core.Features.Students.Queries.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Features.Students.Queries.Models
{
    public class GetStudentByIdQuery: IRequest<ApiResponse<GetStudentByIdResponse>>
    {
        public readonly int ID;

        public GetStudentByIdQuery(int Id)
        {
            ID = Id;
        }
    }
}
