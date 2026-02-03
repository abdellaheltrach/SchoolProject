using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using School.Domain.Helpers;
using School.Service.AuthServices.Interfaces;

namespace School.Core.Filters
{
    public class ValidateAdminRoleFilter : IAsyncActionFilter
    {
        private readonly ICurrentUserService _currentUserService;
        //private readonly ILogger<ValidateAdminRoleFilter> _logger;

        public ValidateAdminRoleFilter(
            ICurrentUserService currentUserService
            //, ILogger<ValidateAdminRoleFilter> logger
            )
        {
            _currentUserService = currentUserService;
            //_logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if user is authenticated
            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                //_logger.LogWarning("Unauthenticated user attempted to access protected resource");
                context.Result = new UnauthorizedObjectResult(new
                {
                    statusCode = 401,
                    message = "Unauthorized. Please log in.",
                    succeeded = false
                });
                return;
            }

            try
            {
                // Get user's current roles from database
                var roles = await _currentUserService.GetCurrentUserRolesAsync();

                // Check if user has "User" role
                if (!roles.Contains(AppRolesConstants.Admin))
                {
                    var userId = context.HttpContext.User.FindFirst(nameof(UserClaimModel.Id))?.Value;
                    //_logger.LogWarning($"User {userId} does not have User role. Roles: {string.Join(", ", roles)}");

                    context.Result = new ObjectResult(new
                    {
                        statusCode = 403,
                        message = "Forbidden. You don't have the required User role.",
                        succeeded = false
                    })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }

                // User has valid User role - continue to action
                await next();
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Error validating user role: {ex.Message}");
                context.Result = new ObjectResult(new
                {
                    statusCode = 500,
                    message = "An error occurred while validating permissions.",
                    succeeded = false
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
