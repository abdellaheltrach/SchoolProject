using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Base.Wrappers;
using School.Core.Features.Departement.Queries.Models;
using School.Core.Features.Departement.Queries.QueriesResponse;
using School.Core.Helpers;
using School.Core.Resources;
using School.Domain.Entities;
using School.Service.Services.Interfaces;
using System.Linq.Expressions;

namespace School.Core.Features.Departement.Queries.Hundlers
{
    public class DepartementQueryHundler : ApiResponseHandler,
        IRequestHandler<GetDepartementByIdQuery, ApiResponse<GetDepartmentByIdResponse>>,
        IRequestHandler<GetDepartmentStudentListCountQuery, ApiResponse<List<GetDepartmentStudentListCountResponse>>>
    {
        #region Fields
        private readonly IDepartmentService _DepartmentService;
        private readonly IStudentService _StudentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion


        public DepartementQueryHundler(IDepartmentService departmentService,
                                        IStudentService studentService,
                                        IMapper mapper,
                                        IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _DepartmentService = departmentService;
            _StudentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        #region Hunder
        public async Task<ApiResponse<GetDepartmentByIdResponse>> Handle(GetDepartementByIdQuery request, CancellationToken cancellationToken)
        {
            //call departement service to get departement by id including related students , subjects , instructors
            var departement = await _DepartmentService.GetDepartmentByIdIncluding_DS_Subj_Ins_InsManger(request.ID);
            //check if departement is null return not found response
            if (departement == null) return NotFound<GetDepartmentByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            //map departement to GetDepartementByIdResponse
            var mapper = _mapper.Map<GetDepartmentByIdResponse>(departement);

            // pagination of student list

            Expression<Func<Student, StudentResponse>> expression = e => new StudentResponse(e.StudentID, LocalizationHelper.GetLocalizedName(e.NameAr, e.NameEn));
            var studentQuerable = _StudentService.GetStudentsByDepartmentIDQuerable(request.ID);
            var PaginatedList = await studentQuerable.Select(expression).ToPaginatedListAsync(request.StudentPageNumber, request.StudentPageSize);
            mapper.StudentList = PaginatedList;


            //return success response with mapped data
            return Success(mapper);
        }

        public async Task<ApiResponse<List<GetDepartmentStudentListCountResponse>>> Handle(GetDepartmentStudentListCountQuery request, CancellationToken cancellationToken)
        {
            var viewDepartmentResult = await _DepartmentService.GetViewDepartmentDataAsync();
            var result = _mapper.Map<List<GetDepartmentStudentListCountResponse>>(viewDepartmentResult);
            return Success(result);
        }
        #endregion
    }
}
