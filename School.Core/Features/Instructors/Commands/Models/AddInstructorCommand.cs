using MediatR;
using Microsoft.AspNetCore.Http;
using School.Core.Base.ApiResponse;

namespace School.Core.Features.Instructors.Commands.Models
{
    public class AddInstructorCommand : IRequest<ApiResponse<string>>
    {
        public string? InstructorNameAr { get; set; }
        public string? InstructorNameEn { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public int? SupervisorId { get; set; }
        public decimal? Salary { get; set; }
        public int DepartementId { get; set; }
        public IFormFile? Image { get; set; }
    }

}
