using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Resources;
using Serilog;
using System.Net;
using System.Text.Json;




namespace School.Core.MiddleWare
{

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ILogger _logger;

        // need to add a FrameworkReference so it's can be used
        // <FrameworkReference Include="Microsoft.AspNetCore.App" /> + using Microsoft.AspNetCore.Hosting;
        private readonly IWebHostEnvironment _env;




        public ErrorHandlerMiddleware(
            RequestDelegate next,
            IStringLocalizer<SharedResources> stringLocalizer,
            IWebHostEnvironment env)
        {
            _next = next;
            _localizer = stringLocalizer;
            _logger = Log.ForContext<ErrorHandlerMiddleware>();
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context, error);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var responseModel = new ApiResponse<string>()
            {
                Succeeded = false,
                Errors = new List<string>()
            };

            switch (error)
            {
                case UnauthorizedAccessException unauthorizedEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    responseModel.StatusCode = HttpStatusCode.Unauthorized;
                    responseModel.Message = _localizer[SharedResourcesKeys.UnAuthorized];
                    _logger.Warning(unauthorizedEx, "Unauthorized access attempt");
                    break;

                case FluentValidation.ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    responseModel.Message = _localizer[SharedResourcesKeys.ValidationFailed];
                    responseModel.Errors = validationEx.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    _logger.Warning("Validation failed: {Errors}", string.Join(", ", responseModel.Errors));
                    break;

                case ArgumentNullException argNullEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    responseModel.Message = _localizer[SharedResourcesKeys.BadRequest];
                    responseModel.Errors.Add(argNullEx.Message);
                    _logger.Warning(argNullEx, "Argument null error: {ParamName}", argNullEx.ParamName);
                    break;

                case ArgumentException argEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    responseModel.Message = _localizer[SharedResourcesKeys.BadRequest];
                    responseModel.Errors.Add(argEx.Message);
                    _logger.Warning(argEx, "Argument validation error: {Message}", argEx.Message);
                    break;


                case KeyNotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseModel.StatusCode = HttpStatusCode.NotFound;
                    responseModel.Message = _localizer[SharedResourcesKeys.NotFound];
                    _logger.Warning(notFoundEx, "Resource not found");
                    break;

                case DbUpdateException dbEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    responseModel.Message = _localizer[SharedResourcesKeys.DatabaseUpdateError];

                    // Only show detailed error in development
                    if (_env.IsDevelopment() && dbEx.InnerException != null)
                    {
                        responseModel.Errors.Add(dbEx.InnerException.Message);
                    }

                    _logger.Error(dbEx, "Database update error");
                    break;

                case Exception e when e.GetType().Name == "ApiException":
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    responseModel.Message = _localizer[SharedResourcesKeys.BadRequest];

                    // Only show inner exception in development
                    if (_env.IsDevelopment() && e.InnerException != null)
                    {
                        responseModel.Errors.Add(e.InnerException.Message);
                    }

                    _logger.Error(e, "API exception occurred");
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel.StatusCode = HttpStatusCode.InternalServerError;
                    responseModel.Message = _localizer[SharedResourcesKeys.InternalServerError];

                    // Only show detailed error in development
                    if (_env.IsDevelopment())
                    {
                        responseModel.Errors.Add(error.Message);
                        if (error.InnerException != null)
                        {
                            responseModel.Errors.Add($"Inner: {error.InnerException.Message}");
                        }
                    }

                    _logger.Error(error, "Unhandled exception occurred");
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await response.WriteAsync(result);
        }
    }
}
