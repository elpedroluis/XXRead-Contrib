using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using XStory.DTO;
using XXRead.Helpers;
using XXRead.Helpers.Constants;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        #region --- Fields ---
        private XStory.BL.Common.Contracts.IServiceStory _serviceStory;
        private XStory.BL.Common.Contracts.IServiceCategory _serviceCategory;
        private XStory.BL.Common.Contracts.IServiceConfig _serviceConfig;

        private IPopupService _popupService;

        private ObservableCollection<Story> _stories;

        public ObservableCollection<Story> Stories
        {
            get { return _stories; }
            set { SetProperty(ref _stories, value); }
        }

        private bool _isStoriesListRefreshing;
        public bool IsStoriesListRefreshing
        {
            get { return _isStoriesListRefreshing; }
            set { SetProperty(ref _isStoriesListRefreshing, value); }
        }

        private Category _currentCategory;
        public Category CurrentCategory
        {
            get { return _currentCategory; }
            set { SetProperty(ref _currentCategory, value); }
        }

        #endregion

        #region --- Commands ---
        public RelayCommand CategoryTappedCommand { get; set; }
        public RelayCommand GetDbStoriesCommand { get; set; }
        public RelayCommand LoadMoreStoriesCommand { get; set; }
        public RelayCommand<Story> StoriesItemTappedCommand { get; set; }
        public RelayCommand StoriesRefreshCommand { get; set; }
        public RelayCommand SettingsCommand { get; set; }
        #endregion

        #region --- Ctor ---
        public MainPageViewModel(INavigationService navigationService,
            IPopupService popupService,
            XStory.BL.Common.Contracts.IServiceStory serviceStory,
            XStory.BL.Common.Contracts.IServiceCategory serviceCategory,
            XStory.BL.Common.Contracts.IServiceConfig serviceConfig) : base(navigationService)
        {
            _serviceStory = serviceStory;
            _serviceCategory = serviceCategory;
            _serviceConfig = serviceConfig;

            _popupService = popupService;

            Title = MainPageConstants.MAINPAGE_TITLE;
            ViewState = ViewStateEnum.Loading;

            AppearingCommand = new RelayCommand(ExecuteAppearingCommand);
            CategoryTappedCommand = new RelayCommand(ExecuteCategoryTappedCommand);
            GetDbStoriesCommand = new RelayCommand(ExecuteGetDbStoriesCommand);
            LoadMoreStoriesCommand = new RelayCommand(ExecuteLoadMoreStoriesCommand);
            SettingsCommand = new RelayCommand(ExecuteSettingsCommand);
            StoriesItemTappedCommand = new RelayCommand<Story>((story) => ExecuteStoriesItemTappedCommand(story));
            StoriesRefreshCommand = new RelayCommand(ExecuteStoriesRefreshCommand);
            TryAgainCommand = new RelayCommand(ExecuteTryAgainCommand);

            if (AppSettings.FirstRun)
            {
                // If FIRST run : diclaimer Message "Welcome" + "disabled categories"
                this.DisplayFirstRunMessage();

                AppSettings.FirstRun = false;
            }

            this.InitData();
        }
        #endregion

        private void InitData()
        {
            _serviceConfig.SetCurrentDataSource((XStory.DTO.Config.DataSources)Enum.Parse(typeof(XStory.DTO.Config.DataSources), AppSettings.DataSource));

            Task.Run(this.InitCategories).Wait();
            Task.Run(this.InitHiddenCategories).Wait();
            Task.Run(() => this.InitStories()).Wait();
        }

        [Obsolete("Deprecated. (to improvise - with Task.ContinueWith()) - Use InitData instead")]
        private void InitDataOld()
        {
            this.InitCategories().ContinueWith(result =>
                {
                    if (result.Status == TaskStatus.RanToCompletion)
                    {
                        this.InitHiddenCategories().ContinueWith(res =>
                        {
                            if (res.Status == TaskStatus.RanToCompletion)
                            {
                                this.InitStories();
                            }
                        });
                    }
                });
        }

        private void DisplayFirstRunMessage()
        {
            // SUPER UGLY !
            // SUPER UGLY !
            // What about a beautiful welcome page :)
            // SUPER UGLY !
            // SUPER UGLY !

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.DisplayAlert(
                    MainPageConstants.MAINPAGE_FIRST_RUN_TITLE,
                    MainPageConstants.MAINPAGE_FIRST_RUN_MESSAGE,
                    "OK");
            });
        }

        /// <summary>
        /// <br>First appearing.</br>
        /// <br>Init Color themes.</br>
        /// </summary>
        protected override void ExecuteAppearingCommand()
        {
            // Have to call InitTheming() everytime VM appears because of this stupid Android BackButton issue
            this.InitTheming();

            if (AppSettings.DataSourceChanged)
            {
                Stories = null;
                this.InitData();
            }
        }

        /// <summary>
        /// Display Categories selection popup
        /// </summary>
        private async void ExecuteCategoryTappedCommand()
        {
            _currentCategory = _serviceCategory.GetCurrentCategory();

            var result = await _popupService.ShowPopupAsync<ViewModels.PopupViewModels.PopupSelectCategoryPageViewModel>();
            if (result?.GetType() == typeof(object))
            {
                return;
            }
            XStory.DTO.Category selectedCategory = result as XStory.DTO.Category;

            if (selectedCategory != _currentCategory)
            {
                CurrentCategory = selectedCategory;
                _serviceCategory.SetCurrentCategory(selectedCategory);
                Stories = new ObservableCollection<Story>(await _serviceStory.InitStories());
            }
        }

        private async void ExecuteSettingsCommand()
        {
            //await _popupService.ShowPopupAsync<ViewModels.PopupViewModels.PopupFlyoutMenuPageViewModel>();
            await NavigationService.NavigateAsync(nameof(Views.SettingsPage));
        }

        private async void ExecuteStoriesItemTappedCommand(Story story)
        {
            if (story != null)
            {
                _serviceStory.SetCurrentStory(story);
            }

            await NavigationService.NavigateAsync(nameof(Views.StoryPage));
        }

        /// <summary>
        /// When the PullToRefresh command is triggered on StoriesList.
        /// If the 1st item of the two lists is different : refresh. 
        /// Else : nothing
        /// </summary>
        private async void ExecuteStoriesRefreshCommand()
        {
            try
            {
                if (Stories == null || Stories.Count == 0)
                {
                    Stories = new ObservableCollection<Story>(await _serviceStory.InitStories());
                    return;
                    // OU Exception ?
                }
                IsStoriesListRefreshing = true;

                List<Story> refreshList = await _serviceStory.RefreshStories(Stories.First());
                if (refreshList == null)
                {
                    throw new Exception("Couldn't refresh stories");
                }
                else if (refreshList.Count > 0)
                {
                    Stories = new ObservableCollection<Story>(refreshList);
                }

                IsStoriesListRefreshing = false;
            }
            catch (Exception ex)
            {
                XStory.Logger.ServiceLog.Error(ex);
                IsStoriesListRefreshing = false;
            }
        }

        private async void ExecuteLoadMoreStoriesCommand()
        {
            var moreStories = await _serviceStory.LoadMoreStories();

            moreStories.ForEach(story => _stories.Add(story));

            Stories = new ObservableCollection<Story>(_serviceStory.DistinctStories(_stories.ToList()));
        }

        /// <summary>
        /// Get stories on Main Page.
        /// </summary>
        /// <param name="forceInit">Force (re)init if true.</param>
        private async void InitStories(bool forceInit = false)
        {
            if (AppSettings.HiddenCategoriesChanged)
            {
                forceInit = true;
                _serviceStory.ResetPageNumber();
                CurrentCategory = _serviceCategory.GetCurrentCategory();
            }

            if (forceInit || (Stories == null || Stories.Count == 0))
            {
                ViewState = ViewStateEnum.Loading;
                try
                {
                    Stories = new ObservableCollection<Story>(await _serviceStory.InitStories());
                    ViewState = ViewStateEnum.Display;
                }
                catch (Exception ex)
                {
                    XStory.Logger.ServiceLog.Error(ex);
                    ViewState = ViewStateEnum.Error;
                }
                AppSettings.DataSourceChanged = false;
                AppSettings.HiddenCategoriesChanged = false;
            }
        }

        private async void ExecuteGetDbStoriesCommand()
        {
            if (Title == MainPageConstants.MAINPAGE_TITLE)
            {
                var storiess = await _serviceStory.GetStoriesSQLite();
                Stories = new ObservableCollection<Story>(storiess);
                Title = MainPageConstants.MAINPAGE_TITLE_SAVED;
            }
            else
            {
                this.InitStories(true);
                Title = MainPageConstants.MAINPAGE_TITLE;
            }
        }

        /// <summary>
        /// Get Categories from web and insert it in the database.
        /// </summary>
        private async Task InitCategories()
        {
            try
            {
                await _serviceCategory.InitCategories();
            }
            catch (Exception ex)
            {
                XStory.Logger.ServiceLog.Error(ex);
            }
        }

        private async Task InitHiddenCategories()
        {
            await _serviceCategory.InitHiddenCategories();
        }

        protected override void ExecuteTryAgainCommand()
        {
            this.InitStories();
        }

        private void OnCurrentCategoryChanged()
        {
            _serviceStory.ResetPageNumber();

            this.InitStories(true);
        }

        /*public override void OnNavigatedTo(INavigationParameters parameters)
		{
			if (AppSettings.DataSourceChanged)
			{
				_serviceCategory.SetCurrentCategory(null);
			}
			else if (_serviceCategory.HasCategorySelectionChanged(CurrentCategory))
			{
				CurrentCategory = _serviceCategory.GetCurrentCategory();

				this.InitStories(true);
			}
		}*/
    }
}
