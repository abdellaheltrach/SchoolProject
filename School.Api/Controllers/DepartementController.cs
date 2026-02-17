using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using School.Api.Base;
using School.Core.Base.ApiResponse;
using School.Core.Features.Departement.Queries.Models;
using School.Core.Features.Departement.Queries.QueriesResponse;
using School.Domain.AppRoutes;

namespace School.Api.Controllers
{

    [ApiController]
    [Authorize]
    [EnableRateLimiting("authenticatedLimiter")]  //  60 per minute per user
    public class DepartementController : AppBaseController
    {
        public DepartementController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet(AppRouter.DepartmentRouting.GetDepartmentByID)]
        [ProducesResponseType(typeof(ApiResponse<GetDepartmentByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<GetDepartmentByIdResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDepartmentByID([FromQuery] GetDepartementByIdQuery Query)
        {
            return NewResult(await _mediator.Send(Query));
        }

        [HttpGet(AppRouter.DepartmentRouting.GetDepartmentStudentsCount)]
        [ProducesResponseType(typeof(ApiResponse<List<GetDepartmentStudentListCountResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDepartmentStudentsCount()
        {
            return NewResult(await _mediator.Send(new GetDepartmentStudentListCountQuery()));
        }
        [HttpGet(AppRouter.DepartmentRouting.GetDepartmentStudentsCountById)]
        [ProducesResponseType(typeof(ApiResponse<GetDepartmentStudentCountByIDResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDepartmentStudentsCountById([FromRoute] int Id)
        {
            return NewResult(await _mediator.Send(new GetDepartmentStudentCountByIDQuery() { DID = Id }));
        }
    }
}
