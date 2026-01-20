namespace School.Domain.Entities
{
    public class InstructorSubject
    {
        public int InsSubId { get; set; }
        public int InstructorId { get; set; }
        public int SubjectId { get; set; }

        public virtual Instructor? Instructor { get; set; }
        public virtual Subject? Subject { get; set; }
    }

}
