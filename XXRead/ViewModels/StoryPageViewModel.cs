using XStory.DTO;
using XXRead.Helpers;
using XStory.Logger;
using CommunityToolkit.Mvvm.Input;
using XXRead.Helpers.Services;
using CommunityToolkit.Maui.Core;

namespace XXRead.ViewModels
{
    public class StoryPageViewModel : Common.BaseStoryViewModel
    {
        #region --- Fields ---
        private IPopupService _popupService;

        private XStory.BL.Common.Contracts.IServiceStory _serviceStory;
        private XStory.BL.Common.Contracts.IServiceAuthor _serviceAuthor;

        private bool _isStoryInfoVisible;
        public bool IsStoryInfoVisible
        {
            get { return _isStoryInfoVisible; }
            set { SetProperty(ref _isStoryInfoVisible, value); }
        }

        /// <summary>
        /// Story from current StoryPageViewModel. To keep Story from this VM when/if other StoryPageViewModel is instanciated
        /// </summary>
        private Story _storyKeep;
        #endregion

        #region --- Commands ---
        public RelayCommand AuthorTappedCommand { get; set; }
        public RelayCommand<string> ChapterArrowTapped { get; set; }
        public RelayCommand ChapterNameTappedCommand { get; set; }
        public RelayCommand DisplayStoryInfoCommand { get; set; }
        public RelayCommand OpenStoryActionsCommand { get; set; }
        public RelayCommand OpenInBrowserCommand { get; set; }
        public RelayCommand ToggleStoryInfosCommand { get; set; }
        #endregion

        #region --- Ctor ---
        public StoryPageViewModel(INavigationService navigationService,
            IPopupService popupService,
            XStory.BL.Common.Contracts.IServiceStory serviceStory,
            XStory.BL.Common.Contracts.IServiceAuthor serviceAuthor)
            : base(navigationService, serviceStory)
        {
            _popupService = popupService;

            _serviceStory = serviceStory;
            _serviceAuthor = serviceAuthor;

            AppearingCommand = new RelayCommand(ExecuteAppearingCommand);
            AuthorTappedCommand = new RelayCommand(ExecuteAuthorTappedCommand);
            ChapterArrowTapped = new RelayCommand<string>((direction) => ExecuteChapterArrowTappedCommand(direction));
            ChapterNameTappedCommand = new RelayCommand(ExecuteChapterNameTappedCommand);
            DisplayStoryInfoCommand = new RelayCommand(ExecuteDisplayStoryInfoCommand);
            OpenStoryActionsCommand = new RelayCommand(ExecuteOpenStoryActionsCommand);
            OpenInBrowserCommand = new RelayCommand(ExecuteOpenInBrowserCommand);
            ToggleStoryInfosCommand = new RelayCommand(ExecuteToggleStoryInfosCommand);
            TryAgainCommand = new RelayCommand(InitStory);

            this.InitStory();
        }
        #endregion

        /// <summary>
        /// Navigates to Author's page. If already from same Author page, go back.
        /// </summary>
        private async void ExecuteAuthorTappedCommand()
        {
            if (Story.Author != null)
            {
                _serviceAuthor.SetCurrentAuthor(Story.Author);

                // experimental ↓↓↓
                ShellNavigationState state = Shell.Current.CurrentState;
                if (state.Location.ToString().Contains(nameof(Views.AuthorPage)))
                // ↑↑↑ experimental --- ↓↓↓ original ↓↓↓
                //if (NavigationService.GetNavigationUriPath().Contains(nameof(Views.AuthorPage)))
                {
                    // if AuthorPage exists : same Author so, go back to AuthorPage
                    await NavigationService.GoBackAsync();
                }

                _storyKeep = Story;
                //await NavigationService.NavigateAsync(nameof(Views.AuthorPage));
                //await Shell.Current.GoToAsync($"//AuthorPage");
                await Shell.Current.GoToAsync(nameof(Views.AuthorPage));
            }
        }

        private void ExecuteChapterArrowTappedCommand(string direction)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(direction)) throw new ArgumentNullException();

                Story chapter = null;

                switch (direction)
                {
                    case "left":
                        int previousChapter = Story.ChapterNumber - 1;
                        chapter = Story.ChaptersList.ElementAt(previousChapter - 1);
                        break;

                    case "right":
                        int nextChapter = Story.ChapterNumber + 1;
                        chapter = Story.ChaptersList.ElementAt(nextChapter - 1);
                        break;
                }

                if (chapter != null)
                {
                    _serviceStory.SetCurrentStory(chapter);

                    ViewState = Helpers.ViewStateEnum.Loading;

                    this.InitStory();
                }
            }
            catch (Exception ex)
            {
                XStory.Logger.ServiceLog.Error(ex);
            }
        }

        private async void ExecuteChapterNameTappedCommand()
        {
            if (Story != null && Story.ChaptersList != null && Story.ChaptersList.Count > 0)
            {
                _serviceStory.SetCurrentStory(Story);

                Story? selectedChapterFromPopup = await _popupService.ShowPopupAsync<ViewModels.PopupViewModels.PopupChaptersPageViewModel>() as Story;

                if (selectedChapterFromPopup != null)
                {
                    Story = selectedChapterFromPopup;
                    this.InitStory();
                }
            }
        }

        private async void ExecuteDisplayStoryInfoCommand()
        {
            await NavigationService.NavigateAsync(nameof(Views.StoryInfoPage));
        }

        /// <summary>
        /// Gets the Story from either the cache if exists, or from the web if it does not.
        /// And displays it, or error.
        /// </summary>
        private async void InitStory()
        {
            try
            {
                ViewState = ViewStateEnum.Loading;

                var currentStory = _serviceStory.GetCurrentStory()
                    ?? throw new Exception("Story must not be null.");

                var alreadyLoadedStory = _serviceStory.GetAlreadyLoadedStory(currentStory);
                if (alreadyLoadedStory != null)
                {
                    // Get story from cache
                    Story = alreadyLoadedStory;
                }
                else
                {
                    // or from web
                    Story = await _serviceStory.InitStory();
                }

                if (Story == null)
                {
                    throw new Exception("Story must not be null.");
                }

                Title = Story.Title;

                _serviceStory.AddAlreadyLoadedStory(Story);

                ViewState = Helpers.ViewStateEnum.Display;
            }
            catch (Exception ex)
            {
                XStory.Logger.ServiceLog.Error(ex);
                ViewState = Helpers.ViewStateEnum.Error;
            }
        }

        private async void ExecuteOpenStoryActionsCommand()
        {
            await NavigationService.NavigateAsync(nameof(Views.Popup.PopupStoryActionsPage));
        }

        private async void ExecuteOpenInBrowserCommand()
        {
            Uri storyUri = new Uri(Story.Url);

            await Browser.Default.OpenAsync(storyUri, BrowserLaunchMode.External);
        }

        private void ExecuteToggleStoryInfosCommand()
        {
            if (!IsStoryInfoVisible)
            {
                IsStoryInfoVisible = true;
            }
            else
            {
                IsStoryInfoVisible = false;
            }
        }

        /*public override void OnNavigatedTo(INavigationParameters parameters)
		{
			if (_storyKeep != null)
			{
				return;
			}
			else if (Story != null && Story != _serviceStory.GetCurrentStory())
			{
				this.InitStory();
			}
		}*/
    }
}
