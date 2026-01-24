namespace School.Core.Resources
{
    public static class SharedResourcesKeys
    {




        //api response hundler messages
        public const string Success = "Success";
        public const string NotFound = "NotFound"; //used in error handling middleware as well
        public const string UnAuthorized = "UnAuthorized"; //used in error handling middleware as well
        public const string BadRequest = "BadRequest"; //used in error handling middleware as well
        public const string UnprocessableEntity = "UnprocessableEntity";
        public const string DeleteSuccess = "DeleteSuccess";
        public const string CreateSuccess = "CreateSuccess";
        public const string Deleted = "Deleted";
        public const string DeletedFailed = "DeletedFailed";



        //validation error middleware messages
        public const string ValidationFailed = "ValidationFailed";
        public const string DatabaseUpdateError = "DatabaseUpdateError";
        public const string InternalServerError = "InternalServerError";

        //validators error messages


        public const string PasswordNotEqualConfirmationPassword = "PasswordNotEqualConfirmationPassword";
        public const string PasswordTooWeak = "PasswordTooWeak";
        public const string InvalidEmail = "InvalidEmail";

        //generic validation messages
        public const string PropNotExist = "PropNotExist";
        public const string PropRequired = "PropRequired";
        public const string PropNotEmpty = "PropNotEmpty";
        public const string PropMaxLengthis100 = "PropMaxLengthis100";
        public const string PropAlreadyExists = "PropAlreadyExists";
        public const string PropMustBeGreaterThanZero = "PropMustBeGreaterThanZero";


        //properties names
        public const string NameAr = "NameAr";
        public const string NameEn = "NameEn";
        public const string Address = "Address";
        public const string DepartementID = "DepartementID";
        public const string Phone = "Phone";
        public const string StudentID = "StudentID";


        //CQRS Handlers specific messages
        public const string UserCreationFailed = "UserCreationFailed";
        public const string UserNameIsExist = "UserNameIsExist";
        public const string UpdateUserFailed = "UpdateUserFailed";
        public const string UserUpdated = "UserUpdated";





    }
}
