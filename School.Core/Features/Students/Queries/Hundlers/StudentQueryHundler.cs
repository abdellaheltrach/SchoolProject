using AutoMapper;
using Azure.Core;
using MediatR;
using School.Core.ApiResponse;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.QueriesResponse;
using School.Core.Features.Students.Queries.Response;
using School.Service.Services.Interfaces;


namespace School.Core.Features.Students.Queries.Hundlers
{
    public class StudentQueryHundler : ApiResponseHandler,
        IRequestHandler<GetStudentListQuery,ApiResponse< List<GetStudentListResponse>>>,
        IRequestHandler<GetStudentByIdQuery, ApiResponse<GetStudentByIdResponse>>
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
        public async Task<ApiResponse< List<GetStudentListResponse>>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentList = await _studentService.GetAllStudentListAsync();
            var studentListResponseMapper = _mapper.Map<List<GetStudentListResponse>>(studentList);
            return Success( studentListResponseMapper);
        }

        public async Task<ApiResponse<GetStudentByIdResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentService.GetStudentByIDAsync(request.ID);
            if (student == null) { 
                return NotFound<GetStudentByIdResponse>("Student not found!");
             }
            var studentResponseMapper = _mapper.Map<GetStudentByIdResponse>(student);
            return Success(studentResponseMapper);
        }



        #endregion
    }
}
