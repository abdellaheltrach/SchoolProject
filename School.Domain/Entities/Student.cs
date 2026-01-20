namespace School.Domain.Entities
{
    public class Student
    {
        public Student()
        {
            StudentSubjects = new HashSet<StudentSubject>();
        }

        public int StudentID { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int? DepartementId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }

}
