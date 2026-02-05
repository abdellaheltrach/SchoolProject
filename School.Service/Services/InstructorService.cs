using School.Infrastructure.Context;
using School.Infrastructure.Repositories.Interfaces.Functions;
using School.Service.Services.Interfaces;

namespace School.Service.Services
{
    public class InstructorService : IInstructorService
    {
        #region Fileds
        private readonly AppDbContext _dbContext;
        private readonly IInstructorFunctionsRepository _instructorFunctionsRepository;

        #endregion
        #region Constructors
        public InstructorService(AppDbContext dbContext,
                                 IInstructorFunctionsRepository instructorFunctionsRepository)
        {
            _dbContext = dbContext;
            _instructorFunctionsRepository = instructorFunctionsRepository;
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
    }
}
