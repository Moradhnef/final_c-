using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTestMaker.Models
{
    public class TestResult
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public Test Test { get; set; }
        public Guid TestId { get; set; }
        public int Grade { get; set; }
        public List<Error> Errors { get; set; }
        public TestResult()
        {
            Errors= new List<Error>();
        }

        public TestResult(int studentId, string studentName, Guid examId, int grade, List<Error> errors)
        {
            StudentId = studentId;
            StudentName = studentName;
            TestId = examId;
            Grade = grade;
            Errors = errors;
        }
    }
}