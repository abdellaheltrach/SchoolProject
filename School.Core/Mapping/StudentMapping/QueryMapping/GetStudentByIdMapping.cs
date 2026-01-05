using AutoMapper;
using School.Core.Features.Students.Queries.QueriesResponse;
using School.Core.Features.Students.Queries.Response;
using School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Mapping.StudentMapping
{
    public partial class StudentProfile : Profile
    {
        public void GetStudentByIdMapping()
        {
            CreateMap<Student, GetStudentByIdResponse>()
            .ForMember(response => response.DepartementName,
            options => options.MapFrom(src => src.Department != null ? src.Department.DNameEn : "No Department"));

        }
    }
}
