using Microsoft.Extensions.Localization;
using School.Core.Resources;

namespace School.Core.Base.ApiResponse
{
    public class ApiResponseHandler
    {
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public ApiResponseHandler(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
        public ApiResponse<T> Deleted<T>(string message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = string.IsNullOrWhiteSpace(message) ? _stringLocalizer[SharedResourcesKeys.DeleteSuccess] : message
            };
        }
        public ApiResponse<T> Success<T>(T entity, string message = null, object Meta = null)
        {
            return new ApiResponse<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = string.IsNullOrWhiteSpace(message) ? _stringLocalizer[SharedResourcesKeys.Success] : message,
                Meta = Meta
            };
        }
        //401 Unauthorized response indicates that the user is not authenticated.
        public ApiResponse<T> Unauthorized<T>(string message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? _stringLocalizer[SharedResourcesKeys.UnAuthorized] : message
            };
        }
        //403 Forbidden response indicates that the user is not allowed to access the resource.
        public ApiResponse<T> Forbidden<T>(string message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? _stringLocalizer[SharedResourcesKeys.Forbidden] : message
            };
        }
        public ApiResponse<T> BadRequest<T>(string message = null, List<string> errors = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Errors = errors,
                Message = string.IsNullOrWhiteSpace(message) ? _stringLocalizer[SharedResourcesKeys.BadRequest] : message
            };
        }

        public ApiResponse<T> NotFound<T>(string message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? _stringLocalizer[SharedResourcesKeys.NotFound] : message
            };
        }

        public ApiResponse<T> UnprocessableEntity<T>(string message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? _stringLocalizer[SharedResourcesKeys.UnprocessableEntity] : message
            };
        }


        public ApiResponse<T> Created<T>(T entity, string message = null, object Meta = null)
        {
            return new ApiResponse<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = string.IsNullOrWhiteSpace(message) ? _stringLocalizer[SharedResourcesKeys.CreateSuccess] : message,
                Meta = Meta
            };
        }
    }
}
