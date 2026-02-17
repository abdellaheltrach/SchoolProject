using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using School.Api.Base;
using School.Core.Base.ApiResponse;
using School.Core.Features.Autorazation.Commands.Models;
using School.Core.Features.Autorazation.Queries.Models;
using School.Core.Features.Autorazation.Queries.QueriesResponse;
using School.Domain.AppRoutes;
using School.Domain.Helpers;
using School.Domain.Results;

namespace School.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = AppRolesConstants.Admin)]
    [EnableRateLimiting("authenticatedLimiter")]  //  60 per minute per user
    public class AutorazationController : AppBaseController
    {
        public AutorazationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost(AppRouter.AuthorizationRouting.Create)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] AddRoleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(AppRouter.AuthorizationRouting.Edit)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit([FromForm] EditRoleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpDelete(AppRouter.AuthorizationRouting.Delete)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var response = await _mediator.Send(new DeleteRoleCommand(Id));
            return NewResult(response);
        }

        [HttpGet(AppRouter.AuthorizationRouting.RoleList)]
        [ProducesResponseType(typeof(ApiResponse<List<GetRolesListResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoleList()
        {
            var response = await _mediator.Send(new GetRolesListQuery());
            return NewResult(response);
        }
        [HttpGet(AppRouter.AuthorizationRouting.GetRoleById)]
        [ProducesResponseType(typeof(ApiResponse<GetRoleByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<GetRoleByIdResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetRoleByIdQuery() { Id = id });
            return NewResult(response);
        }

        [HttpGet(AppRouter.AuthorizationRouting.ManageUserRoles)]
        [ProducesResponseType(typeof(ApiResponse<ManageUserRolesResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ManageUserRolesResult>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ManageUserRoles([FromRoute] int userId)
        {
            var response = await _mediator.Send(new ManageUserRolesQuery() { UserId = userId });
            return NewResult(response);
        }

        [HttpPut(AppRouter.AuthorizationRouting.UpdateUserRoles)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


        [HttpGet(AppRouter.AuthorizationRouting.ManageUserClaims)]
        [ProducesResponseType(typeof(ApiResponse<ManageUserClaimsResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ManageUserClaimsResult>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ManageUserClaims([FromRoute] int userId)
        {
            var response = await _mediator.Send(new ManageUserClaimsQuery() { UserId = userId });
            return NewResult(response);
        }

        [HttpPut(AppRouter.AuthorizationRouting.UpdateUserClaims)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserClaims([FromBody] UpdateUserClaimsCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

    }
}
