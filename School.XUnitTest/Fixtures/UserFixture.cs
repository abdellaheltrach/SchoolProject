using School.Domain.Entities.Identity;

namespace School.Tests.Fixtures
{
    public static class UserFixture
    {
        public static User CreateUser(int id = 1, string email = "test@example.com")
        {
            return new User
            {
                Id = id,
                UserName = email.Split('@')[0], // derived username
                NormalizedUserName = email.Split('@')[0].ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAE...", // Mock hash
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                // Custom properties you might have added:
                FullName = "John Doe",
                Address = "123 Main St",
            };
        }

        public static List<User> CreateUserList(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => CreateUser(i, $"user{i}@test.com"))
                .ToList();
        }
    }
}
