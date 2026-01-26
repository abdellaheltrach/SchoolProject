using Microsoft.IdentityModel.Tokens;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Service.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        public JwtSettings _jwtSettings { get; set; }
        #endregion


        #region  Constructor

        public AuthenticationService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }
        #endregion



        #region Methods
        public async Task<string> GetJWTToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(nameof(UserClaimModel.UserName), user.UserName),
                new Claim(nameof(UserClaimModel.Email), user.Email),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString()),

            };


            var jwtToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey)), SecurityAlgorithms.HmacSha256Signature));
            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessTokenString;
        }

        #endregion
    }
}
