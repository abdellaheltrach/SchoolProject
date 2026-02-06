

using School.Core.Features.Departement.Queries.QueriesResponse;
using School.Core.Helpers;
using School.Domain.Entities.Views;

namespace School.Core.Mapping.DepartmentMapping
{
    public partial class DepartementProfile
    {
        public void GetDepartmentStudentCountMapping()
        {
            CreateMap<DepartementTotalStudentView, GetDepartmentStudentListCountResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => LocalizationHelper.GetLocalizedName(src.DepartmentNameAr, src.DepartmentNameEn)))
                .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.TotalStudents));
        }
    }
}