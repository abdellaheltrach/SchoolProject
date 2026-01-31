using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Autorazation.Queries.Models;
using School.Core.Features.Autorazation.Queries.QueriesResponse;
using School.Core.Resources;
using School.Domain.Entities.Identity;
using School.Domain.Results;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Autorazation.Queries.Handlers
{
    public class RolesQueryHandler : ApiResponseHandler,
       IRequestHandler<GetRolesListQuery, ApiResponse<List<GetRolesListResponse>>>,
       IRequestHandler<GetRoleByIdQuery, ApiResponse<GetRoleByIdResponse>>,
        IRequestHandler<ManageUserRolesQuery, ApiResponse<ManageUserRolesResult>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        public RolesQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                IAuthorizationService authorizationService,
                                IMapper mapper,
                                UserManager<User> userManager) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
        }

        public async Task<ApiResponse<List<GetRolesListResponse>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRolesList();
            var result = _mapper.Map<List<GetRolesListResponse>>(roles);
            return Success(result);
        }

        public async Task<ApiResponse<GetRoleByIdResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleById(request.Id);
            if (role == null) return NotFound<GetRoleByIdResponse>(_stringLocalizer[SharedResourcesKeys.RoleNotExist]);
            var result = _mapper.Map<GetRoleByIdResponse>(role);
            return Success(result);
        }

        public async Task<ApiResponse<ManageUserRolesResult>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<ManageUserRolesResult>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            var result = await _authorizationService.ManageUserRolesData(user);
            return Success(result);
        }
    }
}
