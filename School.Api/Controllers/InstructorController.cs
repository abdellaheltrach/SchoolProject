using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Instructors.Commands.Models;
using School.Core.Features.Instructors.Queries.Models;
using School.Core.Base.ApiResponse;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{
    [ApiController]
    public class InstructorController : AppBaseController
    {
        public InstructorController(IMediator mediator) : base(mediator)
        {
        }



        [HttpGet(AppRouter.InstructorRouting.GetSalarySummationOfInstructor)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSalarySummation()
        {
            return NewResult(await _mediator.Send(new GetSummationSalaryOfInstructorQuery()));
        }

        [HttpPost(AppRouter.InstructorRouting.AddInstructor)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddInstructor([FromForm] AddInstructorCommand command)
        {
            return NewResult(await _mediator.Send(command));
        }
    }
}
