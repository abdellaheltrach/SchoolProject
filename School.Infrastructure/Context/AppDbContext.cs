using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Department> departments { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<DepartmetSubject> departmetSubjects { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<StudentSubject> studentSubjects { get; set; }
    }

}
