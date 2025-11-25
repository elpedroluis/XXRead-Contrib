using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using XXRead.Helpers;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels
{
	public class BaseViewModel : BindableBase
	{
        protected Prism.Navigation.INavigationService NavigationService { get; private set; }
        public RelayCommand AppearingCommand { get; set; }
		public RelayCommand TryAgainCommand { get; set; }

		// [ObservableProperty]
		private string _title;
		private ViewStateEnum _viewState;

		private Color _themeMain;
		private Color _themePrimary;
		private Color _themeSecondary;
		private Color _themeFontPrimary;
		private Color _themeFontSecondary;

		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		public ViewStateEnum ViewState
		{
			get { return _viewState; }
			set { SetProperty(ref _viewState, value); }
		}

		public Color ThemePrimary
		{
			get { return _themePrimary; }
			set { SetProperty(ref _themePrimary, value); }
		}

		public Color ThemeSecondary
		{
			get { return _themeSecondary; }
			set { SetProperty(ref _themeSecondary, value); }
		}

		public Color ThemeMain
		{
			get { return _themeMain; }
			set { SetProperty(ref _themeMain, value); }
		}

		public Color ThemeFontPrimary
		{
			get { return _themeFontPrimary; }
			set { SetProperty(ref _themeFontPrimary, value); }
		}

		public Color ThemeFontSecondary
		{
			get { return _themeFontSecondary; }
			set { SetProperty(ref _themeFontSecondary, value); }
		}

		public BaseViewModel(Prism.Navigation.INavigationService navigationService)
		{
			NavigationService = navigationService;

			InitTheming();

			InitDataSource();

			ViewState = ViewStateEnum.Display;
		}

		protected void InitTheming()
		{
			ThemeMain = Color.FromArgb(Helpers.AppSettings.ThemeMain);
			ThemePrimary = Color.FromArgb(Helpers.AppSettings.ThemePrimary);
			ThemeSecondary = Color.FromArgb(Helpers.AppSettings.ThemeSecondary);
			ThemeFontPrimary = Color.FromArgb(Helpers.AppSettings.ThemeFontPrimary);
			ThemeFontSecondary = Color.FromArgb(Helpers.AppSettings.ThemeFontSecondary);
		}

		private void InitDataSource()
		{
			StaticContext.DATASOURCE = AppSettings.DataSource;
		}

		protected virtual void ExecuteAppearingCommand() { }

		protected virtual void ExecuteTryAgainCommand() { }

		public virtual void Initialize()
		{

		}

		public virtual void OnNavigatedFrom()
		{

		}

		public virtual void OnNavigatedTo()
		{

		}
	}
}
