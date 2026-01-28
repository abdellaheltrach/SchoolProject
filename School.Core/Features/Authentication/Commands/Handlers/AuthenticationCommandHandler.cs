using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Features.Authentication.Commands.Response;
using School.Core.Helpers;
using School.Core.Resources;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ApiResponseHandler
        , IRequestHandler<SignInCommand, ApiResponse<SignInResponse>>
    {

        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly CookieSettings _cookieSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;


        #endregion

        #region Constructors
        public AuthenticationCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                            UserManager<User> userManager,
                                            SignInManager<User> signInManager,
                                            IAuthenticationService authenticationService,
                                            CookieSettings cookieSettings,
                                            IHttpContextAccessor httpContextAccessor) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _cookieSettings = cookieSettings;
            _httpContextAccessor = httpContextAccessor;

        }

        #endregion



        #region Hundlers
        public async Task<ApiResponse<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
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

            //Return not found return InvalidCredentials
            if (user == null) return BadRequest<SignInResponse>(_stringLocalizer[SharedResourcesKeys.InvalidCredentials]);
            //try To Sign in 
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            //if Failed Return InvalidCredentials
            if (!signInResult.Succeeded) return BadRequest<SignInResponse>(_stringLocalizer[SharedResourcesKeys.InvalidCredentials]);

            //user seccessfully signed in then generate JWT Token and Refresh Token
            var result = await _authenticationService.GenerateJwtTokenAsync(user);

            // Set refresh token as HTTP-only cookie
            var httpContext = _httpContextAccessor.HttpContext;
            httpContext.Response.Cookies.Append("refreshToken", result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = _cookieSettings.HttpOnly,
                    Secure = _cookieSettings.Secure,
                    SameSite = Enum.Parse<SameSiteMode>(_cookieSettings.SameSite),
                    Expires = DateTimeOffset.UtcNow.AddDays(_cookieSettings.RefreshTokenExpirationTimeInDays)
                });
            //return response with access token
            var response = new SignInResponse
            {
                AccessToken = result.AccessToken
            };
            //return Token 
            return Success(response);
        }


        #endregion


    }
}
