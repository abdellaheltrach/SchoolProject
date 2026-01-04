using AutoMapper;
using MediatR;
using School.Core.ApiResponse;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.Response;
using School.Service.Services.Interfaces;


namespace School.Core.Features.Students.Queries.Hundlers
{
    public class StudentHundler : ApiResponseHandler,  IRequestHandler<GetStudentListQuery,ApiResponse< List<GetStudentListResponse>>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        #endregion
        #region Constructors
        public StudentHundler(IStudentService studentService, IMapper mapper)
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

 
        #endregion
    }
}
