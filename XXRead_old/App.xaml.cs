namespace XXRead
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage() { Title = "Je suis un kebab" }) { };
        }
    }
}