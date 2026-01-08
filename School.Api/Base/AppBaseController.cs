using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Core.ApiResponse;
using System.Net;

namespace School.Api.Base
{

    [ApiController]
    public class AppBaseController : ControllerBase
    {
        public readonly IMediator _mediator;

        public AppBaseController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        #region Actions
        public ObjectResult NewResult<T>(ApiResponse<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(response);
                case HttpStatusCode.Created:
                    return new CreatedResult(string.Empty, response);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case HttpStatusCode.Accepted:
                    return new AcceptedResult(string.Empty, response);
                case HttpStatusCode.UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                default:
                    return new BadRequestObjectResult(response);
            }
        }
        #endregion
    }
}
