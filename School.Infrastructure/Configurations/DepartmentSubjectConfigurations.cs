using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

public class DepartmetSubjectConfiguration : IEntityTypeConfiguration<DepartmentSubject>
{
    public void Configure(EntityTypeBuilder<DepartmentSubject> builder)
    {
        // Primary key
        builder.HasKey(ds => ds.Id);

        // Identity column
        builder.Property(ds => ds.Id)
               .ValueGeneratedOnAdd();

        // Department relationship
        builder.HasOne(ds => ds.Department)
               .WithMany(d => d.DepartmentSubjects)
               .HasForeignKey(ds => ds.DepartementId)
               .OnDelete(DeleteBehavior.Restrict);

        // Subject relationship
        builder.HasOne(ds => ds.Subject)
               .WithMany(s => s.DepartmetsSubjects)
               .HasForeignKey(ds => ds.SubjectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
