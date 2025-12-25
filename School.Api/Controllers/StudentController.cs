using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Core.Features.Students.Commands.Models;

namespace School.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            this._mediator = mediator;
        }



        [HttpGet("GetStudentList")]
        public async Task<IActionResult> GetStudentList()
        {
            var Reponse = await _mediator.Send(new GetStudentListQuery());
            return Ok(Reponse);
        }
    }
}
