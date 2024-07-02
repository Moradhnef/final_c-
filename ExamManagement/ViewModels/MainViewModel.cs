using EasyTestMaker.Constants;
using EasyTestMaker.PubSub;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;

namespace EasyTestMaker.ViewModels
{
	public class MainViewModel : ViewModelBase, IDisposable
    {
		private ViewModelBase _currentViewModel;

		public ViewModelBase CurrentViewModel
		{
			get { return _currentViewModel; }
			set => SetProperty(ref _currentViewModel, value);

		}

		private ViewModelBase _rightViewModel;

		public ViewModelBase RightViewModel

		{
			get { return _rightViewModel; }
			set => SetProperty(ref _rightViewModel, value);
		}

		private ViewModelBase _leftViewModel;

		public ViewModelBase LeftViewModel
		{
			get { return _leftViewModel; }
			set => SetProperty(ref _leftViewModel, value);
		}

		private readonly IEventAggregator @event = App.GetService<IEventAggregator>();

		public ICommand Loaded { get; private set; }
		public MainViewModel()
		{

			InitializeModel();

        }
	    private void InitializeModel()
		{
			@event.GetEvent<ViewChangeEvent>().Subscribe(ViewChange);
            Loaded = new DelegateCommand(OnLoaded);
			@event.GetEvent<LogoutEvent>().Subscribe(Logout);
            CurrentViewModel = new LoginViewModel();
        }

        private void Change(StudentMenuViewSelection obj)
        {
            throw new NotImplementedException();
        }

        private void Logout()
        {
            CurrentViewModel = new LoginViewModel();
			Const.Username = string.Empty;
			Const.UserId = 0;
        }

        private void ViewChange(object obj)
        {
            if(obj is LoginViewModel login)
			{
				CurrentViewModel = new SplashScreenViewModel(login);
			}
			if(obj is bool success)
			{
				CurrentViewModel = success ? new HomeViewModel() : new LoginViewModel();
			}
		
        }

        private void OnLoaded()
        {
          InitializeModel();
        }

        public void Dispose()
        {
            @event.GetEvent<ViewChangeEvent>().Unsubscribe(ViewChange);
            @event.GetEvent<LogoutEvent>().Unsubscribe(Logout);
        }
    }
}
