using Microsoft.AspNetCore.Components.WebView.Maui;
//using CommunityToolkit.Maui;
using XXRead.Data;

namespace XXRead
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				//.UseMauiCommunityToolkit()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();
#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
#endif
			builder.RegisterServices();

			builder.Services.AddSingleton<WeatherForecastService>();

			return builder.Build();
		}

		public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
		{
			mauiAppBuilder.Services.AddTransient<XStory.BL.Web.DSLocator.Contracts.IServiceStory, XStory.BL.Web.DSLocator.ServiceStory>();
			mauiAppBuilder.Services.AddTransient<XStory.BL.Web.DSLocator.Contracts.IServiceCategory, XStory.BL.Web.DSLocator.ServiceCategory>();

			mauiAppBuilder.Services.AddTransient<XStory.BL.Web.XStory.Contracts.IServiceCategory, XStory.BL.Web.XStory.ServiceCategory>();
			mauiAppBuilder.Services.AddTransient<XStory.BL.Web.XStory.Contracts.IServiceStory, XStory.BL.Web.XStory.ServiceStory>();

			mauiAppBuilder.Services.AddTransient<XStory.BL.Web.HDS.Contracts.IServiceCategory, XStory.BL.Web.HDS.ServiceCategory>();
			mauiAppBuilder.Services.AddTransient<XStory.BL.Web.HDS.Contracts.IServiceStory, XStory.BL.Web.HDS.ServiceStory>();

			mauiAppBuilder.Services.AddTransient<XStory.BL.Web.Demo.Contracts.IServiceStory, XStory.BL.Web.Demo.ServiceStory>();

			mauiAppBuilder.Services.AddTransient<XStory.BL.SQLite.Contracts.IServiceCategory, XStory.BL.SQLite.ServiceCategory>();
			mauiAppBuilder.Services.AddTransient<XStory.BL.SQLite.Contracts.IServiceSettings, XStory.BL.SQLite.ServiceSettings>();


			return mauiAppBuilder;
		}
	}
}