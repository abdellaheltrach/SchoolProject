using Microsoft.AspNetCore.Identity;
using Moq;

namespace School.Tests.Helper
{
    public static class MockHelper
    {
        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            // Ensure the mock is set up for common Identity behaviors
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            return mgr;
        }

        public static Mock<RoleManager<TRole>> MockRoleManager<TRole>() where TRole : class
        {
            var store = new Mock<IRoleStore<TRole>>();
            var mgr = new Mock<RoleManager<TRole>>(store.Object, null!, null!, null!, null!);
            return mgr;
        }
    }
}
