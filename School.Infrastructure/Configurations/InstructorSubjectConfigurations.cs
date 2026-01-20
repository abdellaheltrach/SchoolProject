using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

public class InstructorSubjectConfiguration : IEntityTypeConfiguration<InstructorSubject>
{
    public void Configure(EntityTypeBuilder<InstructorSubject> builder)
    {
        // Primary key
        builder.HasKey(isub => isub.InsSubId);

        // Identity column
        builder.Property(isub => isub.InsSubId)
               .ValueGeneratedOnAdd();

        // Relationship: InstructorSubject → Instructor (many-to-one)
        builder.HasOne(isub => isub.Instructor)
               .WithMany(i => i.InstructorSubjects)
               .HasForeignKey(isub => isub.InstructorId)
               .OnDelete(DeleteBehavior.Cascade);

        // Relationship: InstructorSubject → Subject (many-to-one)
        builder.HasOne(isub => isub.Subject)
               .WithMany(s => s.InstructorSubjects)
               .HasForeignKey(isub => isub.SubjectId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
