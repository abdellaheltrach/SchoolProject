using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Primary key
        builder.HasKey(s => s.StudentID);

        // Identity column
        builder.Property(s => s.StudentID)
               .ValueGeneratedOnAdd();

        // Column lengths
        builder.Property(s => s.NameAr)
               .HasMaxLength(500);

        builder.Property(s => s.NameEn)
               .HasMaxLength(500);

        builder.Property(s => s.Address)
               .HasMaxLength(500);

        builder.Property(s => s.Phone)
               .HasMaxLength(500);

        // Relationship: Student → Department (many-to-one)
        builder.HasOne(s => s.Department)
               .WithMany(d => d.Students)
               .HasForeignKey(s => s.DepartementId)
               .OnDelete(DeleteBehavior.SetNull); // optional: set null when department deleted

        // Relationship: Student → StudentSubject (one-to-many)
        builder.HasMany(s => s.StudentSubjects)
               .WithOne(ss => ss.Student)
               .HasForeignKey(ss => ss.StudentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
