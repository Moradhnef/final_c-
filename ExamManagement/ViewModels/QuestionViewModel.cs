using System.Collections.ObjectModel;
using System;
using EasyTestMaker.Models;
using EasyTestMaker.Services;
using Prism.Events;
using Microsoft.Win32;
using HandyControl.Controls;
using EasyTestMaker.Event;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Prism.Commands;
using Newtonsoft.Json.Linq;

namespace EasyTestMaker.ViewModels
{
    public class QuestionViewModel : ViewModelBase
    {
        private Question _question;

        public Question Question
        {
            get { return _question; }
            set
            {
                SetProperty(ref _question, value);
            }
        }


        private ObservableCollection<Question> _questions;

        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set
            {
                SetProperty(ref _questions, value);
            }
        }

        private CreateTestViewModel _createTestViewModel;

        public CreateTestViewModel CreateTestViewModel
        {
            get { return _createTestViewModel; }
            set => SetProperty(ref _createTestViewModel, value);    
        }



        private ObservableCollection<Answer> _answers;

        public ObservableCollection<Answer> Answers
        {
            get { return _answers; }
            set
            {
                SetProperty(ref _answers, value);
            }
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

        public Guid TestGuid { get; set; }


        public string Media { get; set; }
        private bool _isImageQuestion;

        public bool IsImageQuestion
        {
            get { return _isImageQuestion; }
            set
            {
                SetProperty(ref _isImageQuestion, value);
                IsTextQuestion = !value;
                //SetField(ref _isTextQuestion, value, nameof(IsTextQuestion));
            }
        }

        private bool _isTextQuestion;

        public bool IsTextQuestion
        {
            get { return !_isImageQuestion; }
            set
            {
                SetProperty(ref _isTextQuestion, value);
            }
        }


        private bool _randomizeAnswers;

        public bool RandomizeAnswers
        {
            get { return _randomizeAnswers; }
            set
            {
                Question.AnswersSortedRandomly = value;
                SetProperty(ref _randomizeAnswers, value);
            }
        }


        private string _questionText;

        public string QuestionText
        {
            get { return _questionText; }
            set=> SetProperty(ref _questionText, value);
        }

        private BitmapImage _image;

        public BitmapImage Image
        {
            get { return _image; }
            set => SetProperty(ref _image, value);
        }

        private string _answerText;

        public string AnswerText
        {
            get { return _answerText; }
            set => SetProperty(ref _answerText, value);
        }


        private Answer _answer;

        public Answer Answer
        {
            get { return _answer; }
            set
            {
                SetProperty(ref _answer, value);

            }
        }

        private Answer _selectedAnswer;

        public Answer SelectedAnswer
        {
            get { return _selectedAnswer; }
            set => SetProperty(ref _selectedAnswer, value);
        }


        private bool _isCorrect;

        public bool IsCorrect

        {
            get { return _isCorrect; }
            set
            {
                if (IsEditMode)
                {
                    if (value == true)
                    {
                        Question.CorrectAnswerIndex = Question.Answers.IndexOf(SelectedAnswer);
                        TextVisible = true;
                        value = false;
                        ShowIsCorrectAnswer = value;
                        SetProperty(ref _isCorrect, value);
                    }
                }
                else
                {
                    SetProperty(ref _isCorrect, value);
                }
             
            }
        }

        private bool _textVisible;

        public bool TextVisible
        {
            get { return _textVisible; }
            set => SetProperty(ref _textVisible, value);
        }

        private bool _showIsCorrectAnswer;

        public bool ShowIsCorrectAnswer
        {
            get { return _showIsCorrectAnswer; }
            set
            {

                SetProperty(ref _showIsCorrectAnswer, value);
            }
        }

        private bool _isEditMode;

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set => SetProperty(ref _isEditMode, value);
        }

        private readonly IImageService _imageService = App.GetService<IImageService>();
        private readonly IEventAggregator @event = App.GetService<IEventAggregator>();


