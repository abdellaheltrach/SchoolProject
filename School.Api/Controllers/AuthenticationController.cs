using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Base.ApiResponse;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Features.Authentication.Commands.Response;
using School.Core.Features.Authentication.Queries.Models;
using School.Domain.AppRoutes;
using School.Domain.Helpers;
using Serilog;
using System.Net;
using System.Security.Claims;

namespace School.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AuthenticationController : AppBaseController
    {
        public AuthenticationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost(AppRouter.AuthenticationRouting.SignIn)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(AppRouter.AuthenticationRouting.Logout)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            var response = await _mediator.Send(new SignOutCommand());
            return NewResult(response);
        }

        [HttpPost(AppRouter.AuthenticationRouting.RefreshToken)]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(AppRouter.AuthenticationRouting.ConfirmEmail)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] EmailConfirmationQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(AppRouter.AuthenticationRouting.SendResetPasswordCode)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendResetPassword([FromQuery] SendResetPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpGet(AppRouter.AuthenticationRouting.ConfirmResetPasswordCode)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmResetPassword([FromQuery] ConfirmResetPasswordQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
        [HttpPost(AppRouter.AuthenticationRouting.ResetPassword)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet("verify")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public IActionResult VerifyToken()
        {
            try
            {

                // Extract user information from token claims
                var userId = User.FindFirst(nameof(UserClaimModel.Id))?.Value;
                var username = User.FindFirst(nameof(UserClaimModel.UserName))?.Value;
                var email = User.FindFirst(nameof(UserClaimModel.Email))?.Value;
                var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

                Log.Information("Token verified for user: {UserId} ({Username})", userId, username);

                // Return user information
                var userData = new
                {
                    userId,
                    username,
                    email,
                    roles
                };

                return NewResult(new ApiResponse<object>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = userData,
                    Succeeded = true,
                    Message = "Token is valid"
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error verifying token");
                return NewResult(new ApiResponse<object>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Succeeded = false,
                    Message = "Token verification failed"
                });
            }
        }
    }
}

