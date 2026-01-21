using AutoMapper;

namespace School.Core.Mapping.DepartmentMapping
{
    public partial class DepartementProfile : Profile
    {
        public DepartementProfile()
        {
            GetDepartmentByIdMapping();
        }
    }
}
