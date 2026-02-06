using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Instructors.Commands.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Instructors.Commands.Validators
{
    public class AddInstructorValidator : AbstractValidator<AddInstructorCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IDepartmentService _departmentService;
        private readonly IInstructorService _instructorService;
        #endregion

        #region Constructors
        public AddInstructorValidator(IStringLocalizer<SharedResources> localizer,
                                      IDepartmentService departmentService,
                                      IInstructorService instructorService)
        {
            _localizer = localizer;
            _instructorService = instructorService;
            _departmentService = departmentService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();

        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.InstructorNameAr)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);

            RuleFor(x => x.InstructorNameEn)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);

            RuleFor(x => x.DepartementId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.InstructorNameAr)
                .MustAsync(async (Key, CancellationToken) => !await _instructorService.IsNameArExist(Key))
                .WithMessage(_localizer[SharedResourcesKeys.PropAlreadyExists]);
            RuleFor(x => x.InstructorNameEn)
               .MustAsync(async (Key, CancellationToken) => !await _instructorService.IsNameEnExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.PropAlreadyExists]);

            RuleFor(x => x.DepartementId)
           .MustAsync(async (Key, CancellationToken) => await _departmentService.IsDepartmentIdExist(Key))
           .WithMessage(_localizer[SharedResourcesKeys.PropNotExist]);

        }

        #endregion

    }
}