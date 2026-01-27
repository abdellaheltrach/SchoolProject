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
        public async Task<JwtAuthResponse> GenerateJwtTokenAsync(User user)
        {
            var (jwtToken, accessTokenString) = GenerateAccessToken(user);
            var refreshToken = GetRefreshToken(user.UserName);

            var userRefreshToken = await _userRefreshTokenRepository.GetByIdAsync(user.Id);

            if (userRefreshToken == null)
            {
                //
                userRefreshToken = new UserRefreshToken
                {
                    UserId = user.Id,
                    AccessToken = accessTokenString,
                    RefreshToken = refreshToken.RefToken,
                    JwtId = jwtToken.Id,
                    ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationTimeInDays),
                    AddedTime = DateTime.UtcNow,
                    IsUsed = false,
                    IsRevoked = false
                };

                await _userRefreshTokenRepository.AddAsync(userRefreshToken);

            }
            else
            {
                // Update existing record
                userRefreshToken.AccessToken = accessTokenString;
                userRefreshToken.RefreshToken = refreshToken.RefToken;
                userRefreshToken.JwtId = jwtToken.Id;
                userRefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationTimeInDays);
                userRefreshToken.AddedTime = DateTime.UtcNow;
                userRefreshToken.IsUsed = false;
                userRefreshToken.IsRevoked = false;
                await _userRefreshTokenRepository.UpdateAsync(userRefreshToken);

            }

            var response = new JwtAuthResponse
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshToken
            };

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
