using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        // Primary key
        builder.HasKey(i => i.InstructorId);

        // Identity column
        builder.Property(i => i.InstructorId)
               .ValueGeneratedOnAdd();

        // Column lengths
        builder.Property(i => i.InstructorNameAr).HasMaxLength(500);
        builder.Property(i => i.InstructorNameEn).HasMaxLength(500);
        builder.Property(i => i.Address).HasMaxLength(500);
        builder.Property(i => i.Position).HasMaxLength(200);

        // Salary precision
        builder.Property(i => i.Salary)
               .HasColumnType("decimal(18,2)");

        // Instructor → Department (many-to-one)
        // Changed to Restrict to avoid cascade conflicts
        builder.HasOne(i => i.Department)
               .WithMany(d => d.Instructors)
               .HasForeignKey(i => i.DepartementId)
               .OnDelete(DeleteBehavior.Restrict);

        // Instructor → Managed Department (1–0..1)
        // Keep as Restrict
        builder.HasOne(i => i.ManagedDepartment)
               .WithOne(d => d.InstructorManager)
               .HasForeignKey<Department>(d => d.InstructorManagerId)
               .OnDelete(DeleteBehavior.Restrict);

        // Instructor → Supervisor (self-referencing)
        // Keep as Restrict (NEVER Cascade or SetNull for self-referencing)
        builder.HasOne(i => i.Supervisor)
               .WithMany(s => s.Subordinates)
               .HasForeignKey(i => i.SupervisorId)
               .OnDelete(DeleteBehavior.Restrict);

        // Instructor → InstructorSubjects (join table)
        // Cascade is fine here
        builder.HasMany(i => i.InstructorSubjects)
               .WithOne(isub => isub.Instructor)
               .HasForeignKey(isub => isub.InstructorId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}