using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Autorazation.Commands.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Autorazation.Commands.Validators
{
    public class DeleteRoleCommandValidators : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthorizationService _authorizationService;

        #endregion

        #region Constructors
        public DeleteRoleCommandValidators(IStringLocalizer<SharedResources> localizer,
            IAuthorizationService authorizationService)
        {
            _localizer = localizer;
            _authorizationService = authorizationService;
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


        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Id)
.MustAsync(async (Key, CancellationToken) => await _authorizationService.IsRoleExistById(Key))
.WithMessage(_localizer[SharedResourcesKeys.PropNotExist]);
        }

        #endregion
    }
}
