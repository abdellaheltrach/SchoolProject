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
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.PropMaxLengthis100]);

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.PropMaxLengthis100]);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropRequired])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.PropMaxLengthis100]);

            RuleFor(x => x.DepartementID)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);
        }


        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.NameAr)
                .MustAsync(async (model, key, cancellationToken) =>
                    !await _studentService.IsNameArExistExcludeSelf(key, model.StudentID))
                .WithMessage(_localizer[SharedResourcesKeys.PropAlreadyExists]);

            RuleFor(x => x.NameEn)
                .MustAsync(async (model, key, cancellationToken) =>
                    !await _studentService.IsNameEnExistExcludeSelf(key, model.StudentID))
                .WithMessage(_localizer[SharedResourcesKeys.PropAlreadyExists]);
        }

        #endregion
    }
}