        public ICommand AddImageCommand { get;}
        public ICommand AddAnotherQuestionCommand { get;}
        public ICommand SaveQuestionCommand { get;}
        public ICommand AddAnswerCommand { get;set;}
        public QuestionViewModel(CreateTestViewModel model)
        {
            
            Question = new Question();
            Questions = model.ObservableQuestions;
            Answers = new ObservableCollection<Answer>();
            Answer = new Answer();
            Test = model.Test;
            ShowIsCorrectAnswer = true;
            CreateTestViewModel = model;
            AddImageCommand = new DelegateCommand(AddImage);
            AddAnotherQuestionCommand = new DelegateCommand(AddAnotherQuestion);
            SaveQuestionCommand = new DelegateCommand(SaveQuestion);
            IsTextQuestion = true;
            IsImageQuestion = false;
            AddAnswerCommand = new DelegateCommand(AddAnswer);

        }
        public QuestionViewModel(CreateTestViewModel model, Question question)
        {

            Question = question;
            Questions = model.ObservableQuestions;
            Answers = new ObservableCollection<Answer>();
            Answer = new Answer();
            Test = model.Test;
            IsEditMode = true;

            SelectedAnswer = question.Answers[question.CorrectAnswerIndex];
            IsCorrect = true;
            ShowIsCorrectAnswer = false;
            CreateTestViewModel = model;
            AddImageCommand = new DelegateCommand(AddImage);
            AddAnotherQuestionCommand = new DelegateCommand(AddAnotherQuestion);
            SaveQuestionCommand = new DelegateCommand(SaveQuestion);
            IsTextQuestion = !string.IsNullOrEmpty(question.ImageUrl);
            IsImageQuestion = !IsTextQuestion;
            AddAnswerCommand = new DelegateCommand(AddAnswer);
            if (IsImageQuestion)
            {
               Image = _imageService.GetMedia(question.ImageUrl);
            }
            RandomizeAnswers = question.AnswersSortedRandomly;
            QuestionText = question.Text;
            Answers = new ObservableCollection<Answer>(question.Answers);
        }
        private void AddImage()
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png;*.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;

            if (choofdlog.ShowDialog() == true)
            {
                var imagePath = _imageService.AddImage(choofdlog.FileName, TestGuid == Guid.Empty ? Test.Id : TestGuid);
                Question.ImageUrl = imagePath;
                try
                {
                    Image = _imageService.GetMedia(imagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AddAnotherQuestion()
        {
            ShowIsCorrectAnswer = true;
            if (Question.Answers.Count > 0)
            {
                Question.Text = string.IsNullOrEmpty(Question.ImageUrl) ? QuestionText : "Image question";
                CreateTestViewModel.Test.Questions.Add(Question);
                CreateTestViewModel.ObservableQuestions.Add(Question);
                Question = new Question();
                Answers = new ObservableCollection<Answer>();
                IsImageQuestion = false;
                Image = null;
                QuestionText = string.Empty;

            }
            else
            {
                MessageBox.Show("You didnt create any answers");
            }
        }
        private void AddAnswer() {
            if (IsEditMode)
            {
                ShowIsCorrectAnswer = true;
                TextVisible = false;
            }
            if (IsCorrect)
            {
                Answer.Text = AnswerText;

                Question.Answers.Add(Answer);
                Question.CorrectAnswerIndex = Question.Answers.IndexOf(Answer);
              
                Answers.Add(Answer);
                Answer = new Answer();
                AnswerText = string.Empty;
                ShowIsCorrectAnswer = false;
                IsCorrect = false;


            }
            else
            {
                Answer.Text = AnswerText;
                Question.Answers.Add(Answer);
                Answers.Add(Answer);
                Answer = new Answer();
                AnswerText = string.Empty;
               
            }
        }
        private void SaveQuestion()
        {
            if (Question != null)
            {
                Question.Text = string.IsNullOrEmpty(Question.ImageUrl) ? QuestionText : "Image question";
                CreateTestViewModel.Test.Questions.Add(Question);
                CreateTestViewModel.ObservableQuestions.Add(Question);
                MessageBox.Show("Questions added");
                CreateTestViewModel.IsVisible = true;
                CreateTestViewModel.QuestionViewModel = null;

            }
            else
            {
                if (CreateTestViewModel.Test.Questions.Count == 0)
                {
                    MessageBox.Show("Your question does not have answers");
                }
                else
                {
                    if (IsImageQuestion)
                    {
                        Question.Text = "Image Question";
                        CreateTestViewModel.Test.Questions.Add(Question);
                        CreateTestViewModel.ObservableQuestions.Add(Question);
                        CreateTestViewModel.IsVisible = true;
                        CreateTestViewModel.QuestionViewModel = null;
                    }
                    else
                    {
                        Question.Text = QuestionText;
                        CreateTestViewModel.Test.Questions.Add(Question);
                        CreateTestViewModel.ObservableQuestions.Add(Question);
                        CreateTestViewModel.IsVisible = true;
                        CreateTestViewModel.QuestionViewModel = null;
                    }
                
                }

            }
        }
       
    }
}
