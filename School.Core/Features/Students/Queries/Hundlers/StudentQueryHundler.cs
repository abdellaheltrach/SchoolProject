using AutoMapper;
using MediatR;
using School.Core.Base.ApiResponse;
using School.Core.Base.Wrappers;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.QueriesResponse;
using School.Core.Features.Students.Queries.Response;
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

        #endregion
        #region Constructors
        public StudentQueryHundler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
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
                return NotFound<GetStudentByIdResponse>("Student not found!");
            }
            var studentResponseMapper = _mapper.Map<GetStudentByIdResponse>(student);
            return Success(studentResponseMapper);
        }

        public async Task<ApiResponse<PaginatedResult<GetStudentPaginatedListResponse>>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        {
            var FilterQuery = _studentService.FilterStudentPaginatedQuerable(request.OrderBy, request.Search, request.SortDesc);
            var PaginatedList = await _mapper.ProjectTo<GetStudentPaginatedListResponse>(FilterQuery).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            PaginatedList.Meta = new { Count = PaginatedList.Data.Count() };
            return Success(PaginatedList);
        }



        #endregion
    }
}
