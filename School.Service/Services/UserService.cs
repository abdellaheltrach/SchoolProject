using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Domain.Entities.Identity;
using School.Domain.Helpers;
using School.Infrastructure.Context;
using School.Service.Services.Interfaces;

namespace School.Service.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailsService _emailsService;
        private readonly AppDbContext _applicationDBContext;
        private readonly IUrlHelper _urlHelper;
        #endregion
        #region Constructors
        public UserService(UserManager<User> userManager,
                                      IHttpContextAccessor httpContextAccessor,
                                      IEmailsService emailsService,
                                      AppDbContext applicationDBContext,
                                      IUrlHelper urlHelper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailsService = emailsService;
            _applicationDBContext = applicationDBContext;
            _urlHelper = urlHelper;
        }
        #endregion
        #region Handle Functions
        public async Task<string> AddUserAsync(User user, string password)
        {
            var trans = await _applicationDBContext.Database.BeginTransactionAsync();
            try
            {
                //Search the user using email
                var existUser = await _userManager.FindByEmailAsync(user.Email);
                //failed if email is exist
                if (existUser != null) return "EmailIsExist";

                //Search the user using username i
                var userByUserName = await _userManager.FindByNameAsync(user.UserName);
                //failed if username is Exist
                if (userByUserName != null) return "UserNameIsExist";

                //Create user
                var createResult = await _userManager.CreateAsync(user, password);
                //handle create failed
                if (!createResult.Succeeded)
                    return string.Join(",", createResult.Errors.Select(x => x.Description).ToList());
                //give it a 
                await _userManager.AddToRoleAsync(user, AppRolesConstants.User);

                //Send Confirm Email to the user
                var sendEmailResult = await _emailsService.SendEmailConfirmationMail(user);

                if (!sendEmailResult) return "Failed";


                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "Failed";
            }

        }
        #endregion
    }
}
