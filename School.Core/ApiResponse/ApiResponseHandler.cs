

namespace School.Core.ApiResponse
{
    public class ApiResponseHandler
    {
        public ApiResponseHandler()
        {

        }
        public ApiResponse<T> Deleted<T>()
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Deleted Successfully"
            };
        }
        public ApiResponse<T> Success<T>(T entity, string message = "Action executed successfully", object Meta = null)
        {
            return new ApiResponse<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message,
                Meta = Meta
            };
        }
        public ApiResponse<T> Unauthorized<T>()
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = "UnAuthorized"
            };
        }
        public ApiResponse<T> BadRequest<T>(string Message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? "Bad Request" : Message
            };
        }

        public ApiResponse<T> NotFound<T>(string message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? "Not Found" : message
            };
        }

        public ApiResponse<T> UnprocessableEntity<T>(string message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message == null ? "Unprocessable Entity" : message
            };
        }


        public ApiResponse<T> Created<T>(T entity, object Meta = null)
        {
            return new ApiResponse<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created",
                Meta = Meta
            };
        }
    }
}
