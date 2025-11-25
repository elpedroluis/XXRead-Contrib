using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace XStory.ViewModels.PopupViewModels
{
	public class BasePopupViewModel : BaseViewModel
	{
		public DelegateCommand ClosePopupCommand { get; set; }

		public BasePopupViewModel(INavigationService navigationService) : base(navigationService)
		{

		}

		public virtual async void ExecuteClosePopupCommand()
		{
			await NavigationService.GoBackAsync();
		}
	}
}
