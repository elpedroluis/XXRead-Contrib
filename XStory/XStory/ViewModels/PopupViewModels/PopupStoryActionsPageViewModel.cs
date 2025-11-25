using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using XStory.DTO;
using XStory.Logger;

namespace XStory.ViewModels.PopupViewModels
{
	public class PopupStoryActionsPageViewModel : Common.BaseStoryViewModel
	{
		#region --- Fields ---
		BL.Common.Contracts.IServiceStory _serviceStory;
		#endregion

		#region --- Commands ---
		public DelegateCommand ClosePopupCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public PopupStoryActionsPageViewModel(INavigationService navigationService,
			BL.Common.Contracts.IServiceStory serviceStory) : base(navigationService, serviceStory)
		{
			_serviceStory = serviceStory;

			ClosePopupCommand = new DelegateCommand(ExecuteClosePopupCommand);

			this.InitStory();
		}
		#endregion

		private void InitStory()
		{
			Story = _serviceStory.GetCurrentStory();
		}

		private async void ExecuteClosePopupCommand()
		{
			await NavigationService.GoBackAsync();
		}
	}
}
