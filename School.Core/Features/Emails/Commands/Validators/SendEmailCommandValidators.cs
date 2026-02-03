using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Emails.Commands.Models;
using School.Core.Helpers;
using School.Core.Resources;

namespace School.Core.Features.Emails.Commands.Validators
{
    public class SendEmailCommandValidators : AbstractValidator<SendEmailCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Constructors
        public SendEmailCommandValidators(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();

        }
        #endregion
        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);

            RuleFor(x => x.Message)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.PropNotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.PropRequired]);
        }

        public void ApplyCustomValidationsRules()
        {

            RuleFor(x => x.Email)
                .Must(email => SignInHelper.IsEmail(email))
                .WithMessage(_localizer[SharedResourcesKeys.InvalidEmail]);

        }
        #endregion
    }
}