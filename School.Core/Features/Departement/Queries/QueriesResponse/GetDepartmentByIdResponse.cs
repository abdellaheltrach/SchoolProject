using School.Core.Base.Wrappers;

namespace School.Core.Features.Departement.Queries.QueriesResponse
{
    public class GetDepartmentByIdResponse
    {
        public int Id { get; set; }

        public string? DepartmentName { get; set; }

        public string? ManagerName { get; set; }

        public List<SubjectResponse>? SubjectList { get; set; }

        public PaginatedResult<StudentResponse>? StudentList { get; set; }

        public List<InstructorResponse>? InstructorList { get; set; }
    }


    public class StudentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StudentResponse()
        {

        }
        public StudentResponse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public class SubjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class InstructorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
