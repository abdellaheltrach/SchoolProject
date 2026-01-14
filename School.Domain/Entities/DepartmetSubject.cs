using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace School.Domain.Entities
{
    public class DepartmetSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int DepartementID { get; set; }

        public int SubjectID { get; set; }

        [ForeignKey(nameof(DepartementID))]
        [InverseProperty(nameof(Department.DepartmentSubjects))]
        public virtual Department? Department { get; set; }

        [ForeignKey(nameof(SubjectID))]
        [InverseProperty(nameof(Subject.DepartmetsSubjects))]
        public virtual Subject? Subject { get; set; }
    }
}
