using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Entities
{
    public class Instructor
    {
        public Instructor()
        {
            InstructorSubjects = new HashSet<InstructorSubject>();
            Subordinates = new HashSet<Instructor>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstructorId { get; set; }

        public string? InstructorNameAr { get; set; }
        public string? InstructorNameEn { get; set; }

        public string? Address { get; set; }
        public string? Position { get; set; }

        public int? SupervisorId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        public string? Image { get; set; }

        public int DepartementID { get; set; }

        /* -------------------- Department (Instructor belongs to Department) -------------------- */

        [ForeignKey(nameof(DepartementID))]
        [InverseProperty(nameof(Department.Instructors))]
        public Department? Department { get; set; }

        /* -------------------- Department Manager (1–1 or 1–0..1) -------------------- */

        [InverseProperty(nameof(Department.InstructorManager))]
        public Department? ManagedDepartment { get; set; }

        /* -------------------- Self-referencing Supervisor relationship -------------------- */

        [ForeignKey(nameof(SupervisorId))]
        [InverseProperty(nameof(Subordinates))]
        public Instructor? Supervisor { get; set; }

        [InverseProperty(nameof(Supervisor))]
        public ICollection<Instructor> Subordinates { get; set; }

        /* -------------------- Instructor ↔ Subject (Many-to-Many) -------------------- */

        [InverseProperty(nameof(InstructorSubject.Instructor))]
        public ICollection<InstructorSubject> InstructorSubjects { get; set; }
    }
}
