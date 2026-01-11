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
    }
}
