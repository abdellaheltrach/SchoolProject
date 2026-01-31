using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Autorazation.Commands.Models;
using School.Core.Features.Autorazation.Queries.Models;
using School.Domain.AppRoutes;
using School.Domain.Helpers;

namespace School.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = AppRolesConstants.Admin)]
    public class AutorazationController : AppBaseController
    {
        public AutorazationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost(AppRouter.AuthorizationRouting.Create)]
        public async Task<IActionResult> Create([FromForm] AddRoleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(AppRouter.AuthorizationRouting.Edit)]
        public async Task<IActionResult> Edit([FromForm] EditRoleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpDelete(AppRouter.AuthorizationRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var response = await _mediator.Send(new DeleteRoleCommand(Id));
            return NewResult(response);
        }

        [HttpGet(AppRouter.AuthorizationRouting.RoleList)]
        public async Task<IActionResult> GetRoleList()
        {
            var response = await _mediator.Send(new GetRolesListQuery());
            return NewResult(response);
        }
        [HttpGet(AppRouter.AuthorizationRouting.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetRoleByIdQuery() { Id = id });
            return NewResult(response);
        }

        [HttpGet(AppRouter.AuthorizationRouting.ManageUserRoles)]
        public async Task<IActionResult> ManageUserRoles([FromRoute] int userId)
        {
            var response = await _mediator.Send(new ManageUserRolesQuery() { UserId = userId });
            return NewResult(response);
        }

        [HttpPut(AppRouter.AuthorizationRouting.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
