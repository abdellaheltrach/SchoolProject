using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Autorazation.Commands.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Autorazation.Commands.Validators
{
    public class AddRoleCommandValidators : AbstractValidator<AddRoleCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion
        #region Constructors
        public AddRoleCommandValidators(IStringLocalizer<SharedResources> stringLocalizer,
            IAuthorizationService authorizationService)
        {
            _localizer = stringLocalizer;
            _authorizationService = authorizationService;
            ApplyValidationRules();
            ApplyCustomValidationsRules();
        }
        #endregion
        #region Methods

        public void ApplyValidationRules()
        {

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.PropMaxLengthis100]);

        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.RoleName)
    .MustAsync(async (Key, CancellationToken) => !await _authorizationService.IsRoleExistByName(Key))
    .WithMessage(_localizer[SharedResourcesKeys.PropAlreadyExists]);


        }


        #endregion
    }
}
