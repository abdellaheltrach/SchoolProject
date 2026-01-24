using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Users.Commands.Models;
using School.Core.Features.Users.Queries.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{
    [ApiController]
    public class UserController : AppBaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {

        }

        [HttpPost(AppRouter.UserRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddUserCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(AppRouter.UserRouting.GetUserByID)]
        public async Task<IActionResult> GetUserbyId([FromRoute] int Id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery(Id));
            return NewResult(response);
        }

        [HttpGet(AppRouter.UserRouting.Paginated)]
        public async Task<IActionResult> GetUserbyId([FromQuery] GetPaginatedUsersListQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpPut(AppRouter.UserRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditUserCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(AppRouter.UserRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            return NewResult(await _mediator.Send(new DeleteUserCommand(Id)));
        }
    }
}