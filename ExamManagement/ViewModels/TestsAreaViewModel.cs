using EasyTestMaker.PubSub;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTestMaker.ViewModels
{
    public class TestsAreaViewModel : ViewModelBase, IDisposable
    {
		private ViewModelBase _currentViewModel;

		public ViewModelBase CurrentViewModel
		{
			get { return _currentViewModel; }
			set => SetProperty(ref _currentViewModel, value);
		}

		private IEventAggregator @event = App.GetService<IEventAggregator>();

		public TestsAreaViewModel()
		{
			CurrentViewModel = new TestListViewModel();
			@event.GetEvent<EditTestEvent>().Subscribe(Change);
		}

        private void Change(object obj)
        {
			if(obj is CreateTestViewModel model)
            CurrentViewModel = model;
        }

        public void Dispose()
        {
            @event.GetEvent<EditTestEvent>().Unsubscribe(Change);
        }
    }
}
