using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Resources;

namespace School.Core.Features.Authentication.Commands.Validators
{
    public class EditRoleCommandValidators : AbstractValidator<EditRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditRoleCommandValidators(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);

            RuleFor(x => x.NewRoleName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);
        }

        public void ApplyCustomValidationsRules()
        {
        }

        #endregion

    }
}
