using Microsoft.AspNetCore.Identity;

namespace School.Domain.Entities.Identity
{
    // Make sure you have referenced the Microsoft.AspNetCore.Identity.EntityFrameworkCore package in your project.
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
    }
}
