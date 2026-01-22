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

        //validators error messages
        public const string NameArRequired = "NameArRequired";
        public const string NameArMaxLength = "NameArMaxLength";
        public const string NameEnRequired = "NameEnRequired";
        public const string NameEnMaxLength = "NameEnMaxLength";
        public const string AddressRequired = "AddressRequired";
        public const string AddressMaxLength = "AddressMaxLength";
        public const string DepartementIDRequired = "DepartementIDRequired";
        public const string NameArExists = "NameArExists";
        public const string NameEnExists = "NameEnExists";
        public const string PropNotExist = "PropNotExist";


        //properties names
        public const string NameAr = "NameAr";
        public const string NameEn = "NameEn";
        public const string Address = "Address";
        public const string DepartementID = "DepartementID";
        public const string Phone = "Phone";
        public const string StudentID = "StudentID";






    }
}
