namespace XXRead.Views
{
	public partial class AuthorPage_bakRecent : ContentPage
	{
		public AuthorPage_bakRecent(ViewModels.AuthorPageViewModel viewModel)
		{
			InitializeComponent();

			this.BindingContext = viewModel;
		}
	}
}