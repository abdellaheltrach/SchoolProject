namespace School.Core.Resources
{
    public static class SharedResourceskeys
    {

        //public const string AlreadyExists = "AlreadyExists";
        //public const string UpdateSuccess = "UpdateSuccess";


        //api response hundler messages
        public const string Success = "Success";
        public const string NotFound = "NotFound"; //used in error handling middleware as well
        public const string UnAuthorized = "UnAuthorized"; //used in error handling middleware as well
        public const string BadRequest = "BadRequest"; //used in error handling middleware as well
        public const string UnprocessableEntity = "UnprocessableEntity";
        public const string DeleteSuccess = "DeleteSuccess";
        public const string CreateSuccess = "CreateSuccess";

        //validation error middleware messages
        public const string ValidationFailed = "ValidationFailed";
        public const string DatabaseUpdateError = "DatabaseUpdateError";
        public const string InternalServerError = "InternalServerError";


    }
}
