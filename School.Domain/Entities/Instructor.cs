namespace School.Domain.Entities
{
    public class Instructor
    {
        public Instructor()
        {
            InstructorSubjects = new HashSet<InstructorSubject>();
            Subordinates = new HashSet<Instructor>();
        }

        public int InstructorId { get; set; }
        public string? InstructorNameAr { get; set; }
        public string? InstructorNameEn { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public int? SupervisorId { get; set; }
        public decimal? Salary { get; set; }
        public string? Image { get; set; }
        public int DepartementId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Department? ManagedDepartment { get; set; }
        public virtual Instructor? Supervisor { get; set; }
        public virtual ICollection<Instructor> Subordinates { get; set; }
        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
    }

}
