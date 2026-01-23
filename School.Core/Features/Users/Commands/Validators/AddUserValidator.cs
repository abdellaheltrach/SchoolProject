using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;
using School.Domain.Entities.Identity;

namespace School.Core.Features.Users.Commands.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;

        #endregion
        #region constructor
        public AddUserValidator(IStringLocalizer<SharedResources> localizer, UserManager<User> userManager)
        {

            _localizer = localizer;
            _userManager = userManager;

            ApplyValidationRules();
            ApplyCustomValidationsRules();
        }
        #endregion
        #region Validation Rules
        public void ApplyValidationRules()
        {
            RuleFor(x => x.FullName)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.PropMaxLengthis100]);

            RuleFor(x => x.UserName)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.PropMaxLengthis100]);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.InvalidEmail]);

            RuleFor(x => x.Password)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                 .Equal(x => x.ConfirmationPassword).WithMessage(_localizer[SharedResourcesKeys.PasswordNotEqualConfirmationPassword]); ;



        }
        public void ApplyCustomValidationsRules()
        {
            // Check if username already exists
            RuleFor(x => x.UserName)
                .MustAsync(async (userName, cancellationToken) =>
                    await IsUserNameUnique(userName))
                .WithMessage(_localizer[SharedResourcesKeys.PropAlreadyExists]);

            // Check if email already exists
            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellationToken) =>
                    await IsEmailUnique(email))
                .WithMessage(_localizer[SharedResourcesKeys.PropAlreadyExists]);

            // Check if phone number already exists (if required)
            RuleFor(x => x.PhoneNumber)
                .MustAsync(async (phone, cancellationToken) =>
                    await IsPhoneNumberUnique(phone))
                .WithMessage(_localizer[SharedResourcesKeys.PropAlreadyExists])
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            // Validate password strength (if creating new user)
            RuleFor(x => x.Password)
                .Must(password => IsPasswordStrong(password))
                .WithMessage(_localizer[SharedResourcesKeys.PasswordTooWeak])
                .When(x => !string.IsNullOrEmpty(x.Password));

        }

        // Helper methods
        private async Task<bool> IsUserNameUnique(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return true;

            var user = await _userManager.FindByNameAsync(userName);
            return user == null;
        }

        private async Task<bool> IsEmailUnique(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        private async Task<bool> IsPhoneNumberUnique(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true;

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phone);
            return user == null;
        }

        private bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return true;
            if (password.Length < 8)
                return false;

            // At least one uppercase, one lowercase, one digit, and one special character
            var hasUpperCase = password.Any(char.IsUpper);
            var hasLowerCase = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));


            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }


        #endregion

    }
}
