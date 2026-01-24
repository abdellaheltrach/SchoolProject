using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Users.Queries.Models;
using School.Core.Features.Users.Queries.Response;
using School.Core.Resources;
using School.Domain.Entities.Identity;

namespace School.Core.Features.Users.Queries.Handlers
{
    public class GetUserByIdQueryHandler : ApiResponseHandler, IRequestHandler<GetUserByIdQuery, ApiResponse<GetUserByIdQueryResponse>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _Localizer;
        private readonly UserManager<User> _user;
        private readonly IMapper _mapper;
        #endregion



        #region Constructor
        public GetUserByIdQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                       UserManager<User> User,
                                       IMapper mapper) : base(stringLocalizer)
        {
            _Localizer = stringLocalizer;
            _user = User;
            _mapper = mapper;
        }
        #endregion

        #region Handlers

        public async Task<ApiResponse<GetUserByIdQueryResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _user.FindByIdAsync(request.Id.ToString());
            if (user == null) return NotFound<GetUserByIdQueryResponse>(_Localizer[SharedResourcesKeys.NotFound]);
            var result = _mapper.Map<GetUserByIdQueryResponse>(user);
            return Success(result);
        }
        #endregion

    }
}
