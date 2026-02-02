using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Authentication.Queries.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Authentication.Queries.Handlers
{
    public class AuthenticationQueryHandler : ApiResponseHandler,
        IRequestHandler<EmailConfirmationQuery, ApiResponse<string>>

    {


        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region Constructors
        public AuthenticationQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                            IAuthenticationService authenticationService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _authenticationService = authenticationService;
        }


        #endregion

        #region Handle Functions



        public async Task<ApiResponse<string>> Handle(EmailConfirmationQuery request, CancellationToken cancellationToken)
        {
            var isConfirmed = await _authenticationService.ConfirmEmail(request.UserId, request.code);
            if (!isConfirmed)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.ErrorWhenConfirmEmail]);
            return Success<string>(_stringLocalizer[SharedResourcesKeys.ConfirmEmailDone]);
        }


        #endregion
    }
}
