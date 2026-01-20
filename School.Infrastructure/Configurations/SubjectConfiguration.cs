using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        // Primary key
        builder.HasKey(s => s.Id);

        // Identity column
        builder.Property(s => s.Id)
               .ValueGeneratedOnAdd();

        // Column lengths
        builder.Property(s => s.SubjectNameAr)
               .HasMaxLength(500);

        builder.Property(s => s.SubjectNameEn)
               .HasMaxLength(500);

        // Relationships

        // Subject ↔ StudentSubject (many-to-many via join entity)
        builder.HasMany(s => s.StudentsSubjects)
               .WithOne(ss => ss.Subject)
               .HasForeignKey(ss => ss.SubjectId)
               .OnDelete(DeleteBehavior.Cascade);

        // Subject ↔ DepartmetSubject (many-to-many via join entity)
        builder.HasMany(s => s.DepartmetsSubjects)
               .WithOne(ds => ds.Subject)
               .HasForeignKey(ds => ds.SubjectId)
               .OnDelete(DeleteBehavior.Cascade);

        // Subject ↔ InstructorSubject (many-to-many via join entity)
        builder.HasMany(s => s.InstructorSubjects)
               .WithOne(isub => isub.Subject)
               .HasForeignKey(isub => isub.SubjectId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
