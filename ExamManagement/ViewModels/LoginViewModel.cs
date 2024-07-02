using EasyTestMaker.Constants;
using EasyTestMaker.Models;
using EasyTestMaker.PubSub;
using EasyTestMaker.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EasyTestMaker.ViewModels
{
    public class LoginViewModel : ViewModelBase, IDisposable
    {

        private string _userName;

        public string Username
        {
            get { return _userName; }
            set => SetProperty(ref _userName, value);
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set => SetProperty(ref _password, value);
        }

     


        public ICommand Login { get; private set;}
        public ICommand Loaded { get; private set;}

        private bool _isVIsible;

        public bool IsVisible
        {
            get { return _isVIsible; }
            set => SetProperty(ref _isVIsible, value);
        }
        private bool _isInfoVisible;

        public bool IsInfoVisible
        {
            get { return _isInfoVisible; }
            set=> SetProperty(ref _isInfoVisible, value);
        }

        private string _infoMessage;

        public string InfoMessage
        {
            get { return _infoMessage; }
            set => this.SetProperty(ref _infoMessage, value);
        }



        private IAuthService _auth => App.GetService<IAuthService>();
        private IEventAggregator @event = App.GetService<IEventAggregator>();
        public LoginViewModel()
        {
            InitializeCommands();
            InitializeModel();

        }

        private void InitializeCommands()
        {
            Login = new DelegateCommand(StartLogin);
            Loaded = new DelegateCommand(OnLoad);

        }

        private void StartLogin()
        {
            ChangeView(this);
        }

        private void OnLoad()
        {
           
        }

        private void InitializeModel()
        {
            IsVisible = true;
        }
        public async Task<bool> StartLoginProcessAsync(SplashScreenViewModel model)
        {

            var output = await OnLogin(model);
            return output;
        }
        private async Task<bool> OnLogin(SplashScreenViewModel model)
        {
            model.Progress = 20;
            User user = new User(Username, Password);
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
               throw new ArgumentException("Username or password cannot be empty");
            }
             model.Progress = 40;
            bool success = await _auth.Login(user);
            model.Progress = 80;

            model.Progress = 100;
            Thread.Sleep(1000);

            ChangeView(success);
        
            return success;
        }
        private void ChangeView(object value)
        {
            @event.GetEvent<ViewChangeEvent>().Publish(value);
        }
        public void Dispose()
        {
            
        }
    }
}
