using EasyTestMaker.Models;
using EasyTestMaker.PubSub;
using EasyTestMaker.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EasyTestMaker.ViewModels
{
    public class TestListViewModel : ViewModelBase
    {
        private ObservableCollection<Test> _tests;

        public ObservableCollection<Test> Tests
        {
            get { return _tests; }
            set
            {
               SetProperty(ref _tests, value);
            }
        }
        private bool _isEditEnabled;

        public bool IsEditEnabled
        {
            get { return _isEditEnabled; }
            set
            {
               SetProperty(ref _isEditEnabled, value);
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
                    IsEditEnabled = true;
                }
               SetProperty(ref _test, value);
            }
        }

        private readonly ITestService _testService = App.GetService<ITestService>();
        private readonly IEventAggregator @event = App.GetService<IEventAggregator>();
        public ICommand Edit { get;}
        public TestListViewModel()
        {
            Initialize();
            Edit = new DelegateCommand(EditTest);
            
        }

        private void EditTest()
        {
            var model = new CreateTestViewModel(Test);
            @event.GetEvent<EditTestEvent>().Publish(model);
        }

        public void Initialize()
        {
            var tests = Task.Run(() => _testService.GetTestsAsync()).Result;
            Tests = new ObservableCollection<Test>(tests);
        }
       

        
    }
}
