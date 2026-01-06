using MediatR;
using School.Core.ApiResponse;
using System.ComponentModel.DataAnnotations;

namespace School.Core.Features.Students.Commands.Models
{
    public class AddStudentCommand : IRequest<ApiResponse<string>>
    {
        [Required]
        public string? NameAr { get; set; }
        [Required]
        public string NameEn { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(500)]

        public string Phone { get; set; }
        [Required]
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