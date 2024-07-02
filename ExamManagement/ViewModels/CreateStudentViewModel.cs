
using EasyTestMaker.Models;
using EasyTestMaker.Services;
using HandyControl.Controls;
using Prism.Commands;
using System.Windows.Input;

namespace EasyTestMaker.ViewModels
{
	public class CreateStudentViewModel : ViewModelBase
    {
		private string _username;

		public string Username
		{
			get { return _username; }
			set=> SetProperty(ref _username ,value);

		}
		private string _secret;

		public string Secret
		{
			get { return _secret; }
			set=> SetProperty(ref _secret, value);
		}

		public ICommand Create { get;}
        private readonly IAuthService _authService = App.GetService<IAuthService>();	
        public CreateStudentViewModel()
		{
			Create = new DelegateCommand(CreateUser);
		}

        private async void CreateUser()
        {
            if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Secret))
			{
				MessageBox.Show("Please fill in all fields");
            }
			else
			{
				User user = new User(Username, Secret);
				bool success = await _authService.CreateUser(user);
				if (success)
				{
                    MessageBox.Show("Successfully created user");
					this.Username = string.Empty;
					this.Secret = string.Empty;
                }
				else
				{
                    MessageBox.Show("Something went wrong");
                }
			}
        }
    }
}
