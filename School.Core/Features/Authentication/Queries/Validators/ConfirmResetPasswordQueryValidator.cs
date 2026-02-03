using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authentication.Queries.Models;
using School.Core.Resources;

namespace School.Core.Features.Authentication.Queries.Validators
{
    public class ConfirmResetPasswordQueryValidator : AbstractValidator<ConfirmResetPasswordQuery>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ConfirmResetPasswordQueryValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Code)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);

        }

        public void ApplyCustomValidationsRules()
        {
        }

        #endregion

    }
}
