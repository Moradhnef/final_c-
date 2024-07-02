using EasyTestMaker.PubSub;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace EasyTestMaker.ViewModels
{
	public class SplashScreenViewModel : ViewModelBase, IDisposable
    {
		private int _progress;

		public int Progress
		{
			get { return _progress; }
			set => SetProperty(ref _progress, value);
		}

	
		private IEventAggregator @event = App.GetService<IEventAggregator>();
		public SplashScreenViewModel(object data)
		{
			@event.GetEvent<SplashScreenEvent>().Subscribe(OnChange);
			InitializeSplashScreen(data);
		}
		private void InitializeSplashScreen(object data)
		{
			if(data != null && data is LoginViewModel login)
			{
				Task.Run(() => login.StartLoginProcessAsync(this));
				
			}
		}
        private void OnChange(string value)
        {
        }

        public void Dispose()
        {
            @event.GetEvent<SplashScreenEvent>().Unsubscribe(OnChange);
        }
    }
}
