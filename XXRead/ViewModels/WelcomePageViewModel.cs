using CommunityToolkit.Mvvm.Input;
using XXRead.Helpers;
using XXRead.Helpers.Constants;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels
{
    public class WelcomePageViewModel : BaseViewModel
    {
        public RelayCommand<string> DataSourceSelectCommand { get; set; }

        private Dictionary<string, string> _dataSourceDictionary = new Dictionary<string, string>()
        {
            { "XStory" , GlobalConstants.STORIES_SOURCE_XSTORY },
            { "Histoires-de-sexe" , GlobalConstants.STORIES_SOURCE_HDS }
        };

        public WelcomePageViewModel(Prism.Navigation.INavigationService navigationService) : base(navigationService)
        {
            Title = Helpers.Constants.WelcomePageConstants.WELCOMEPAGE_TITLE;

            DataSourceSelectCommand = new RelayCommand<string>((param) => ExecuteDataSourceSelectCommand(param));
        }

        private void ExecuteDataSourceSelectCommand(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                throw new Exception();
            }
            else if (_dataSourceDictionary.ContainsKey(param))
            {
                StaticContext.DATASOURCE = _dataSourceDictionary[param];
                AppSettings.DataSource = StaticContext.DATASOURCE;

                NavigationService.NavigateAsync(string.Concat("/NavigationPage/",nameof(Views.MainPage)));
            }
        }
    }
}
