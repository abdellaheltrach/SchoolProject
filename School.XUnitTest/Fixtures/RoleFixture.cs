using School.Domain.Entities.Identity;

namespace School.Tests.Fixtures
{
    public static class RoleFixture
    {
        public static Role CreateRole(int id = 1, string name = "Admin")
        {
            return new Role
            {
                Id = id,
                Name = name,
                NormalizedName = name.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
        }

        public static List<Role> CreateRoleList(int count)
        {
            var roles = new List<string> { "Admin", "User", "Instructor", "Student" };
            return Enumerable.Range(1, count)
                .Select(i => CreateRole(i, roles[i % roles.Count]))
                .ToList();
        }
    }
}
