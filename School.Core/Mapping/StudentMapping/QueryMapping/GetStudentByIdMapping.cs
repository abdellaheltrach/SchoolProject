using AutoMapper;
using School.Core.Features.Students.Queries.QueriesResponse;
using School.Domain.Entities;

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
