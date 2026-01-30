using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Autorazation.Commands.Models;
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
    }
}
