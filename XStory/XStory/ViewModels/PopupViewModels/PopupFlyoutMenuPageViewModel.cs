using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using XStory.Helpers;

namespace XStory.ViewModels.PopupViewModels
{
	public class PopupFlyoutMenuPageViewModel : BaseViewModel
	{
		#region --- Fields ---
		BL.Common.Contracts.IServiceConfig _serviceConfig;

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
		public DelegateCommand SettingsPageCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public PopupFlyoutMenuPageViewModel(INavigationService navigationService, BL.Common.Contracts.IServiceConfig serviceConfig) : base(navigationService)
		{
			_serviceConfig = serviceConfig;

			SettingsPageCommand = new DelegateCommand(ExecuteSettingsPageCommand);

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
			double deviceWidth = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
			double pixelDensity = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;

			double trueWidth = Math.Round(deviceWidth / pixelDensity, 0);
			double sidePadding = trueWidth * 0.3;

			this.PopupPadding = new Thickness(sidePadding, 0, 0, 0);
		}
	}
}
