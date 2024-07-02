using EasyTestMaker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyTestMaker.Services
{
    public interface ITestService
    {
        void AddTest(Test exam);
        Task<Test> GetTestByName(string name);
        Task<List<TestResult>> GetTestResults();
        Task<List<TestResult>> GetTestResultsById(int id);
        Task<List<Test>> GetTestsAsync();
        Task<Test> GetTestStatistics(Guid examId);
        Task PutToServer(Test exam);
        Task GettestResults(TestResult result);
        Task UpdateTest(Test exam, bool pushToServer);
        Task<List<Test>> UpdateTestJson(Test exam);
    }
}