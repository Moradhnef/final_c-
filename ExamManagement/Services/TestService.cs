using EasyTestMaker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EasyTestMaker.Services
{
    public class TestService : ITestService
    {
        private readonly HttpClient _httpClient;
        private string Path = Directory.GetCurrentDirectory() + "/Temp/file";

        public TestService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Test>> GetTestsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Test>>("http://localhost:7129/api/test");
        }

        public async Task<List<TestResult>> GetTestResultsById(int id)
        {
            return await _httpClient.GetFromJsonAsync<List<TestResult>>($"http://localhost:7129/api/result/{id}");
        }

        public async Task<List<TestResult>> GetTestResults()
        {
            return await _httpClient.GetFromJsonAsync<List<TestResult>>("http://localhost:7129/api/result");
        }

        public async Task<Test> GetTestByName(string name)
        {
            return await _httpClient.GetFromJsonAsync<Test>($"http://localhost:7129/api/Test/{name}");
        }

        public void AddTest(Test test)
        {
            SaveExamToJson(test, Path);
        }

        public async Task PutToServer(Test test)
        {
            await _httpClient.PutAsJsonAsync("http://localhost:7129/api/test", test);
        }

        public async Task<List<Test>> UpdateTestJson(Test test)
        {
            string json = File.ReadAllText(Path + "/tests.json");
            List<Test> tests = System.Text.Json.JsonSerializer.Deserialize<List<Test>>(json);
            var testUpdate = tests.FirstOrDefault(s => s.Id == test.Id);

            if (testUpdate != null)
            {
                tests.Remove(testUpdate);
                tests.Add(test);
                json = System.Text.Json.JsonSerializer.Serialize(test);
                File.WriteAllText(Path + "/tests.json", json);
            }
            return tests;
        }

        public async Task UpdateTest(Test test, bool pushToServer)
        {
            var tests = await UpdateTestJson(test);

            if (pushToServer)
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:7129/api/test", tests);
               
            }
        }

        public async Task GettestResults(TestResult result)
        {
            await _httpClient.PostAsJsonAsync("http://localhost:7129/api/result", result);
        }

        public async Task<Test> GetTestStatistics(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Test>($"http://localhost:7129/api/results/{id}");
        }

        private void SaveExamToJson(Test test, string filePath)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            if (File.Exists(Path + "/tests.json"))
            {
                string json = File.ReadAllText(Path + "/tests.json");

                List<Test> exams = null;
                try
                {
                    // Try to deserialize the JSON as an array of Test objects
                    exams = JsonConvert.DeserializeObject<List<Test>>(json);
                }
                catch
                {
                    // If deserialization fails, assume the JSON file contains a single Test object
                    Test exam = JsonConvert.DeserializeObject<Test>(json);
                    exams = new List<Test> { exam };
                }

                if (exams.Where(s => s.Id == test.Id).FirstOrDefault() == null)
                {
                    exams.Add(test);
                }

                // Serialize the updated list of exams to JSON
                json = JsonConvert.SerializeObject(exams, Formatting.Indented);

                // Write the JSON back to the file
                File.WriteAllText(Path + "/tests.json", json);
            }
            else
            {
                List<Test> testList = new List<Test> { test };
                string examJson = System.Text.Json.JsonSerializer.Serialize(testList);

                File.WriteAllText(Path + "/tests.json", examJson);
            }
        }
    }
}
