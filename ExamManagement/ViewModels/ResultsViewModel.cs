using EasyTestMaker.Models;
using EasyTestMaker.Services;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyTestMaker.ViewModels
{
    public class ResultsViewModel : ViewModelBase
    {

        private double _averageGrade;

        public double AverageGrade
        {
            get { return _averageGrade; }
            set
            {
                SetProperty(ref _averageGrade, value);
            }
        }

        private int _numberOfStudents;

        public int NumberOfStudents
        {
            get { return _numberOfStudents; }
            set
            {
               SetProperty(ref _numberOfStudents, value);
            }
        }

        private int _correctAnswers;

        public int CorrectAnswers
        {
            get { return _correctAnswers; }
            set
            {
                SetProperty(ref _correctAnswers, value);
            }
        }
        private int _invalidAnswers;

        public int InvalidAnswers
        {
            get { return _invalidAnswers; }
            set
            {
                SetProperty(ref _invalidAnswers, value);
            }
        }
        private readonly ITestService _testService = App.GetService<ITestService>();

        public ResultsViewModel()
        {
            GetGrades();
        }


        /// <summary>
        /// Get all grades and calculate the average grade
        /// SHows Valid and Invalid answers
        /// </summary>
        private async void GetGrades()
        {
            List<TestResult> results = await _testService.GetTestResults();

            if (results == null)
            {
                MessageBox.Show("No results found");
                return;
            }
            if(results.Count == 0)
            {
                MessageBox.Show("No results found");
                return;
            }
            foreach (var result in results)
            {
                if (result.Errors.Count == 0)
                {
                    var exams = await _testService.GetTestsAsync();
                    CorrectAnswers += exams.Where(s => s.Id == result.TestId).FirstOrDefault().Questions.Count();
                }
                foreach (var error in result.Errors)
                {
                    
                    if (error.SelectedAnswer == error.CorrectAnswer)
                    {
                        CorrectAnswers++;
                       
                    }
                    else
                    {
                        InvalidAnswers++;
                    }
                }
            }

            AverageGrade = results.Average(x => x.Grade);
            NumberOfStudents = results.Select(s => s.StudentId).Count(); ;
         
            


        }
    }
}