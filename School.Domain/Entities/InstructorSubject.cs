using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Entities
{
    public class InstructorSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Ins_SubId { get; set; }
        public int InstructorId { get; set; }

        public int SubjectId { get; set; }

        [ForeignKey(nameof(InstructorId))]
        [InverseProperty(nameof(Instructor.InstructorSubjects))]
        public Instructor? Instructor { get; set; }


        [ForeignKey(nameof(SubjectId))]
        [InverseProperty(nameof(Subject.Ins_Subjects))]
        public Subject? Subject { get; set; }
    }
}
