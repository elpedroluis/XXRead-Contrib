namespace XXRead.Views
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage(ViewModels.SettingsPageViewModel vm)
		{
			InitializeComponent();

			this.BindingContext = vm;
		}
	}
}
