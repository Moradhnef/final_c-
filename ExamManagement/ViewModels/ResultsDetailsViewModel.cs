using EasyTestMaker.Constants;
using EasyTestMaker.Models;
using EasyTestMaker.Services;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTestMaker.ViewModels
{
    public class ResultsDetailsViewModel : ViewModelBase
    {
        private ObservableCollection<TestResult> _testResults;

        public ObservableCollection<TestResult> TestResults
        {
            get { return _testResults; }
            set
            {
                SetProperty(ref _testResults, value);
            }
        }
        private readonly ITestService _testService = App.GetService<ITestService>();

        public ResultsDetailsViewModel()
        {
            
            Initialize();
        }
        private string _username;

        public string Username
        {
            get { return _username; }
            set => SetProperty(ref _username, value);
        }

        private async void Initialize()
        {
            try
            {
                var results = await _testService.GetTestResultsById(Const.UserId);
                TestResults = new ObservableCollection<TestResult>(results);
                
                MapResultName();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void MapResultName()
        {
            foreach(var result in TestResults)
            {
                result.StudentName = Const.Username;
            }
        }
    }
}
