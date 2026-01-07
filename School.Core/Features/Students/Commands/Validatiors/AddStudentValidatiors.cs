using FluentValidation;
using School.Core.Features.Students.Commands.Models;

namespace School.Core.Features.Students.Commands.Validitor
{
    public class AddStudentValidatiors : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        #endregion

        #region constructor
        public AddStudentValidatiors()
        {
            ApplyValidationsRules();
        }
        #endregion

        #region Actions

        public void ApplyValidationsRules()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Name (Arabic) is required.")
                .MaximumLength(100).WithMessage("Name (Arabic) must not exceed 100 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(100).WithMessage("Address must not exceed 100 characters.");

            RuleFor(x => x.DepartementID)
                .NotEmpty().WithMessage("Department ID is required.");
        }


        #endregion
    }
}
