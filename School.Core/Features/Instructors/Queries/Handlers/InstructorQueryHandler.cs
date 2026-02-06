using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Instructors.Queries.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Instructors.Queries.Handlers
{
    public class InstructorQueryHandler : ApiResponseHandler,
        IRequestHandler<GetSummationSalaryOfInstructorQuery, ApiResponse<object>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IInstructorService _instructorService;
        #endregion
        #region Constructors

        public InstructorQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                      IMapper mapper,
                                      IInstructorService instructorService) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _instructorService = instructorService;
        }
        #endregion
        #region Methods

        public async Task<ApiResponse<object>> Handle(GetSummationSalaryOfInstructorQuery request, CancellationToken cancellationToken)
        {
            var result = await _instructorService.GetSalarySummationOfInstructor();
            return Success((object)new { TotalSalary = result });
        }
        #endregion




    }
}
