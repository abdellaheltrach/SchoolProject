using School.Core.Features.Instructors.Commands.Models;
using School.Domain.Entities;

namespace School.Core.Mapping.InstructorMapping
{
    public partial class InstructorProfile
    {
        public void AddInstructorMapping()
        {
            CreateMap<AddInstructorCommand, Instructor>()
                 .ForMember(dest => dest.Image, opt => opt.Ignore());
        }

    }
}

