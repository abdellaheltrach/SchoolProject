using FluentValidation;
using School.Core.Features.Students.Commands.Models;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Students.Commands.Validitor
{
    public class EditStudentCommandValidatiors : AbstractValidator<EditStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        #endregion

        #region constructor
        public EditStudentCommandValidatiors(IStudentService studentService)
        {
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _studentService = studentService;
        }
        #endregion

        #region Actions

        public void ApplyValidationsRules()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Name (Arabic) is required.")
                .MaximumLength(100).WithMessage("Name (Arabic) must not exceed 100 characters.");

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Name (English) is required.")
                .MaximumLength(100).WithMessage("Name (English) must not exceed 100 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(100).WithMessage("Address must not exceed 100 characters.");

            RuleFor(x => x.DepartementID)
                .NotEmpty().WithMessage("Department ID is required.");
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.NameAr)
                .MustAsync(async (model, Key, CancellationToken) => !await _studentService.IsNameArExistExcludeSelf(Key, model.StudentID))
                .WithMessage("NameAr already exists!");
            RuleFor(x => x.NameEn)
                .MustAsync(async (model, Key, CancellationToken) => !await _studentService.IsNameEnExistExcludeSelf(Key, model.StudentID))
                .WithMessage("NameEn already exists!");
        }
        #endregion
    }
}
