using AutoMapper;
using MediatR;
using School.Core.ApiResponse;
using School.Core.Features.Students.Commands.Models;
using School.Domain.Entities;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Students.Commands.Handlers
{
    public class StudentCommandHundler : ApiResponseHandler, IRequestHandler<AddStudentCommand, ApiResponse<string>>
                                                          , IRequestHandler<EditStudentCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        #endregion
        #region Constructors
        public StudentCommandHundler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }
        #endregion

        #region Handlers
        public async Task<ApiResponse<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var studentmapper = _mapper.Map<Student>(request);

            var result = await _studentService.AddStudentAsync(studentmapper);

            if (result.success)
            {
                return Created(result.message);
            }
            else
            {
                return BadRequest<string>(result.message);
            }


        }

        public async Task<ApiResponse<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            //Check if the Id is Exist Or not
            var student = await _studentService.GetStudentByIDAsync(request.StudentID);
            //return NotFound
            if (student == null) return NotFound<string>();
            //mapping Between request and student
            var studentmapper = _mapper.Map(request, student);
            //Call service that make Edit
            var result = await _studentService.EditAsync(studentmapper);
            //return response
            if (result == "Success") return Success("Updated Successfully");
            else return BadRequest<string>();

        }
        #endregion
    }
}
