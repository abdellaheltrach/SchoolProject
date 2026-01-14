using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace School.Domain.Entities
{
    public partial class Department
    {
        public Department()
        {
            Students = new HashSet<Student>();
            DepartmentSubjects = new HashSet<DepartmetSubject>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? DepartmentNameAr { get; set; }
        [StringLength(200)]
        public string? DepartmentNameEn { get; set; }

        public int? InsManager { get; set; }

        [InverseProperty(nameof(Student.Department))]
        public virtual ICollection<Student> Students { get; set; }
        [InverseProperty(nameof(DepartmetSubject.Department))]
        public virtual ICollection<DepartmetSubject> DepartmentSubjects { get; set; }



    }
}
