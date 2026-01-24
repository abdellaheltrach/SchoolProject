using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;
using School.Domain.Entities.Identity;

namespace School.Core.Features.Users.Commands.Validators
{
    public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;

        #endregion
        #region constructor
        public EditUserCommandValidator(IStringLocalizer<SharedResources> localizer, UserManager<User> userManager)
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
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                 .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.PropMaxLengthis100]);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.PropMaxLengthis100]);

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);


        }
        public void ApplyCustomValidationsRules()
        { }


        #endregion

    }
}
