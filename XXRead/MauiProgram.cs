using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Prism;
using XXRead.Helpers;
using XXRead.Helpers.Services;

namespace XXRead
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UsePrism(prism =>
                {
                    prism.RegisterTypes(container =>
                    {
                        container.RegisterForNavigation<Views.MyNavigationPage>();

                        container.RegisterForNavigation<Views.AuthorPage, ViewModels.AuthorPageViewModel>();
                        container.RegisterForNavigation<Views.MainPage, ViewModels.MainPageViewModel>();
                        container.RegisterForNavigation<Views.SettingsPage, ViewModels.SettingsPageViewModel>();
                        container.RegisterForNavigation<Views.Settings.SettingsAppearancePage, ViewModels.Settings.SettingsAppearancePageViewModel>();
                        container.RegisterForNavigation<Views.StoryInfoPage, ViewModels.StoryInfoPageViewModel>();
                        container.RegisterForNavigation<Views.StoryPage, ViewModels.StoryPageViewModel>();
                        container.RegisterForNavigation<Views.WelcomePage, ViewModels.WelcomePageViewModel>();

                        /* --- BL --- */
                        container.Register<XStory.BL.Common.Contracts.IServiceStory, XStory.BL.Common.ServiceStory>();
                        container.Register<XStory.BL.Common.Contracts.IServiceCategory, XStory.BL.Common.ServiceCategory>();
                        container.Register<XStory.BL.Common.Contracts.IServiceAuthor, XStory.BL.Common.ServiceAuthor>();
                        container.Register<XStory.BL.Common.Contracts.IServiceConfig, XStory.BL.Common.ServiceConfig>();

                        container.Register<XStory.BL.Web.DSLocator.Contracts.IServiceStory, XStory.BL.Web.DSLocator.ServiceStory>();
                        container.Register<XStory.BL.Web.DSLocator.Contracts.IServiceCategory, XStory.BL.Web.DSLocator.ServiceCategory>();
                        container.Register<XStory.BL.Web.DSLocator.Contracts.IServiceAuthor, XStory.BL.Web.DSLocator.ServiceAuthor>();

                        container.Register<XStory.BL.Web.XStory.Contracts.IServiceCategory, XStory.BL.Web.XStory.ServiceCategory>();
                        container.Register<XStory.BL.Web.XStory.Contracts.IServiceStory, XStory.BL.Web.XStory.ServiceStory>();
                        container.Register<XStory.BL.Web.XStory.Contracts.IServiceAuthor, XStory.BL.Web.XStory.ServiceAuthor>();

                        container.Register<XStory.BL.Web.HDS.Contracts.IServiceCategory, XStory.BL.Web.HDS.ServiceCategory>();
                        container.Register<XStory.BL.Web.HDS.Contracts.IServiceStory, XStory.BL.Web.HDS.ServiceStory>();
                        container.Register<XStory.BL.Web.HDS.Contracts.IServiceAuthor, XStory.BL.Web.HDS.ServiceAuthor>();

                        container.Register<XStory.BL.Web.Demo.Contracts.IServiceStory, XStory.BL.Web.Demo.ServiceStory>();
                        container.Register<XStory.BL.Web.Demo.Contracts.IServiceAuthor, XStory.BL.Web.Demo.ServiceAuthor>();

                        container.Register<XStory.BL.SQLite.Contracts.IServiceCategory, XStory.BL.SQLite.ServiceCategory>();
                        container.Register<XStory.BL.SQLite.Contracts.IServiceSettings, XStory.BL.SQLite.ServiceSettings>();
                        container.Register<XStory.BL.SQLite.Contracts.IServiceStory, XStory.BL.SQLite.ServiceStory>();
                        container.Register<XStory.BL.SQLite.Contracts.IServiceAuthor, XStory.BL.SQLite.ServiceAuthor>();


                        /* --- DAL --- */

                        container.Register<XStory.DAL.Web.HDS.Contracts.IRepositoryWebHDS, XStory.DAL.Web.HDS.RepositoryWebHDS>();

                        container.Register<XStory.DAL.SQLite.Contracts.IRepositoryStory, XStory.DAL.SQLite.RepositoryStory>();
                        container.Register<XStory.DAL.SQLite.Contracts.IRepositoryCategory, XStory.DAL.SQLite.RepositoryCategory>();
                        container.Register<XStory.DAL.SQLite.Contracts.IRepositoryAuthor, XStory.DAL.SQLite.RepositoryAuthor>();
                        container.Register<XStory.DAL.SQLite.Contracts.IRepositoryAuthorStory, XStory.DAL.SQLite.RepositoryAuthorStory>();

                    });

                    prism.CreateWindow(async (containerProvider, navigationService) =>
                    {
                        if (AppSettings.FirstRun)
                            await navigationService.NavigateAsync("/MyNavigationPage/WelcomePage");
                        else
                            await navigationService.NavigateAsync("/MyNavigationPage/MainPage");
                    });
                })
                .UseMauiCommunityToolkit()
                .RegisterViews()
                .RegisterViewModels()
                .RegisterPopups()
                .RegisterWebServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                //.ConfigureMauiHandlers(handlers =>
                //{
                //	handlers.AddHandler(typeof(Helpers.CustomControls.LabelJustify), typeof(Helpers.Handlers.LabelJustifyHandler));
                //})
                ;

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazorWebViewDeveloperTools();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            /* UI Services*/
            // builder.Services.AddSingleton<Prism.Navigation.INavigationService, NavigationService>();

            /* Database */
            builder.Services.AddSingleton<XStory.DAL.SQLite.Contracts.IXXReadDatabase, XStory.DAL.SQLite.XXReadDatabase>();

            return builder.Build();
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<Views.AuthorPage>();
            builder.Services.AddTransient<Views.AuthorPage>();
            builder.Services.AddTransient<Views.MainPage>();
            builder.Services.AddTransient<Views.SettingsPage>();
            builder.Services.AddTransient<Views.StoryInfoPage>();
            builder.Services.AddTransient<Views.StoryPage>();
            builder.Services.AddTransient<Views.WelcomePage>();

            //Register all routes for Shell (/!\ all except MainPage /!\)
            Routing.RegisterRoute(nameof(Views.AuthorPage), typeof(Views.AuthorPage));
            Routing.RegisterRoute(nameof(Views.SettingsPage), typeof(Views.SettingsPage));
            Routing.RegisterRoute(nameof(Views.StoryInfoPage), typeof(Views.StoryInfoPage));
            Routing.RegisterRoute(nameof(Views.StoryPage), typeof(Views.StoryPage));
            Routing.RegisterRoute(nameof(Views.WelcomePage), typeof(Views.WelcomePage));
            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<ViewModels.AuthorPageViewModel>();
            builder.Services.AddTransient<ViewModels.AuthorPageWebViewModel>();
            builder.Services.AddTransient<ViewModels.MainPageViewModel>();
            builder.Services.AddTransient<ViewModels.StoryInfoPageViewModel>();
            builder.Services.AddTransient<ViewModels.StoryPageViewModel>();
            builder.Services.AddTransient<ViewModels.SettingsPageViewModel>();
            builder.Services.AddTransient<ViewModels.Settings.SettingsAppearancePageViewModel>();
            builder.Services.AddTransient<ViewModels.WelcomePageViewModel>();
            return builder;
        }

        public static MauiAppBuilder RegisterPopups(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<ViewModels.PopupViewModels.BasePopupViewModel>();

            builder.Services.AddTransientPopup<Views.Popup.PopupChaptersPage, ViewModels.PopupViewModels.PopupChaptersPageViewModel>();
            builder.Services.AddTransientPopup<Views.Popup.PopupDataSourceSelectionPage, ViewModels.PopupViewModels.PopupDataSourceSelectionPageViewModel>();
            builder.Services.AddTransientPopup<Views.Popup.PopupFlyoutMenuPage, ViewModels.PopupViewModels.PopupFlyoutMenuPageViewModel>();
            builder.Services.AddTransientPopup<Views.Popup.PopupHiddenCategoriesPage, ViewModels.PopupViewModels.PopupHiddenCategoriesPageViewModel>();
            builder.Services.AddTransientPopup<Views.Popup.PopupSelectCategoryPage, ViewModels.PopupViewModels.PopupSelectCategoryPageViewModel>();
            builder.Services.AddTransientPopup<Views.Popup.PopupStoryActionsPage, ViewModels.PopupViewModels.PopupStoryActionsPageViewModel>();

            return builder;
        }

        public static MauiAppBuilder RegisterWebServices(this MauiAppBuilder builder)
        {
            /* --- BL --- */
            builder.Services.AddTransient<XStory.BL.Common.Contracts.IServiceStory, XStory.BL.Common.ServiceStory>();
            builder.Services.AddTransient<XStory.BL.Common.Contracts.IServiceCategory, XStory.BL.Common.ServiceCategory>();
            builder.Services.AddTransient<XStory.BL.Common.Contracts.IServiceAuthor, XStory.BL.Common.ServiceAuthor>();
            builder.Services.AddTransient<XStory.BL.Common.Contracts.IServiceConfig, XStory.BL.Common.ServiceConfig>();

            builder.Services.AddTransient<XStory.BL.Web.DSLocator.Contracts.IServiceStory, XStory.BL.Web.DSLocator.ServiceStory>();
            builder.Services.AddTransient<XStory.BL.Web.DSLocator.Contracts.IServiceCategory, XStory.BL.Web.DSLocator.ServiceCategory>();
            builder.Services.AddTransient<XStory.BL.Web.DSLocator.Contracts.IServiceAuthor, XStory.BL.Web.DSLocator.ServiceAuthor>();

            builder.Services.AddTransient<XStory.BL.Web.XStory.Contracts.IServiceCategory, XStory.BL.Web.XStory.ServiceCategory>();
            builder.Services.AddTransient<XStory.BL.Web.XStory.Contracts.IServiceStory, XStory.BL.Web.XStory.ServiceStory>();
            builder.Services.AddTransient<XStory.BL.Web.XStory.Contracts.IServiceAuthor, XStory.BL.Web.XStory.ServiceAuthor>();

            builder.Services.AddTransient<XStory.BL.Web.HDS.Contracts.IServiceCategory, XStory.BL.Web.HDS.ServiceCategory>();
            builder.Services.AddTransient<XStory.BL.Web.HDS.Contracts.IServiceStory, XStory.BL.Web.HDS.ServiceStory>();
            builder.Services.AddTransient<XStory.BL.Web.HDS.Contracts.IServiceAuthor, XStory.BL.Web.HDS.ServiceAuthor>();

            builder.Services.AddTransient<XStory.BL.Web.Demo.Contracts.IServiceStory, XStory.BL.Web.Demo.ServiceStory>();
            builder.Services.AddTransient<XStory.BL.Web.Demo.Contracts.IServiceAuthor, XStory.BL.Web.Demo.ServiceAuthor>();

            builder.Services.AddTransient<XStory.BL.SQLite.Contracts.IServiceCategory, XStory.BL.SQLite.ServiceCategory>();
            builder.Services.AddTransient<XStory.BL.SQLite.Contracts.IServiceSettings, XStory.BL.SQLite.ServiceSettings>();
            builder.Services.AddTransient<XStory.BL.SQLite.Contracts.IServiceStory, XStory.BL.SQLite.ServiceStory>();
            builder.Services.AddTransient<XStory.BL.SQLite.Contracts.IServiceAuthor, XStory.BL.SQLite.ServiceAuthor>();


            /* --- DAL --- */

            builder.Services.AddTransient<XStory.DAL.Web.HDS.Contracts.IRepositoryWebHDS, XStory.DAL.Web.HDS.RepositoryWebHDS>();

            builder.Services.AddTransient<XStory.DAL.SQLite.Contracts.IRepositoryStory, XStory.DAL.SQLite.RepositoryStory>();
            builder.Services.AddTransient<XStory.DAL.SQLite.Contracts.IRepositoryCategory, XStory.DAL.SQLite.RepositoryCategory>();
            builder.Services.AddTransient<XStory.DAL.SQLite.Contracts.IRepositoryAuthor, XStory.DAL.SQLite.RepositoryAuthor>();
            builder.Services.AddTransient<XStory.DAL.SQLite.Contracts.IRepositoryAuthorStory, XStory.DAL.SQLite.RepositoryAuthorStory>();

            return builder;
        }
    }
}
