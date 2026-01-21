using School.Core.Features.Departement.Queries.QueriesResponse;
using School.Core.Helpers;
using School.Domain.Entities;


namespace School.Core.Mapping.DepartmentMapping
{
    public partial class DepartementProfile
    {

        public void GetDepartmentByIdMapping()
        {

            CreateMap<Department, GetDepartmentByIdResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => LocalizationHelper.GetLocalizedName(src.DepartmentNameAr, src.DepartmentNameEn)))
                 .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => LocalizationHelper.GetLocalizedName(src.InstructorManager.InstructorNameAr, src.InstructorManager.InstructorNameEn)))
                 //need map between DepartmetSubject and SubjectResponse
                 .ForMember(dest => dest.SubjectList, opt => opt.MapFrom(src => src.DepartmentSubjects))
                 //need map between Student and StudentResponse
                 .ForMember(dest => dest.StudentList, opt => opt.MapFrom(src => src.Students))
                 //need map between Instructor and InstructorResponse
                 .ForMember(dest => dest.InstructorList, opt => opt.MapFrom(src => src.Instructors));

            CreateMap<DepartmetSubject, SubjectResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => LocalizationHelper.GetLocalizedName(src.Subject.SubjectNameAr, src.Subject.SubjectNameEn)));

            CreateMap<Student, StudentResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => LocalizationHelper.GetLocalizedName(src.NameAr, src.NameEn)));

            CreateMap<Instructor, InstructorResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InstructorId))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => LocalizationHelper.GetLocalizedName(src.InstructorNameAr, src.InstructorNameEn)));
        }
    }
}
