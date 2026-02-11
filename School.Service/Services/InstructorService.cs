using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Bases.UnitOfWork;
using School.Infrastructure.Repositories.Interfaces;
using School.Infrastructure.Repositories.Interfaces.Functions;
using School.Service.Services._Interfaces;
using School.Service.Services.Interfaces;


namespace School.Service.Services
{
    public class InstructorService : IInstructorService
    {
        #region Fileds
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInstructorFunctionsRepository _instructorFunctionsRepository;
        private readonly IInstructorRepository _instructorsRepository;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion
        #region Constructors
        public InstructorService(IUnitOfWork unitOfWork,
                                 IInstructorRepository instructorsRepository,
                                 IInstructorFunctionsRepository instructorFunctionsRepository,
                                 IFileService fileService,
                                 IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _instructorFunctionsRepository = instructorFunctionsRepository;
            _instructorsRepository = instructorsRepository;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }



        #endregion
        public async Task<decimal> GetSalarySummationOfInstructor()
        {
            /*
             CREATE FUNCTION GetSalarySummation()
RETURNS decimal(18,2) AS
BEGIN
    DECLARE @salary decimal(18,2)
    SELECT @salary = SUM(Salary) FROM Instructors
    RETURN @salary
END


             */
            decimal result = 0;
            result = _instructorFunctionsRepository.GetSalarySummationOfInstructor("select dbo.GetSalarySummation()");
            return result;
        }


        public async Task<bool> IsNameArExist(string nameAr)
        {
            //Check if the name is Exist Or not
            var student = await _instructorsRepository.GetTableNoTracking().Where(x => x.InstructorNameAr.Equals(nameAr)).FirstOrDefaultAsync();
            if (student == null) return false;
            return true;
        }

        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            //Check if the name is Exist Or not
            var student = await _instructorsRepository.GetTableNoTracking().Where(x => x.InstructorNameAr.Equals(nameAr) & x.InstructorId != id).FirstOrDefaultAsync();
            if (student == null) return false;
            return true;
        }

        public async Task<bool> IsNameEnExist(string nameEn)
        {
            //Check if the name is Exist Or not
            var student = await _instructorsRepository.GetTableNoTracking().Where(x => x.InstructorNameEn.Equals(nameEn)).FirstOrDefaultAsync();
            if (student == null) return false;
            return true;
        }

        public async Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id)
        {
            //Check if the name is Exist Or not
            var student = await _instructorsRepository.GetTableNoTracking().Where(x => x.InstructorNameEn.Equals(nameEn) & x.InstructorId != id).FirstOrDefaultAsync();
            if (student == null) return false;
            return true;
        }
        public async Task<string> AddInstructorAsync(Instructor instructor, IFormFile file)
        {
            var context = _httpContextAccessor.HttpContext.Request;
            var baseUrl = context.Scheme + "://" + context.Host;
            var imageUrl = await _fileService.UploadImage("Instructors", file);
            switch (imageUrl)
            {
                case "NoImage": return "NoImage";
                case "FailedToUploadImage": return "FailedToUploadImage";
            }
            instructor.Image = baseUrl + imageUrl;

            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var instructorRepo = _unitOfWork.Repository<Instructor>();
                await instructorRepo.AddAsync(instructor);
                await _unitOfWork.CommitAsync();
                return "Success";
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return "FailedInAdd";
            }
        }

    }
}
