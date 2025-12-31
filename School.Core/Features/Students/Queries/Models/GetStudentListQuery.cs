using MediatR;
using School.Core.Features.Students.Queries.Response;
using School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Features.Students.Queries.Models
{
    public  class GetStudentListQuery: IRequest<List<GetStudentListResponse>>
    {
    }
}
