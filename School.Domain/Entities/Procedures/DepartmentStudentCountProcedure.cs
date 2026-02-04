namespace School.Domain.Entities.Procedures
{
    public class DepartmentStudentCountProcedure
    {
        /*
         * CREATE PROC DepartmentStudentCountProcedure
            @DepartmentId INT = 0
        AS
        BEGIN
            CREATE TABLE #temp(
                Id INT,
                DepartmentNameAr NVARCHAR(200),
                DepartmentNameEn NVARCHAR(200),
                TotalStudents INT
            )

            INSERT INTO #temp
            SELECT 
                d.Id,
                d.DepartmentNameAr,
                d.DepartmentNameEn,
                COUNT(s.StudentID) AS TotalStudents
            FROM departments d
            INNER JOIN students s ON d.Id = s.DepartementId
            WHERE d.Id = CASE WHEN @DepartmentId = 0 THEN d.Id ELSE @DepartmentId END
            GROUP BY d.Id, d.DepartmentNameAr, d.DepartmentNameEn

            SELECT * FROM #temp
        END
         */
        public int Id { get; set; }
        public string? DepartmentNameAr { get; set; }
        public string? DepartmentNameEn { get; set; }
        public int TotalStudents { get; set; }
    }
    public class DepartmentStudentCountProcedureParameters
    {
        public int DepartmentId { get; set; } = 0;
    }
}
