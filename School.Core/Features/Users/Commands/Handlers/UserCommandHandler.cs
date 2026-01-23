using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;
using School.Domain.Entities.Identity;

namespace School.Core.Features.Users.Commands.Handlers
{
    public class UserCommandHandler : ApiResponseHandler, IRequestHandler<AddUserCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IMapper _mapper;
        public readonly UserManager<User> _userManager;
        #endregion
        #region Constructors
        public UserCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                        IMapper mapper,
                        UserManager<User> userManager) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion

        #region Handlers
        public async Task<ApiResponse<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var NewidentityUser = _mapper.Map<User>(request);
            var createResult = await _userManager.CreateAsync(NewidentityUser, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors
                    .Select(error => error.Description)
                    .ToList();
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserCreationFailed], errors);

            }
            return Created<string>(null, null, new { id = NewidentityUser.Id });

        }
        #endregion
    }
}
