using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Users.Commands.Models;
using School.Core.Features.Users.Queries.Models;
using School.Core.Features.Users.Queries.Response;
using School.Core.Base.ApiResponse;
using School.Core.Base.Wrappers;
using School.Core.Filters;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ServiceFilter(typeof(ValidateAdminRoleFilter))]
    public class UserController : AppBaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {

        }

        [HttpPost(AppRouter.UserRouting.Create)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddUserCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(AppRouter.UserRouting.GetUserByID)]
        [ProducesResponseType(typeof(ApiResponse<GetUserByIdQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<GetUserByIdQueryResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserbyId([FromRoute] int Id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery(Id));
            return NewResult(response);
        }

        [HttpGet(AppRouter.UserRouting.Paginated)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<GetPaginatedUsersListResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserbyId([FromQuery] GetPaginatedUsersListQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpPut(AppRouter.UserRouting.Edit)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit([FromBody] EditUserCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(AppRouter.UserRouting.Delete)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            return NewResult(await _mediator.Send(new DeleteUserCommand(Id)));
        }

        [HttpPut(AppRouter.UserRouting.ChangePassword)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}