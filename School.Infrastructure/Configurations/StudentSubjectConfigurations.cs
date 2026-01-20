using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

public class StudentSubjectConfiguration : IEntityTypeConfiguration<StudentSubject>
{
    public void Configure(EntityTypeBuilder<StudentSubject> builder)
    {
        // Primary key
        builder.HasKey(ss => ss.Id);

        // Identity column
        builder.Property(ss => ss.Id)
               .ValueGeneratedOnAdd();

        // Grade precision
        builder.Property(ss => ss.Grade)
               .HasColumnType("decimal(5,2)");

        // Relationship: StudentSubject → Student (many-to-one)
        builder.HasOne(ss => ss.Student)
               .WithMany(s => s.StudentSubjects)
               .HasForeignKey(ss => ss.StudentId)
               .OnDelete(DeleteBehavior.Cascade);

        // Relationship: StudentSubject → Subject (many-to-one)
        builder.HasOne(ss => ss.Subject)
               .WithMany(s => s.StudentsSubjects)
               .HasForeignKey(ss => ss.SubjectId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
