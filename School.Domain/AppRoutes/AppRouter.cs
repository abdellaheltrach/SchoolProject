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
            private const string Prefix = Rule + "Authentication";
            public const string SignIn = Prefix + "/SignIn";
            public const string RefreshToken = Prefix + "/RefreshToken";

        }
        public static class AuthorizationRouting
        {
            private const string Prefix = Rule + "Authorization";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";

        }
    }
}
