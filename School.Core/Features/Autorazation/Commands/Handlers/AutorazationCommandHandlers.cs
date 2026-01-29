using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Autorazation.Commands.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Autorazation.Commands.Handlers
{
    public class AutorazationCommandHandlers : ApiResponseHandler,
        IRequestHandler<AddRoleCommand, ApiResponse<string>>
    {
        private readonly IAuthorizationService _authorizationService;
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public AutorazationCommandHandlers(IAuthorizationService authorizationService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Methods
        public async Task<ApiResponse<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.AddRoleAsync(request.RoleName);
            if (result) return Success("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CreateFailed]);
        }
        #endregion


    }
}
