using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Entities.Identity;
using System.Reflection;

namespace School.Infrastructure.Context
{
    //IdentityDbContext requires Microsoft.AspNetCore.Identity.EntityFrameworkCore package
    //change PK type to be int rather than string by specifying IdentityX<int> 
    public class AppDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        private readonly IEncryptionProvider _encryptionProvider;

        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _encryptionProvider = new GenerateEncryptionProvider("8a4dcaaec64d412380fe4b02193cd26f");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRefreshToken> userRefreshTokens { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<DepartmetSubject> departmetSubjects { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<StudentSubject> studentSubjects { get; set; }
        public DbSet<Instructor> instructors { get; set; }
        public DbSet<InstructorSubject> instructorSubjects { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //apply entities configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.UseEncryption(_encryptionProvider);


        }
    }

}
