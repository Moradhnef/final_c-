using EasyTestMaker.Event;
using EasyTestMaker.Models;
using EasyTestMaker.Services;
using HandyControl.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EasyTestMaker.ViewModels
{
    public class CreateTestViewModel : ViewModelBase
    {
        private readonly ITestService _service = App.GetService<ITestService>();

        private bool _canSave;

        public bool CanSave
        {
            get { return _canSave; }
            set => SetProperty(ref _canSave, value);
        }



        private QuestionViewModel _questionViewModel;

        public QuestionViewModel QuestionViewModel
        {
            get { return _questionViewModel; }
            set => SetProperty(ref _questionViewModel, value);
        }


        private Test _test;

        public Test Test
        {
            get { return _test; }
            set
            {
                SetProperty(ref _test, value);
              
            }
        }
        private Question _question;

        public Question Question
        {
            get { return _question; }
            set => SetProperty(ref _question, value);
        }

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set => SetProperty(ref _isVisible,value);
        }


        private ObservableCollection<Question> _observableQuestions;

        public ObservableCollection<Question> ObservableQuestions
        {
            get { return _observableQuestions; }
            set
            {
                SetProperty(ref _observableQuestions, value);
               
            }
        }
        public bool IsEditMode { get; set; }
        public ICommand CreateCommand { get;}
        public ICommand UpdateServerCommand { get;}
        public ICommand CreateQuestions { get;}
      
        public ICommand EditCommand { get;}
        public ICommand Delete { get;}
        public CreateTestViewModel()
        {

            Initialize();
            CreateCommand = new Prism.Commands.DelegateCommand(CreateTest);
            UpdateServerCommand = new Prism.Commands.DelegateCommand(Update);
            CreateQuestions = new Prism.Commands.DelegateCommand(OpenQuestionsWindow);
            EditCommand = new Prism.Commands.DelegateCommand(EditQuestion);
            Delete = new Prism.Commands.DelegateCommand(DeleteQuestion);
            ObservableQuestions = new ObservableCollection<Question>();
            IsVisible = true;
         
        }

        private void DeleteQuestion()
        {
            var result = HandyControl.Controls.MessageBox.Show("Are you sure you want to remove this Question from exam?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Test.Questions.Remove(Question);
                ObservableQuestions.Remove(Question);
                Question = null;
             
            }
        }

        public CreateTestViewModel(Test test)
        {
            Test = test;
            ObservableQuestions = new ObservableCollection<Question>(Test.Questions);
            CreateCommand = new Prism.Commands.DelegateCommand(CreateTest);
            UpdateServerCommand = new Prism.Commands.DelegateCommand(Update);
            CreateQuestions = new Prism.Commands.DelegateCommand(OpenQuestionsWindow);
            EditCommand = new Prism.Commands.DelegateCommand(EditQuestion);
            IsVisible = true;
            Delete = new Prism.Commands.DelegateCommand(DeleteQuestion);
            IsEditMode = true;
        }
        private void EditQuestion()
        {
         
            IsVisible = false;
            QuestionViewModel = new QuestionViewModel(this, Question);
        }

    
        private void OpenQuestionsWindow()
        {
            IsVisible = false;
            QuestionViewModel = new QuestionViewModel(this);
            
        }
        private bool CanSaveTest()
        {
            var outout =  !string.IsNullOrEmpty(Test.Name)
                   && !string.IsNullOrEmpty(Test.LecturerName)
                   && Test.TotalTime.TotalSeconds > 0
                   && Test.Questions.Count > 0;
            return outout;
            
        }

        private void Update()
        {
            Task.Run(() => SendToServerAsync());
        }

        private void Initialize()
        {
           Test = new Test();
           ObservableQuestions = new ObservableCollection<Question>();

        }
   

        public void Reset()
        {
            Test = new Test();
            ObservableQuestions = new ObservableCollection<Question>();
        }


        private async Task SendToServerAsync()
        {
            try
            {
                await _service.UpdateTest(Test, true);
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(ex.Message, "Error");
            }
        }
        private void CreateTest()
        {
            if (IsEditMode)
            {
                _service.UpdateTest(Test, true);
            }
            else
            {
                _service.AddTest(Test);
            }

           
            var result = HandyControl.Controls.MessageBox.Show("Would you like to create another test?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Test = new Test();
                ObservableQuestions = new ObservableCollection<Question>();

            }
            else
            {
                var question = HandyControl.Controls.MessageBox.Show("Would you like to push this test to server?", "", MessageBoxButton.YesNo);
                if (question == MessageBoxResult.Yes)
                {
                    try
                    {
                        Task.Run(() => PushToServer()).ContinueWith(s=>
                        {
                            Reset();
                        });
                        
                    }
                    catch (Exception ex)
                    {
                        HandyControl.Controls.MessageBox.Show(ex.Message, "Error");
                    }
                }
                else
                {
                    Reset();
                }

            }

        }
         
        private async Task PushToServer()
        {

           await _service.UpdateTest(Test, true);

        }
    }
}
