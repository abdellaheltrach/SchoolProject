using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Base.Wrappers;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.QueriesResponse;
using School.Core.Features.Students.Queries.Response;
using School.Core.Resources;
using School.Service.Services.Interfaces;


namespace School.Core.Features.Students.Queries.Hundlers
{
    public class StudentQueryHundler : ApiResponseHandler,
        IRequestHandler<GetStudentListQuery, ApiResponse<List<GetStudentListResponse>>>,
        IRequestHandler<GetStudentByIdQuery, ApiResponse<GetStudentByIdResponse>>,
        IRequestHandler<GetStudentPaginatedListQuery, ApiResponse<PaginatedResult<GetStudentPaginatedListResponse>>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion
        #region Constructors
        public StudentQueryHundler(IStudentService studentService,
                                        IMapper mapper,
                                        IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        #endregion
        #region Hunder
        public async Task<ApiResponse<List<GetStudentListResponse>>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentList = await _studentService.GetAllStudentListAsync();
            var studentListResponseMapper = _mapper.Map<List<GetStudentListResponse>>(studentList);
            return Success(studentListResponseMapper);
        }

        public async Task<ApiResponse<GetStudentByIdResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentService.GetStudentByIdWithNoTrachingAsync(request.ID);
            if (student == null)
            {
                return NotFound<GetStudentByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            }
            var studentResponseMapper = _mapper.Map<GetStudentByIdResponse>(student);
            return Success(studentResponseMapper);
        }

        public async Task<ApiResponse<PaginatedResult<GetStudentPaginatedListResponse>>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        {
            var FilterQuery = _studentService.FilterStudentPaginatedQuerable(request.Search, request.OrderBy, request.SortDesc);
            var PaginatedList = await _mapper.ProjectTo<GetStudentPaginatedListResponse>(FilterQuery).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return Success(PaginatedList);
        }



        #endregion
    }
}
