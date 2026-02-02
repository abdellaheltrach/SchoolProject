using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Emails.Commands.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Emails.Commands.Handlers
{
    public class EmailsCommandHandler : ApiResponseHandler,
        IRequestHandler<SendEmailCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IEmailsService _emailsService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion
        #region Constructors
        public EmailsCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                    IEmailsService emailsService) : base(stringLocalizer)
        {
            _emailsService = emailsService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion
        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _emailsService.SendEmail(request.Email, request.Message, null);
            if (response)
                return Success<string>("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.SendEmailFailed]);
        }
        #endregion
    }
}
