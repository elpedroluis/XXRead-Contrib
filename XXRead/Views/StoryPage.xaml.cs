namespace XXRead.Views
{
	public partial class StoryPage : ContentPage
	{
		public StoryPage(ViewModels.StoryPageViewModel vm)
		{
			InitializeComponent();

			this.BindingContext = vm;
		}
	}
}
