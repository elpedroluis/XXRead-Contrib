using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using XStory.ViewModels;
using XStory.ViewModels.Settings;
using XStory.Views;
using XStory.Views.Settings;


namespace XStory
{
	public partial class App
	{
		public App(IPlatformInitializer initializer)
			: base(initializer)
		{
			AppCenter.Start("android=ae4c4a06-c715-4593-bbef-993e55d59c64;", typeof(Analytics), typeof(Crashes));
		}

		protected override async void OnInitialized()
		{
			InitializeComponent();

			await NavigationService.NavigateAsync("NavigationPage/MainPage");

			((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex(Helpers.AppSettings.ThemeMain);
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			/* App Init */
			containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

			/* Database */
			containerRegistry.RegisterSingleton<DAL.SQLite.Contracts.IXXReadDatabase, DAL.SQLite.XXReadDatabase>();

			/* UI Services*/
			containerRegistry.RegisterPopupNavigationService();
			containerRegistry.RegisterPopupDialogService();

			/* Pages */
			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
			containerRegistry.RegisterForNavigation<StoryPage, StoryPageViewModel>();
			containerRegistry.RegisterForNavigation<AuthorPage, AuthorPageViewModel>();
			containerRegistry.RegisterForNavigation<StoryInfoPage, StoryInfoPageViewModel>();
			containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
			//containerRegistry.RegisterForNavigation<SettingsPage, SettingsPage3ViewModel>();
			containerRegistry.RegisterForNavigation<SettingsAppearancePage, SettingsAppearancePageViewModel>();
			containerRegistry.RegisterForNavigation<WelcomePage, WelcomePageViewModel>();

			/* Popup pages */
			containerRegistry.RegisterForNavigation<Views.Popup.PopupFlyoutMenuPage, ViewModels.PopupViewModels.PopupFlyoutMenuPageViewModel>();
			containerRegistry.RegisterForNavigation<Views.Popup.PopupSelectCategoryPage, ViewModels.PopupViewModels.PopupSelectCategoryPageViewModel>();
			containerRegistry.RegisterForNavigation<Views.Popup.PopupHiddenCategoriesPage, ViewModels.PopupViewModels.PopupHiddenCategoriesPageViewModel>();
			containerRegistry.RegisterForNavigation<Views.Popup.PopupChaptersPage, ViewModels.PopupViewModels.PopupChaptersPageViewModel>();
			containerRegistry.RegisterForNavigation<Views.Popup.PopupDataSourceSelectionPage, ViewModels.PopupViewModels.PopupDataSourceSelectionPageViewModel>();
			containerRegistry.RegisterForNavigation<Views.Popup.PopupStoryActionsPage, ViewModels.PopupViewModels.PopupStoryActionsPageViewModel>();

			/* Dependency Injection */

			/* --- BL --- */
			containerRegistry.Register<BL.Common.Contracts.IServiceStory, BL.Common.ServiceStory>();
			containerRegistry.Register<BL.Common.Contracts.IServiceCategory, BL.Common.ServiceCategory>();
			containerRegistry.Register<BL.Common.Contracts.IServiceAuthor, BL.Common.ServiceAuthor>();
			containerRegistry.Register<BL.Common.Contracts.IServiceConfig, BL.Common.ServiceConfig>();

			containerRegistry.Register<BL.Web.DSLocator.Contracts.IServiceStory, BL.Web.DSLocator.ServiceStory>();
			containerRegistry.Register<BL.Web.DSLocator.Contracts.IServiceCategory, BL.Web.DSLocator.ServiceCategory>();
			containerRegistry.Register<BL.Web.DSLocator.Contracts.IServiceAuthor, BL.Web.DSLocator.ServiceAuthor>();

			containerRegistry.Register<BL.Web.XStory.Contracts.IServiceCategory, BL.Web.XStory.ServiceCategory>();
			containerRegistry.Register<BL.Web.XStory.Contracts.IServiceStory, BL.Web.XStory.ServiceStory>();
			containerRegistry.Register<BL.Web.XStory.Contracts.IServiceAuthor, BL.Web.XStory.ServiceAuthor>();

			containerRegistry.Register<BL.Web.HDS.Contracts.IServiceCategory, BL.Web.HDS.ServiceCategory>();
			containerRegistry.Register<BL.Web.HDS.Contracts.IServiceStory, BL.Web.HDS.ServiceStory>();
			containerRegistry.Register<BL.Web.HDS.Contracts.IServiceAuthor, BL.Web.HDS.ServiceAuthor>();

			containerRegistry.Register<BL.Web.Demo.Contracts.IServiceStory, BL.Web.Demo.ServiceStory>();
			containerRegistry.Register<BL.Web.Demo.Contracts.IServiceAuthor, BL.Web.Demo.ServiceAuthor>();

			containerRegistry.Register<BL.SQLite.Contracts.IServiceCategory, BL.SQLite.ServiceCategory>();
			containerRegistry.Register<BL.SQLite.Contracts.IServiceSettings, BL.SQLite.ServiceSettings>();
			containerRegistry.Register<BL.SQLite.Contracts.IServiceStory, BL.SQLite.ServiceStory>();
			containerRegistry.Register<BL.SQLite.Contracts.IServiceAuthor, BL.SQLite.ServiceAuthor>();

			/* --- DAL --- */
			containerRegistry.Register<DAL.Web.HDS.Contracts.IRepositoryWebHDS, DAL.Web.HDS.RepositoryWebHDS>();

			containerRegistry.Register<DAL.SQLite.Contracts.IRepositoryStory, DAL.SQLite.RepositoryStory>();
			containerRegistry.Register<DAL.SQLite.Contracts.IRepositoryCategory, DAL.SQLite.RepositoryCategory>();
			containerRegistry.Register<DAL.SQLite.Contracts.IRepositoryAuthor, DAL.SQLite.RepositoryAuthor>();
			containerRegistry.Register<DAL.SQLite.Contracts.IRepositoryAuthorStory, DAL.SQLite.RepositoryAuthorStory>();
		}
	}
}
