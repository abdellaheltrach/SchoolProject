using AutoMapper;
using MediatR;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.Response;
using School.Domain.Entities;
using School.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Features.Students.Queries.Hundlers
{
    public class StudentHundler : IRequestHandler<GetStudentListQuery, List<GetStudentListResponse>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        #endregion
        #region Constructors
        public StudentHundler(IStudentService studentService, IMapper mapper)
        {
            this._studentService = studentService;
            _mapper = mapper;
        }
        #endregion
        #region Hunder
        public async Task<List<GetStudentListResponse>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentList = await _studentService.GetAllStudentListAsync();
            var studentListResponseMapper = _mapper.Map<List<GetStudentListResponse>>(studentList);
            return studentListResponseMapper;
        }
        #endregion
    }
}
