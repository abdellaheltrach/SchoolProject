using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Entities
{
    public class Student
    {
        public Student()
        {
            StudentSubjects = new HashSet<StudentSubject>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }
        [StringLength(500)]
        public string? NameAr { get; set; }
        [StringLength(500)]
        public string? NameEn { get; set; }
        [StringLength(500)]
        public string? Address { get; set; }
        [StringLength(500)]
        public string? Phone { get; set; }
        [ForeignKey("DepartementID")]
        public int? DepartementID { get; set; }

        [InverseProperty(nameof(Department.Students))]
        public virtual Department? Department { get; set; }
        [InverseProperty(nameof(StudentSubject.Student))]
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
