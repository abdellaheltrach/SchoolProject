using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Entities
{
    public class Subject
    {
        public Subject()
        {
            StudentsSubjects = new HashSet<StudentSubject>();
            DepartmetsSubjects = new HashSet<DepartmetSubject>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(500)]
        public string? SubjectNameAr { get; set; }
        [StringLength(500)]
        public string? SubjectNameEn { get; set; }
        public int? Period { get; set; }
        [InverseProperty(nameof(StudentSubject.Subject))]
        public virtual ICollection<StudentSubject> StudentsSubjects { get; set; }
        [InverseProperty(nameof(DepartmetSubject.Subject))]
        public virtual ICollection<DepartmetSubject> DepartmetsSubjects { get; set; }

    }
}
