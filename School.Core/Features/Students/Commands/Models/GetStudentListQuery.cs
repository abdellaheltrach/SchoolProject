using MediatR;
using School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Features.Students.Commands.Models
{
    public  class GetStudentListQuery: IRequest<List<Student>>
    {
    }
}
