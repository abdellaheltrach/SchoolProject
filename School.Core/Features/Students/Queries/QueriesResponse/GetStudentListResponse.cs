namespace School.Core.Features.Students.Queries.Response
{
    public class GetStudentListResponse
    {
        public int StudentID { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? DepartementName { get; set; }

    }
}
