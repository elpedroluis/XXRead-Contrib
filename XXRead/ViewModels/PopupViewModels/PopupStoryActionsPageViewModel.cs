using CommunityToolkit.Mvvm.Input;
using XStory.DTO;
using XStory.Logger;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels.PopupViewModels
{
	public class PopupStoryActionsPageViewModel : Common.BaseStoryViewModel
	{
		#region --- Fields ---
		XStory.BL.Common.Contracts.IServiceStory _serviceStory;
		#endregion

		#region --- Commands ---
		public RelayCommand ClosePopupCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public PopupStoryActionsPageViewModel(Prism.Navigation.INavigationService navigationService,
			XStory.BL.Common.Contracts.IServiceStory serviceStory) : base(navigationService, serviceStory)
		{
			_serviceStory = serviceStory;

			ClosePopupCommand = new RelayCommand(ExecuteClosePopupCommand);

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
