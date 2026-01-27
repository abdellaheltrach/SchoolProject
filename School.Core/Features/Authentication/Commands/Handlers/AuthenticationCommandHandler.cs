using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Helpers;
using School.Core.Resources;
using School.Domain.Entities.Identity;
using School.Domain.Responses;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ApiResponseHandler
        , IRequestHandler<SignInCommand, ApiResponse<JwtAuthResponse>>
    {

        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;


        #endregion

        #region Constructors
        public AuthenticationCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                            UserManager<User> userManager,
                                            SignInManager<User> signInManager,
                                            IAuthenticationService authenticationService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
        }

        #endregion



        #region Hundlers
        public async Task<ApiResponse<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {

            User? user;

            //Check if user is exist or not
            if (SignInHelper.IsEmail(request.Identifier))
            {
                user = await _userManager.FindByEmailAsync(request.Identifier);
            }
            else
            {
                user = await _userManager.FindByNameAsync(request.Identifier);
            }

            //Return The UserName Not Found
            if (user == null) return BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.InvalidCredentials]);
            //try To Sign in 
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            //if Failed Return Passord is wrong
            if (!signInResult.Succeeded) return BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.InvalidCredentials]);

            //Generate Token
            var result = await _authenticationService.GenerateJwtTokenAsync(user);
            //return Token 
            return Success(result);
        }


        #endregion


    }
}
