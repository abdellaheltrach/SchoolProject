using MediatR;
using School.Core.Features.Students.Commands.Models;
using School.Domain.Entities;
using School.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Features.Students.Commands.Hundlers
{
    public class StudentHundler : IRequestHandler<GetStudentListQuery, List<Student>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        #endregion
        #region Constructors
        public StudentHundler(IStudentService studentService)
        {
            this._studentService = studentService;
        }
        #endregion
        #region Hunder
        public async Task<List<Student>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            return await _studentService.GetAllStudentListAsync();
        }
        #endregion
    }
}
