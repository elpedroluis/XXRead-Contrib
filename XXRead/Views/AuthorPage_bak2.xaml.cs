namespace XXRead.Views
{
	public partial class AuthorPage_bak2 : ContentPage
	{
		public AuthorPage_bak2(ViewModels.AuthorPageViewModel viewModel)
		{
			InitializeComponent();

			this.BindingContext = viewModel;
		}
	}
}