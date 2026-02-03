using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authentication.Queries.Models;
using School.Core.Resources;

namespace School.Core.Features.Authentication.Queries.Validators
{
    public class EmailConfirmationQueryValidators : AbstractValidator<EmailConfirmationQuery>
    {

        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion

        #region Constructor
        public EmailConfirmationQueryValidators(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;

            ApplyValidationRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Validators

        public void ApplyValidationRules()
        {
            RuleFor(x => x.UserId)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty]);
            RuleFor(x => x.code)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty]);



        }
        public void ApplyCustomValidationsRules()
        {


        }
        #endregion
    }
}
