using AutoMapper;
using School.Core.Features.Students.Commands.Models;
using School.Domain.Entities;

namespace School.Core.Mapping.StudentMapping
{
    public partial class StudentProfile : Profile
    {
        public void EditStudentCommandMapping()
        {
            CreateMap<EditStudentCommand, Student>();
        }
    }
}
