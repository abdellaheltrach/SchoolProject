using AutoMapper;
using School.Core.Features.Students.Queries.Response;
using School.Domain.Entities;

namespace School.Core.Mapping.StudentMapping
{
    public partial class StudentProfile : Profile
    {
        public void GetStudentListMapping()
        {
            CreateMap<Student, GetStudentListResponse>()
            .ForMember(response => response.DepartementName,
            options => options.MapFrom(src => src.Department != null ? src.Department.DNameEn : "No Department"));

        }
    }
}
