using MediatR;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Students.Commands.Models
{
    public class AddStudentCommand : IRequest<ApiResponse<string>>
    {

        public string? NameAr { get; set; }
        public string NameEn { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int? DepartementID { get; set; }

    }
}
/*
        public int StudentID { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        [StringLength(500)]
        public string? Address { get; set; }
        [StringLength(500)]
        public string? Phone { get; set; }
        public int? DepartementID { get; set; }
 */