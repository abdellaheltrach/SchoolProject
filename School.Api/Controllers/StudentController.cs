using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Students.Commands.Models;
using School.Core.Features.Students.Queries.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{

    [ApiController]
    public class StudentController : AppBaseController
    {
        public StudentController(IMediator mediator) : base(mediator)
        {

        }



        [HttpGet(AppRouter.StudentRouting.GetStudentList)]
        public async Task<IActionResult> GetStudentList()
        {
            var Reponse = await _mediator.Send(new GetStudentListQuery());
            return NewResult(Reponse);
        }

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


        [HttpPost(AppRouter.StudentRouting.AddStudent)]
        public async Task<IActionResult> Create([FromBody] AddStudentCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


        [HttpPut(AppRouter.StudentRouting.EditStudent)]
        public async Task<IActionResult> Edit([FromBody] EditStudentCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(AppRouter.StudentRouting.DeleteStudent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await _mediator.Send(new DeleteStudentCommand(id)));
        }


    }
}
