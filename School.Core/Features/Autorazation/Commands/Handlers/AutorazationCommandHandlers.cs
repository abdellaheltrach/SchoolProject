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
        , IRequestHandler<EditRoleCommand, ApiResponse<string>>
        , IRequestHandler<DeleteRoleCommand, ApiResponse<string>>
        , IRequestHandler<UpdateUserRolesCommand, ApiResponse<string>>

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

        public async Task<ApiResponse<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.EditRoleAsync(request.Id, request.NewRoleName);
            if (!result) return NotFound<string>();
            else if (result) return Success((string)_stringLocalizer[SharedResourcesKeys.Success]);
            else
                return BadRequest<string>((string)_stringLocalizer[SharedResourcesKeys.BadRequest]);
        }

        public async Task<ApiResponse<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.DeleteRoleAsync(request.Id);
            if (result == "NotFound") return NotFound<string>();
            else if (result == "Used") return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RoleIsUsed]);
            else if (result == "Success") return Success((string)_stringLocalizer[SharedResourcesKeys.Deleted]);
            else
                return BadRequest<string>(result);
        }

        public async Task<ApiResponse<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.UpdateUserRoles(request.UserId, request.userRoles);
            switch (result)
            {
                case "UserIsNull": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.UserNotFound]);
                case "FailedToRemoveOldRoles": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToRemoveOldRoles]);
                case "FailedToAddNewRoles": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewRoles]);
                case "FailedToUpdateUserRoles": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUpdateUserRoles]);
            }
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion


    }
}
