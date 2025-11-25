using CommunityToolkit.Mvvm.Input;
using XXRead.Helpers;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels.PopupViewModels
{
	public class PopupFlyoutMenuPageViewModel : BaseViewModel
	{
		#region --- Fields ---
		XStory.BL.Common.Contracts.IServiceConfig _serviceConfig;

		private Thickness _popupPadding;
		public Thickness PopupPadding
		{
			get { return _popupPadding; }
			set { SetProperty(ref _popupPadding, value); }
		}

		private DataSourceItem _currentDataSource;
		public DataSourceItem CurrentDataSource
		{
			get { return _currentDataSource; }
			set { SetProperty(ref _currentDataSource, value); }
		}
		#endregion

		#region --- Commands ---
		public RelayCommand SettingsPageCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public PopupFlyoutMenuPageViewModel(Prism.Navigation.INavigationService navigationService, 
			XStory.BL.Common.Contracts.IServiceConfig serviceConfig) : base(navigationService)
		{
			_serviceConfig = serviceConfig;

			SettingsPageCommand = new RelayCommand(ExecuteSettingsPageCommand);

			this.InitDataSourceInfo();
			this.InitPopupSize();
		}
		#endregion

		private async void ExecuteSettingsPageCommand()
		{
			await NavigationService.NavigateAsync(nameof(Views.SettingsPage));
		}

		/// <summary>
		/// Inits the ContentView containing Image + Name of CurrentDataSource.
		/// </summary>
		private void InitDataSourceInfo()
		{
			var dataSource = _serviceConfig.GetCurrentDataSource().ToString();
			CurrentDataSource = new DataSourceItem()
			{
				Name = dataSource,
				Image = string.Concat(dataSource.ToLower(), "_icon")
			};
		}

		/// <summary>
		/// Sets Popup's padding.
		/// 
		/// TODO : test on bigger screens
		/// </summary>
		private void InitPopupSize()
		{

			double deviceWidth = DeviceDisplay.Current.MainDisplayInfo.Width;
			double pixelDensity = DeviceDisplay.Current.MainDisplayInfo.Density;

			double trueWidth = Math.Round(deviceWidth / pixelDensity, 0);
			double sidePadding = trueWidth * 0.3;

			this.PopupPadding = new Thickness(sidePadding, 0, 0, 0);
		}
	}
}
