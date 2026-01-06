using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Reposetries.Interfaces;
using School.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Service.Services
{
    public class StudentService : IStudentService
    {
        #region fields
        private readonly IStudentRepository _studentRepository;
        #endregion


        #region constructors
        public StudentService(IStudentRepository studentRepository)
        {
            this._studentRepository = studentRepository;
        }
        #endregion

        #region methods
        public async Task<List<Student>> GetAllStudentListAsync()
        {
            return await _studentRepository.GetAllStudentListAsync();
        }

        public async Task<Student> GetStudentByIDAsync(int ID)
        {
            var student = _studentRepository.GetTableNoTracking().Include(s=>s.Department).Where(s=>s.StudentID.Equals(ID)).FirstOrDefault();
            return student;
        }

        public async Task<(bool success, string message)> AddStudentAsync(Student student)
        {
            //student is already in the system 
            var existingStudent = _studentRepository.GetTableNoTracking().Where(s => s.NameEn.Equals(student.NameEn)).FirstOrDefault();
            if (existingStudent != null) 
            {
                return (false, "The student already exists!");
            }
            //Student not in the System
            await _studentRepository.AddAsync(student);

            return (true, "Student Added Successfully!");
        }
        #endregion




    }
}
