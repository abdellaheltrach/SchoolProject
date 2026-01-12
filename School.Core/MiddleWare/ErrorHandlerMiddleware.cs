using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Resources;
using System.Net;
using System.Text.Json;



namespace School.Core.MiddleWare
{

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ErrorHandlerMiddleware(RequestDelegate next, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _next = next;
            _localizer = stringLocalizer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ApiResponse<string>() { Succeeded = false, Message = error?.Message };
                //   Log.Error(error, error.Message, context.Request, "");

                switch (error)
                {
                    case UnauthorizedAccessException:
                        responseModel.Message = _localizer[SharedResourceskeys.UnAuthorized];
                        responseModel.StatusCode = HttpStatusCode.Unauthorized;
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    case FluentValidation.ValidationException validationEx:
                        responseModel.Message = _localizer[SharedResourceskeys.ValidationFailed];
                        responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
                        response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        responseModel.Errors = validationEx.Errors
                            .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                            .ToList();
                        break;

                    case KeyNotFoundException:
                        responseModel.Message = _localizer[SharedResourceskeys.NotFound];
                        responseModel.StatusCode = HttpStatusCode.NotFound;
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case DbUpdateException:
                        responseModel.Message = _localizer[SharedResourceskeys.DatabaseUpdateError];
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case Exception e when e.GetType().Name == "ApiException":
                        responseModel.Message = _localizer[SharedResourceskeys.BadRequest];
                        if (e.InnerException != null)
                            responseModel.Message += $"\n{e.InnerException.Message}";
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case Exception e:
                        responseModel.Message = _localizer[SharedResourceskeys.InternalServerError];
                        if (e.InnerException != null)
                            responseModel.Message += $"\n{e.InnerException.Message}";
                        responseModel.StatusCode = HttpStatusCode.InternalServerError;
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;

                    default:
                        responseModel.Message = _localizer[SharedResourceskeys.InternalServerError];
                        responseModel.StatusCode = HttpStatusCode.InternalServerError;
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
