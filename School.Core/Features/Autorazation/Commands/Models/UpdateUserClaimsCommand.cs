using MediatR;
using School.Core.Base.ApiResponse;
using School.Domain.Results.Requests;

namespace School.Core.Features.Autorazation.Commands.Models
{
    public class UpdateUserClaimsCommand : UpdateUserClaimsRequest, IRequest<ApiResponse<string>>
    {
    }
}
