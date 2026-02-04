using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Departement.Queries.Models;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{

    [ApiController]
    [Authorize]
    public class DepartementController : AppBaseController
    {
        public DepartementController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet(AppRouter.DepartmentRouting.GetDepartmentByID)]
        public async Task<IActionResult> GetDepartmentByID([FromQuery] GetDepartementByIdQuery Query)
        {
            return NewResult(await _mediator.Send(Query));
        }

        [HttpGet(AppRouter.DepartmentRouting.GetDepartmentStudentsCount)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDepartmentStudentsCount()
        {
            return NewResult(await _mediator.Send(new GetDepartmentStudentListCountQuery()));
        }
    }
}
