namespace School.Domain.AppRoutes
{
    public static class AppRouter
    {

        private const string root = "Api";
        private const string version = "V1";
        private const string Rule = root + "/" + version + "/";



        public static class StudentRouting
        {
            private const string Prefix = Rule + "Student";
            public const string GetStudentList = Prefix + "/List";
            public const string GetStudentByID = Prefix + "/{Id}";
            public const string AddStudent = Prefix + "/Add";
            public const string EditStudent = Prefix + "/Edit";
            public const string DeleteStudent = Prefix + "/{id}";
            public const string Paginated = Prefix + "/Paginated";


        }
        public static class DepartmentRouting
        {
            private const string Prefix = Rule + "Department";
            public const string GetDepartmentByID = Prefix + "/Id";



        }
        public static class UserRouting
        {
            private const string Prefix = Rule + "User";
            public const string Create = Prefix + "/Create";
            public const string GetUserByID = Prefix + "/{Id}";
            public const string Paginated = Prefix + "/Paginated";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{Id}";
            public const string ChangePassword = Prefix + "/ChangePassword";
        }
        public static class AuthenticationRouting
        {
            private const string Prefix = Rule + "Auth";
            public const string SignIn = Prefix + "/SignIn";
            public const string RefreshToken = Prefix + "/RefreshToken";
            public const string ConfirmEmail = Prefix + "/ConfirmEmail";
            public const string SendResetPasswordCode = Prefix + "/SendResetPasswordCode";
            public const string ConfirmResetPasswordCode = Prefix + "/ConfirmResetPasswordCode";
            public const string ResetPassword = Prefix + "/ResetPassword";

        }
        public static class AuthorizationRouting
        {
            private const string Prefix = Rule + "Role";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{Id}";
            public const string RoleList = Prefix + "/Role-List";
            public const string GetRoleById = Prefix + "/Role-By-Id/{id}";
            public const string ManageUserRoles = Prefix + "/{userId}";
            public const string UpdateUserRoles = Prefix + "/Update-User-Roles";
            public const string ManageUserClaims = Prefix + "/UserClaims/{userId}";
            public const string UpdateUserClaims = Prefix + "/UserClaims/Update";


        }

        public static class EmailsRoute
        {
            private const string Prefix = Rule + "Email";
            public const string SendEmail = Prefix + "/SendEmail";
        }
    }
}
