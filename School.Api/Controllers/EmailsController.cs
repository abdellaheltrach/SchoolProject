using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Emails.Commands.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{
    public class EmailsController : AppBaseController
    {
        public EmailsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost(AppRouter.EmailsRoute.SendEmail)]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
