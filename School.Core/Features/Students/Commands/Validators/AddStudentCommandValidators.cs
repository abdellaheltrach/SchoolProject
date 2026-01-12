using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Students.Commands.Models;
using School.Core.Resources;

namespace School.Core.Features.Students.Commands.Validitor
{
    public class AddStudentCommandValidators : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region constructor
        public AddStudentCommandValidators(IStringLocalizer<SharedResources> localizer)
        {
            ApplyValidationRules();
            _localizer = localizer;
        }
        #endregion

        #region Actions

        public void ApplyValidationRules()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage(_localizer[SharedResourceskeys.NameArRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourceskeys.NameArMaxLength]);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_localizer[SharedResourceskeys.AddressRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourceskeys.AddressMaxLength]);

            RuleFor(x => x.DepartementID)
                .NotEmpty().WithMessage(_localizer[SharedResourceskeys.DepartementIDRequired]);
        }



        #endregion
    }
}
