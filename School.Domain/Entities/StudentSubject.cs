using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace School.Domain.Entities
{
    public class StudentSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StudentID { get; set; }

        public int SubjectID { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? grade { get; set; }

        [ForeignKey("StudentID")]
        [InverseProperty("StudentSubjects")]
        public virtual Student? Student { get; set; }

        [ForeignKey("SubjectID")]
        [InverseProperty("StudentsSubjects")]
        public virtual Subject? Subject { get; set; }

    }
}
