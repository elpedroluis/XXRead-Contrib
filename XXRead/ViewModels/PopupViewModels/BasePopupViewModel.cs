using CommunityToolkit.Mvvm.Input;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels.PopupViewModels
{
	public class BasePopupViewModel : BaseViewModel
	{
		public RelayCommand ClosePopupCommand { get; set; }

		public BasePopupViewModel(Prism.Navigation.INavigationService navigationService) : base(navigationService)
		{

		}

		public virtual async void ExecuteClosePopupCommand()
		{
			await NavigationService.GoBackAsync();
		}
	}
}
