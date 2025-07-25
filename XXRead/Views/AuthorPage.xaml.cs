namespace XXRead.Views
{
	public partial class AuthorPage : TabbedPage
	{
		public AuthorPage(ViewModels.AuthorPageViewModel viewModel)
		{
			InitializeComponent();

			this.BindingContext = viewModel;
		}
	}
}
