using Microsoft.IdentityModel.Tokens;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Domain.Results;
using School.Infrastructure.Repositories.Interfaces;
using School.Service.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JwtSettings _jwtSettings;
        private readonly CookieSettings _cookieSettings;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
        #endregion


        #region  Constructor
        public AuthenticationService(JwtSettings jwtSettings,
                                     CookieSettings cookieSettings,
                                     IUserRefreshTokenRepository userRefreshTokenRepository)
        {
            _jwtSettings = jwtSettings;
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _cookieSettings = cookieSettings;
        }
        #endregion



        #region Methods
        public async Task<JwtAuthResult> GenerateJwtTokenAsync(User user)
        {
            var (jwtToken, accessTokenString) = GenerateAccessToken(user);
            var refreshTokenString = GenerateRefreshTokenString();

            var userRefreshToken = await _userRefreshTokenRepository.GetByIdAsync(user.Id);
            if (userRefreshToken == null)
            {
                userRefreshToken = new UserRefreshToken
                {
                    UserId = user.Id,
                    AccessToken = accessTokenString,
                    RefreshToken = refreshTokenString,  // Use the string directly
                    JwtId = jwtToken.Id,
                    ExpiryDate = DateTime.UtcNow.AddDays(_cookieSettings.RefreshTokenExpirationTimeInDays),
                    AddedTime = DateTime.UtcNow,
                    IsUsed = false,
                    IsRevoked = false
                };
                await _userRefreshTokenRepository.AddAsync(userRefreshToken);
            }
            else
            {
                userRefreshToken.AccessToken = accessTokenString;
                userRefreshToken.RefreshToken = refreshTokenString;
                userRefreshToken.JwtId = jwtToken.Id;
                userRefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(_cookieSettings.RefreshTokenExpirationTimeInDays);
                userRefreshToken.AddedTime = DateTime.UtcNow;
                userRefreshToken.IsUsed = false;
                userRefreshToken.IsRevoked = false;
                await _userRefreshTokenRepository.UpdateAsync(userRefreshToken);
            }


            return new JwtAuthResult
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshTokenString
            };
        }

        #region Helpers
        private (JwtSecurityToken, string) GenerateAccessToken(User user)
        {
            var claims = new List<Claim>()
    {
        new Claim(nameof(UserClaimModel.UserName), user.UserName),
        new Claim(nameof(UserClaimModel.Email), user.Email),
        new Claim(nameof(UserClaimModel.Id), user.Id.ToString()),
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationTimeInMinutes),
                signingCredentials: credentials);

            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessTokenString);
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        #endregion
        #endregion
    }
}
