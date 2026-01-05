using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Core.Features.Students.Queries.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{

    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            this._mediator = mediator;
        }



        [HttpGet(AppRouter.StudentRouting.GetStudentList)]
        public async Task<IActionResult> GetStudentList()
        {
            var Reponse = await _mediator.Send(new GetStudentListQuery());
            return Ok(Reponse);
        }

        [HttpGet(AppRouter.StudentRouting.GetStudentByID)]
        public async Task<IActionResult> GetStudentById(int Id)
        {
            var Reponse = await _mediator.Send(new GetStudentByIdQuery(Id));
            return Ok(Reponse);
        }

    }
}
