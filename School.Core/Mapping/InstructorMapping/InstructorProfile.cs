using AutoMapper;

namespace School.Core.Mapping.InstructorMapping
{
    public partial class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            AddInstructorMapping();
        }
    }
}

