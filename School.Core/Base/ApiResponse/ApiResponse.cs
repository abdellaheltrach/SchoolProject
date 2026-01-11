using System.Net;

namespace School.Core.Base.ApiResponse
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Meta { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        //public Dictionary<string, List<string>> ErrorsBag { get; set; }
        public T Data { get; set; }

        public ApiResponse()
        {

        }
        public ApiResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public ApiResponse(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }
    }
}

