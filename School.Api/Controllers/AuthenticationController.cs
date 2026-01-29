using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Authentication.Commands.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{
    [ApiController]
    public class AuthenticationController : AppBaseController
    {
        public AuthenticationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost(AppRouter.AuthonticationRouting.SignIn)]
        public async Task<IActionResult> Create([FromForm] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(AppRouter.AuthonticationRouting.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

    }
}
