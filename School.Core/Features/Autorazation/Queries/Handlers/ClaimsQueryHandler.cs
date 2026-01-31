using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Autorazation.Queries.Models;
using School.Core.Resources;
using School.Domain.Entities.Identity;
using School.Domain.Results;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Autorazation.Queries.Handlers
{
    public class ClaimsQueryHandler : ApiResponseHandler,
        IRequestHandler<ManageUserClaimsQuery, ApiResponse<ManageUserClaimsResult>>
    {

        #region Fileds
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion
        #region Constructors
        public ClaimsQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IAuthorizationService authorizationService,
                                  UserManager<User> userManager) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }
        #endregion
        #region Methods
        public async Task<ApiResponse<ManageUserClaimsResult>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<ManageUserClaimsResult>(_stringLocalizer[SharedResourcesKeys.UserNotFound]);
            var result = await _authorizationService.ManageUserClaimData(user);
            return Success(result);
        }
        #endregion
    }
}
