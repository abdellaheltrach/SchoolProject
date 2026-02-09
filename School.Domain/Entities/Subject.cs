namespace School.Domain.Entities
{
    public class Subject
    {
        public Subject()
        {
            StudentsSubjects = new HashSet<StudentSubject>();
            DepartmetsSubjects = new HashSet<DepartmentSubject>();
            InstructorSubjects = new HashSet<InstructorSubject>();
        }

        public int Id { get; set; }
        public string? SubjectNameAr { get; set; }
        public string? SubjectNameEn { get; set; }
        public int? Period { get; set; }

        public virtual ICollection<StudentSubject> StudentsSubjects { get; set; }
        public virtual ICollection<DepartmentSubject> DepartmetsSubjects { get; set; }
        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
    }

}
