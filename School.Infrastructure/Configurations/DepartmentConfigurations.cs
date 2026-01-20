using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Primary key
        builder.HasKey(d => d.Id);

        // Identity column
        builder.Property(d => d.Id)
               .ValueGeneratedOnAdd();

        // Column lengths
        builder.Property(d => d.DepartmentNameAr).HasMaxLength(500);
        builder.Property(d => d.DepartmentNameEn).HasMaxLength(200);

        // Instructor Manager (1-0..1 relationship)
        builder.HasOne(d => d.InstructorManager)
               .WithOne(i => i.ManagedDepartment)
               .HasForeignKey<Department>(d => d.InstructorManagerId)
               .OnDelete(DeleteBehavior.Restrict);

        // Students (1-many)
        builder.HasMany(d => d.Students)
               .WithOne(s => s.Department)
               .HasForeignKey(s => s.DepartementId)
               .OnDelete(DeleteBehavior.SetNull);

        // DepartmentSubjects (1-many)
        builder.HasMany(d => d.DepartmentSubjects)
               .WithOne(ds => ds.Department)
               .HasForeignKey(ds => ds.DepartementId)
               .OnDelete(DeleteBehavior.Cascade);

        // Instructors (1-many)
        builder.HasMany(d => d.Instructors)
               .WithOne(i => i.Department)
               .HasForeignKey(i => i.DepartementId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
