using EasyTestMaker.Constants;
using EasyTestMaker.Models;
using EasyTestMaker.Services;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace EasyTestMaker.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        private string _searchBar;

        public string SearchBar
        {
            get { return _searchBar; }
            set
            {
                if (value != _searchBar)
                {
                    SelectedTest = new Test();
                    Tests = new ObservableCollection<Test>();
                    SetProperty(ref _searchBar, value);
                }
            }
        }

        private bool _isProgressVisible;

        public bool IsProgressVisible
        {
            get { return _isProgressVisible; }
            set
            {
                SetProperty(ref _isProgressVisible, value);
            }
        }


        private int _progressBarValue;

        public int ProgressBarValue

        {
            get { return _progressBarValue; }
            set
            {
                SetProperty(ref _progressBarValue, value);
            }
        }
        private bool _isButtonEnabled;

        public bool IsButtonEnabled

        {
            get { return _isButtonEnabled; }
            set
            {
                SetProperty(ref _isButtonEnabled, value);
            }
        }



        private ObservableCollection<Test> _tests;

        public ObservableCollection<Test> Tests
        {
            get { return _tests; }
            set
            {
                SetProperty(ref _tests, value);
            }
        }
        private Test _selectedTest;

        public Test SelectedTest
        {
            get { return _selectedTest; }
            set
            {
                if (value != null)
                {
                    IsButtonEnabled = true;
                }
                if (value != _selectedTest)
                {
                    IsButtonEnabled = true;

                }
                if (!string.IsNullOrEmpty(value.Name))
                {
                    IsButtonEnabled = true;
                }
                else
                {
                    IsButtonEnabled = false;
                }
                if (value == null)
                {
                    IsButtonEnabled = false;
                }
               SetProperty(ref _selectedTest, value);
            }
        }


        private readonly ITestService _testService = App.GetService<ITestService>();



        private string _studentName;

        public string StudentName
        {
            get { return _studentName; }
            set
            {
                if (SelectedTest == null)
                {
                    IsButtonEnabled = false;
                    SetProperty(ref _studentName, value);
                    return;
                }
                if (string.IsNullOrEmpty(value))
                {
                    IsButtonEnabled = false;
                }
                else if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(SelectedTest.Name))
                {
                    IsButtonEnabled = true;
                }

                SetProperty(ref _studentName, value);
            }
        }

        private Test _test;

        public Test Test
        {
            get { return _test; }
            set
            {
                if (value != null)
                {
                    IsButtonEnabled = true;
                }
                else
                {
                    IsButtonEnabled = false;
                }
                if (!string.IsNullOrEmpty(value.Name))
                {
                    IsButtonEnabled = true;
                }
                else
                {
                    IsButtonEnabled = false;
                }
                 SetProperty(ref _test, value);
            }
        }
        public ICommand Open { get;}
        public ICommand Search { get; }
        public TestViewModel()
        {
            Tests = new ObservableCollection<Test>();
            _testService = new TestService();
            Open = new DelegateCommand(StartTest);
            Search = new DelegateCommand(SearchTest);
        }

        private async void SearchTest()
        {
            try
            {
                IsProgressVisible = true;
                if (!string.IsNullOrEmpty(SearchBar))
                {
                    Test exam = await _testService.GetTestByName(SearchBar);
                    if (exam == null || string.IsNullOrEmpty(exam.Name))
                    {
                        IsProgressVisible = false;
                        IsButtonEnabled = false;
                        return;

                    }
                    else
                    {
                        if (Tests.Count > 0)
                        {
                            Tests = new ObservableCollection<Test>();
                        }

                        Test = exam;
                        Tests.Add(Test);
                        SetProperty(ref _test, exam);
                        IsProgressVisible = false;
                    }

                }
            }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

            }
        /// <summary>
        /// When clicking on search exam button
        /// this method is called
        /// it searches exam by name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// When clicking on exam to open this method is called
        /// it creates new student and stores him in db
        /// student service start exam checks if exam is in valid Time 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StartTest()
        {
            try
            {
                StudentService studentService = new StudentService();
                if (string.IsNullOrEmpty(Const.Username))
                {
                    throw new ArgumentException("You need to write name", nameof(Const.Username));
                }
                var examFromServer = await studentService.StartExam(Test);
                var examWindow = new ExamWindow(examFromServer);
                examWindow.Show();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

    }




}
