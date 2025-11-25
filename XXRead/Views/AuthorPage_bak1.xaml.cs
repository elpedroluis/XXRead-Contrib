namespace XXRead.Views
{
	public partial class AuthorPage_bak1 : ContentPage
	{
		public AuthorPage_bak1(ViewModels.AuthorPageViewModel viewModel)
		{
			InitializeComponent();

			this.BindingContext = viewModel;
		}
	}
}