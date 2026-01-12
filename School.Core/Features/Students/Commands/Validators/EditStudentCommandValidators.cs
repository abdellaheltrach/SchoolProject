using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Students.Commands.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Students.Commands.Validitor
{
    public class EditStudentCommandValidators : AbstractValidator<EditStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region constructor
        public EditStudentCommandValidators(IStudentService studentService, IStringLocalizer<SharedResources> localizer)
        {
            _studentService = studentService;
            _localizer = localizer;

            ApplyValidationRules();
            ApplyCustomValidationRules();
            _studentService = studentService;
            _localizer = localizer;
        }
        #endregion

        #region Actions

        public void ApplyValidationRules()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage(_localizer[SharedResourceskeys.NameArRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourceskeys.NameArMaxLength]);

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage(_localizer[SharedResourceskeys.NameEnRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourceskeys.NameEnMaxLength]);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_localizer[SharedResourceskeys.AddressRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourceskeys.AddressMaxLength]);

            RuleFor(x => x.DepartementID)
                .NotEmpty().WithMessage(_localizer[SharedResourceskeys.DepartementIDRequired]);
        }


        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.NameAr)
                .MustAsync(async (model, key, cancellationToken) =>
                    !await _studentService.IsNameArExistExcludeSelf(key, model.StudentID))
                .WithMessage(_localizer[SharedResourceskeys.NameArExists]);

            RuleFor(x => x.NameEn)
                .MustAsync(async (model, key, cancellationToken) =>
                    !await _studentService.IsNameEnExistExcludeSelf(key, model.StudentID))
                .WithMessage(_localizer[SharedResourceskeys.NameEnExists]);
        }

        #endregion
    }
}
