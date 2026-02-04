

using School.Core.Features.Departement.Queries.Models;
using School.Core.Features.Departement.Queries.QueriesResponse;
using School.Domain.Entities.Procedures;

namespace School.Core.Mapping.DepartmentMapping
{
    public partial class DepartementProfile
    {
        public void GetDepartmentStudentCountByIdMapping()
        {
            CreateMap<GetDepartmentStudentCountByIDQuery, DepartmentStudentCountProcedureParameters>()
                .ForMember(dest => dest.DepartmentId, Opt => Opt.MapFrom(src => src.DID));

            CreateMap<DepartmentStudentCountProcedure, GetDepartmentStudentCountByIDResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => Helpers.LocalizationHelper.GetLocalizedName(src.DepartmentNameAr, src.DepartmentNameEn)))
                .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.TotalStudents));
        }
    }
}