using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Students.Commands.Models;
using School.Core.Features.Students.Queries.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{

    [ApiController]
    [Authorize]
    public class StudentController : AppBaseController
    {
        public StudentController(IMediator mediator) : base(mediator)
        {

        }


        [AllowAnonymous]
        [HttpGet(AppRouter.StudentRouting.GetStudentList)]
        public async Task<IActionResult> GetStudentList()
        {
            var Reponse = await _mediator.Send(new GetStudentListQuery());
            return NewResult(Reponse);
        }
        [AllowAnonymous]
        [HttpGet(AppRouter.StudentRouting.Paginated)]
        public async Task<IActionResult> Paginated([FromQuery] GetStudentPaginatedListQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(AppRouter.StudentRouting.GetStudentByID)]
        public async Task<IActionResult> GetStudentById(int Id)
        {
            var Reponse = await _mediator.Send(new GetStudentByIdQuery(Id));
            return NewResult(Reponse);
        }

        [Authorize(Policy = "CreateStudent")]
        [HttpPost(AppRouter.StudentRouting.AddStudent)]
        public async Task<IActionResult> Create([FromBody] AddStudentCommand command)
        {
            Console.WriteLine(command);
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = "EditStudent")]
        [HttpPut(AppRouter.StudentRouting.EditStudent)]
        public async Task<IActionResult> Edit([FromBody] EditStudentCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [Authorize(Policy = "DeleteStudent")]
        [HttpDelete(AppRouter.StudentRouting.DeleteStudent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await _mediator.Send(new DeleteStudentCommand(id)));
        }


    }
}
