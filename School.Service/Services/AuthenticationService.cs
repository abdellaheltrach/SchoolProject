using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School.Domain.Entities.Identity;
using School.Domain.Helpers;
using School.Domain.Options;
using School.Domain.Results;
using School.Infrastructure.Bases.UnitOfWork;
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
        private readonly UserManager<User> _userManager;
        private readonly IEmailsService _emailsService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion


        #region  Constructor
        public AuthenticationService(JwtSettings jwtSettings,
                                     CookieSettings cookieSettings,
                                     UserManager<User> userManager,
                                     IUnitOfWork unitOfWork,
                                     IEmailsService emailsService,
                                     IUserRefreshTokenRepository userRefreshTokenRepository)
        {
            _jwtSettings = jwtSettings;
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _userManager = userManager;
            _cookieSettings = cookieSettings;
            _emailsService = emailsService;
            _unitOfWork = unitOfWork;
        }
        #endregion



        #region Methods
        public async Task<JwtAuthResult> GenerateJwtTokenAsync(User user)
        {
            var (jwtToken, accessTokenString) = GenerateAccessToken(user);
            var refreshTokenString = GenerateRefreshTokenString();

            var userRefreshToken = await _userRefreshTokenRepository.GetByIdAsync(user.Id);

            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var refreshTokenRepo = _unitOfWork.Repository<UserRefreshToken>();

                if (userRefreshToken == null)
                {
                    userRefreshToken = new UserRefreshToken
                    {
                        UserId = user.Id,
                        AccessToken = accessTokenString,
                        RefreshToken = refreshTokenString,
                        JwtId = jwtToken.Id,
                        ExpiryDate = DateTime.UtcNow.AddDays(_cookieSettings.RefreshTokenExpirationTimeInDays),
                        AddedTime = DateTime.UtcNow,
                        IsUsed = false,
                        IsRevoked = false
                    };
                    await refreshTokenRepo.AddAsync(userRefreshToken);
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
                    await refreshTokenRepo.UpdateAsync(userRefreshToken);
                }

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return new JwtAuthResult
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshTokenString
            };
        }
        public (bool IsValid, List<Claim>?, string ErrorMessage) ValidateJwtToken(string AccessToken)
        {

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = key,
                    ValidateIssuer = _jwtSettings.ValidateIssuer,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = _jwtSettings.ValidateAudience,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = _jwtSettings.ValidateLifeTime,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(AccessToken, validationParameters, out SecurityToken validatedToken);

                // Extract claims
                var claims = principal.Claims.ToList();

                return (true, claims, null);
            }
            catch (SecurityTokenExpiredException)
            {
                return (false, null, "Token has expired");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return (false, null, "Invalid token signature - algorithm or key is wrong");
            }
            catch (SecurityTokenException ex)
            {
                return (false, null, $"Invalid token: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Token validation failed: {ex.Message}");
            }
        }

        public async Task<JwtAuthResult> RefreshJwtTokenAsync(string accessToken, string refreshToken)
        {
            try
            {
                // Get the stored refresh token from database
                var storedTokenRecord = await _userRefreshTokenRepository
                    .GetTableAsTracking()
                    .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);

                // Validate refresh token exists in database
                if (storedTokenRecord == null)
                {
                    return new JwtAuthResult
                    {
                        Succeeded = false,
                        Message = "Invalid refresh token"
                    };
                }

                // Validate access token matches the stored one
                if (storedTokenRecord.AccessToken != accessToken)
                {
                    return new JwtAuthResult
                    {
                        Succeeded = false,
                        Message = "Access token and refresh token do not match"
                    };
                }

                // Validate refresh token is not expired
                if (storedTokenRecord.ExpiryDate < DateTime.UtcNow)
                {
                    return new JwtAuthResult
                    {
                        Succeeded = false,
                        Message = "Refresh token has expired"
                    };
                }

                // Get the user
                var user = await _userManager.FindByIdAsync(storedTokenRecord.UserId.ToString());
                if (user == null)
                {
                    return new JwtAuthResult
                    {
                        Succeeded = false,
                        Message = "User not found"
                    };
                }

                // Generate new access token
                var (jwtToken, newAccessTokenString) = GenerateAccessToken(user);

                // Generate new refresh token
                var newRefreshTokenString = GenerateRefreshTokenString();

                // Update database record with new tokens
                var trans = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var refreshTokenRepo = _unitOfWork.Repository<UserRefreshToken>();
                    storedTokenRecord.AccessToken = newAccessTokenString;
                    storedTokenRecord.RefreshToken = newRefreshTokenString;
                    storedTokenRecord.JwtId = jwtToken.Id;
                    storedTokenRecord.ExpiryDate = DateTime.UtcNow.AddDays(_cookieSettings.RefreshTokenExpirationTimeInDays);
                    storedTokenRecord.AddedTime = DateTime.UtcNow;

                    await refreshTokenRepo.UpdateAsync(storedTokenRecord);
                    await _unitOfWork.CommitAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }

                // Return new tokens
                return new JwtAuthResult
                {
                    Succeeded = true,
                    Message = "Token refreshed successfully",
                    AccessToken = newAccessTokenString,
                    RefreshToken = newRefreshTokenString
                };
            }
            catch (Exception ex)
            {
                return new JwtAuthResult
                {
                    Succeeded = false,
                    Message = $"Error refreshing token: {ex.Message}"
                };
            }

        }


        public async Task<bool> ConfirmEmail(int? userId, string? code)
        {
            //check for null
            if (userId == null || code == null)
                return false;
            //find user using id
            var user = await _userManager.FindByIdAsync(userId.ToString());
            //if it's not exists return false
            if (user == null) return false;
            //confirm if the tokken correct using userManager
            var confirmEmail = await _userManager.ConfirmEmailAsync(user, code);
            if (!confirmEmail.Succeeded)
                return false;
            return true;
        }


        public async Task<string> SendResetPasswordCode(string Email)
        {
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                //user
                var user = await _userManager.FindByEmailAsync(Email);
                //user not Exist => not found
                if (user == null)
                    return "UserNotFound";
                //Generate Random Number

                //Random generator = new Random();
                //string randomNumber = generator.Next(0, 1000000).ToString("D6");
                var chars = "0123456789";
                var random = new Random();
                var randomNumber = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

                //update User In Database Code
                user.Code = randomNumber;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    return "ErrorInUpdateUser";
                var message = "Code To Reset Passsword : " + user.Code;
                //Send Code To  Email 
                await _emailsService.SendEmail(user.Email, message, "Reset Password");
                await _unitOfWork.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return "Failed";
            }
        }
        public async Task<string> ConfirmResetPassword(string Code, string Email)
        {

            //Get User
            //user
            var user = await _userManager.FindByEmailAsync(Email);
            //user not Exist => not found
            if (user == null)
                return "UserNotFound";
            //Decrept Code From Database User Code
            var userCode = user.Code;
            //Equal With Code
            if (userCode == Code) return "Success";
            return "Failed";
        }


        public async Task<string> ResetPassword(string Email, string Password)
        {

            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                //Get User
                var user = await _userManager.FindByEmailAsync(Email);
                //user not Exist => not found
                if (user == null)
                    return "UserNotFound";
                await _userManager.RemovePasswordAsync(user);
                if (!await _userManager.HasPasswordAsync(user))
                {
                    await _userManager.AddPasswordAsync(user, Password);
                }
                await _unitOfWork.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return "Failed";
            }
        }



        #endregion


        #region Helpers
        private (JwtSecurityToken, string) GenerateAccessToken(User user)
        {
            var claims = GetClaims(user).Result;

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
        public async Task<List<Claim>> GetClaims(User user)
        {
            //add User Roles to claims
            var roles = await _userManager.GetRolesAsync(user);
            //if User got no roles assign User role to him
            if (!roles.Any())
            {
                await _userManager.AddToRoleAsync(user, AppRolesConstants.User);
                roles = new List<string> { AppRolesConstants.User };
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //add User Claims to claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }



        #endregion
    }
}
