
using CommunityToolkit.Maui.Behaviors;
using XXRead.Helpers;

namespace XXRead
{
    public partial class App : Application
    {
        public App()
        //public App(IServiceProvider serviceProvider)
        {
            // ↓↓↓ Pour récup une instance de view model depuis le container de dependency injection
            // var x = serviceProvider.GetService<ViewModels.MainPageViewModel>();

            InitializeComponent();

            this.InitLabelStoryContent();

            //((NavigationPage)Application.Current.Windows[0].Page).BarBackgroundColor = Color.FromArgb(AppSettings.ThemeMain);
            //Application.Current.Windows[0].Page.Behaviors.Add(new StatusBarBehavior() { StatusBarColor = Color.FromArgb(AppSettings.ThemeMain), ApplyOn = StatusBarApplyOn.OnBehaviorAttachedTo });
        }

        private void InitLabelStoryContent()
        {
            Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping("", (handler, view) =>
            {
                if (view is Helpers.CustomControls.LabelStoryContent)
                {
#if ANDROID
                    handler.PlatformView.JustificationMode = Android.Text.JustificationMode.InterWord;
                    handler.PlatformView.SetTextIsSelectable(true);
                    // JE M'AMUSE
                    var mainColor = Android.Graphics.Color.ParseColor(AppSettings.ThemeMain);
                    handler.PlatformView.SetHighlightColor(Android.Graphics.Color.Argb(128, mainColor.R, mainColor.G, mainColor.B));
                    // JE M'AMUSE
#elif IOS || MACCATALYST
					//handler.PlatformView.EditingDidBegin += (s, e) =>
					//{
					//    handler.PlatformView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
					//};
#elif WINDOWS
					handler.PlatformView.GotFocus += (s, e) =>
					{
						//handler.PlatformView.TextAlignment = Windows.UI.Xaml.TextAlignment.Justify;
					};
#endif
                }
            });
        }
    }
}
