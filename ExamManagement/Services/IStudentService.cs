using EasyTestMaker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyTestMaker.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudentsAsync();
        Task SaveExamResult(int studentId, string studentName, Guid id, int grade, List<Error> errors);
        Task<Test> StartExam(Test exam);
        Task<bool> SubmitTestAnswers(Test exam, int studentId, StudentTest studentExam);
        Task<Student> UpdateStudentInformation(Student student);
    }
}