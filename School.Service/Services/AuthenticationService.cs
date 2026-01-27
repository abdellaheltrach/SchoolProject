using Microsoft.IdentityModel.Tokens;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Domain.Responses;
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
        private JwtSettings _jwtSettings { get; set; }
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
        #endregion


        #region  Constructor
        public AuthenticationService(JwtSettings jwtSettings
                                     , IUserRefreshTokenRepository userRefreshTokenRepository)
        {
            _jwtSettings = jwtSettings;
            _userRefreshTokenRepository = userRefreshTokenRepository;
        }
        #endregion



        #region Methods
        public async Task<JwtAuthResponse> GetJWTToken(User user)
        {
            var (jwtToken, accessTokenString) = GenerateAccessToken(user);
            var refreshToken = GetRefreshToken(user.UserName);
            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationTimeInDays),
                IsUsed = true,
                IsRevoked = false,
                JwtId = jwtToken.Id,
                RefreshToken = refreshToken.RefToken,
                AccessToken = accessTokenString,
                UserId = user.Id
            };
            var userRefreshtoken = await _userRefreshTokenRepository.AddAsync(userRefreshToken);


            var response = new JwtAuthResponse();
            response.refreshToken = refreshToken;
            response.AccessToken = accessTokenString;
            return response;
        }

        #endregion

        #region Helpers
        private (JwtSecurityToken, string) GenerateAccessToken(User user)
        {
            // Create claims
            var claims = new List<Claim>()
{
    new Claim(nameof(UserClaimModel.UserName), user.UserName),
    new Claim(nameof(UserClaimModel.Email), user.Email),
    new Claim(nameof(UserClaimModel.Id), user.Id.ToString()),
};

            // Create signing credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create JWT token
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationTimeInMinutes),
                signingCredentials: credentials);

            // Convert to string
            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessTokenString);

        }

        private RefreshToken GetRefreshToken(string username)
        {
            var refreshToken = new RefreshToken
            {
                ExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationTimeInDays),
                UserName = username,
                RefToken = GenerateRefreshTokenString()
            };
            return refreshToken;
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
    }
}
