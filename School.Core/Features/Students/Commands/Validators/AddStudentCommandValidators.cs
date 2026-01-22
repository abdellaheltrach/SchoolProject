using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Students.Commands.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Students.Commands.Validitor
{
    public class AddStudentCommandValidators : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IStudentService _studentService;
        private IDepartmentService _departmentService;
        #endregion

        #region constructor
        public AddStudentCommandValidators(IStringLocalizer<SharedResources> localizer,
            IStudentService studentService,
            IDepartmentService departmentService)
        {
            _studentService = studentService;
            _departmentService = departmentService;
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationsRules();
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

        public void ApplyCustomValidationsRules()
        {

            RuleFor(x => x.DepartementID)
           .MustAsync(async (Key, CancellationToken) => await _departmentService.IsDepartmentIdExist(Key))
           .WithMessage(_localizer[SharedResourceskeys.PropNotExist]);

        }


        #endregion
    }
}
