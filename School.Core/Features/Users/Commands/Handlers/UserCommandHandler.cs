using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;
using School.Domain.Entities.Identity;

namespace School.Core.Features.Users.Commands.Handlers
{
    public class UserCommandHandler : ApiResponseHandler, IRequestHandler<AddUserCommand, ApiResponse<string>>
                                                        , IRequestHandler<EditUserCommand, ApiResponse<string>>
                                                        , IRequestHandler<DeleteUserCommand, ApiResponse<string>>
                                                        , IRequestHandler<ChangeUserPasswordCommand, ApiResponse<string>>
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

        public async Task<ApiResponse<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {


            //check if user is exist

            var oldUser = await _userManager.FindByIdAsync(request.Id.ToString());



            //if Not Exist notfound
            if (oldUser == null) return NotFound<string>();
            //mapping
            var newUser = _mapper.Map(request, oldUser);

            //if username is Exist
            var getUserByUserName = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == newUser.UserName && x.Id != newUser.Id);
            //username is Exist
            if (getUserByUserName != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);

            //update
            var result = await _userManager.UpdateAsync(newUser);
            //result is not success
            if (!result.Succeeded) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateUserFailed]);
            //message
            return Success<string>(_stringLocalizer[SharedResourcesKeys.UserUpdated]);
        }

        public async Task<ApiResponse<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            //check if user is exist
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            //if Not Exist notfound
            if (user == null) return NotFound<string>(SharedResourcesKeys.NotFound);
            //Delete the User
            var result = await _userManager.DeleteAsync(user);
            //in case of Failure
            if (!result.Succeeded) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeletedFailed]);
            return Success((string)_stringLocalizer[SharedResourcesKeys.Deleted]);
        }

        public async Task<ApiResponse<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            //get user
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            //check if user is exist
            //if Not Exist notfound
            if (user == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            //Change User Password
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            //var user1=await _userManager.HasPasswordAsync(user);
            //await _userManager.RemovePasswordAsync(user);
            //await _userManager.AddPasswordAsync(user, request.NewPassword);

            //result
            if (!result.Succeeded) return BadRequest<string>(result.Errors.FirstOrDefault().Description);
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}
