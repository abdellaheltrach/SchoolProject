using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Authentication.Commands.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{
    [ApiController]
    public class AuthController : AppBaseController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost(AppRouter.Auth.SignIn)]
        public async Task<IActionResult> Create([FromForm] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(AppRouter.Auth.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

    }
}
