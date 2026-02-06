using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories.Interfaces.Functions;
using System.Data;

namespace School.Infrastructure.Repositories.Functions
{
    public class InstructorFunctionsRepository : IInstructorFunctionsRepository
    {
        #region Fileds
        private readonly AppDbContext _dbContext;
        #endregion
        #region Constructors
        public InstructorFunctionsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        #region Handle Functions
        public decimal GetSalarySummationOfInstructor(string query)
        {
            using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                //read From List

                //  var reader = await cmd.ExecuteReaderAsync();
                // var value = await reader.ToListAsync<GetInstructorFunctionResult>();

                decimal response = 0;
                cmd.CommandText = query;
                var value = cmd.ExecuteScalar();
                var result = value.ToString();
                if (decimal.TryParse(result, out decimal d))
                {
                    response = d;
                }
                cmd.Connection.Close();
                return response;
            }
        }
        #endregion
    }
}
