using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Base.ApiResponse;
using School.Core.Features.Departement.Queries.Models;
using School.Core.Features.Departement.Queries.QueriesResponse;
using School.Core.Resources;
using School.Service.Services.Interfaces;

namespace School.Core.Features.Departement.Queries.Hundlers
{
    public class DepartementQueryHundler : ApiResponseHandler,
        IRequestHandler<GetDepartementByIdQuery, ApiResponse<GetDepartmentByIdResponse>>
    {
        #region Fields
        private readonly IDepartmentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion


        public DepartementQueryHundler(IDepartmentService studentService,
                                        IMapper mapper,
                                        IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        #region Hunder
        public async Task<ApiResponse<GetDepartmentByIdResponse>> Handle(GetDepartementByIdQuery request, CancellationToken cancellationToken)
        {
            //call departement service to get departement by id including related students , subjects , instructors
            var departement = await _studentService.GetDepartmentByIdIncluding_St_DS_Subj_Ins_InsManger(request.ID);
            //check if departement is null return not found response
            if (departement == null) return NotFound<GetDepartmentByIdResponse>(_stringLocalizer[SharedResourceskeys.NotFound]);
            //map departement to GetDepartementByIdResponse
            var mapper = _mapper.Map<GetDepartmentByIdResponse>(departement);
            //return success response with mapped data
            return Success(mapper);
        }
        #endregion
    }
}
