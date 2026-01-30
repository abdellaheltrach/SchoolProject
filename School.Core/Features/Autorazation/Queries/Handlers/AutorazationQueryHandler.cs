using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Autorazation.Queries.Models;
using School.Core.Features.Autorazation.Queries.QueriesResponse;
using School.Core.Resources;
using School.Domain.Entities.Identity;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Autorazation.Queries.Handlers
{
    public class AutorazationQueryHandler : ApiResponseHandler,
       IRequestHandler<GetRolesListQuery, ApiResponse<List<GetRolesListResponse>>>,
       IRequestHandler<GetRoleByIdQuery, ApiResponse<GetRoleByIdResponse>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        public AutorazationQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
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
    }
}
