using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Autorazation.Commands.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{
    [ApiController]
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
    }
}
