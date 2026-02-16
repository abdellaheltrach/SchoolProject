using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Emails.Commands.Models;
using School.Core.Base.ApiResponse;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{
    [Authorize]
    public class EmailsController : AppBaseController
    {
        public EmailsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost(AppRouter.EmailsRoute.SendEmail)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
