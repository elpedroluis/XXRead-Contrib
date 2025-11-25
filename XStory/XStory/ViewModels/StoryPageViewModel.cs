using Prism.Commands;
using Prism.Navigation;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using XStory.DTO;
using XStory.Helpers;
using XStory.Logger;

namespace XStory.ViewModels
{
	public class StoryPageViewModel : Common.BaseStoryViewModel
	{
		#region --- Fields ---

		private BL.Common.Contracts.IServiceStory _serviceStory;
		private BL.Common.Contracts.IServiceAuthor _serviceAuthor;

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
		public DelegateCommand AuthorTappedCommand { get; set; }
		public DelegateCommand<string> ChapterArrowTapped { get; set; }
		public DelegateCommand ChapterNameTappedCommand { get; set; }
		public DelegateCommand DisplayStoryInfoCommand { get; set; }
		public DelegateCommand OpenStoryActionsCommand { get; set; }
		public DelegateCommand ToggleStoryInfosCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public StoryPageViewModel(INavigationService navigationService,
			BL.Common.Contracts.IServiceStory serviceStory,
			BL.Common.Contracts.IServiceAuthor serviceAuthor)
			: base(navigationService, serviceStory)
		{
			_serviceStory = serviceStory;
			_serviceAuthor = serviceAuthor;

			AppearingCommand = new DelegateCommand(ExecuteAppearingCommand);
			AuthorTappedCommand = new DelegateCommand(ExecuteAuthorTappedCommand);
			ChapterArrowTapped = new DelegateCommand<string>((direction) => ExecuteChapterArrowTappedCommand(direction));
			ChapterNameTappedCommand = new DelegateCommand(ExecuteChapterNameTappedCommand);
			DisplayStoryInfoCommand = new DelegateCommand(ExecuteDisplayStoryInfoCommand);
			OpenStoryActionsCommand = new DelegateCommand(ExecuteOpenStoryActionsCommand);
			ToggleStoryInfosCommand = new DelegateCommand(ExecuteToggleStoryInfosCommand);
			TryAgainCommand = new DelegateCommand(InitStory);

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

				if (NavigationService.GetNavigationUriPath().Contains(nameof(Views.AuthorPage)))
				{
					// if AuthorPage exists : same Author so, go back to AuthorPage
					await NavigationService.GoBackAsync();
				}

				_storyKeep = Story;
				await NavigationService.NavigateAsync(nameof(Views.AuthorPage));
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
				Logger.ServiceLog.Error(ex);
			}
		}

		private async void ExecuteChapterNameTappedCommand()
		{
			if (Story != null && Story.ChaptersList != null && Story.ChaptersList.Count > 0)
			{
				_serviceStory.SetCurrentStory(Story);

				await NavigationService.NavigateAsync(nameof(Views.Popup.PopupChaptersPage));
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

				var currentStory = _serviceStory.GetCurrentStory();
				if (currentStory == null)
				{
					throw new Exception("Story must not be null.");
				}

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
				Logger.ServiceLog.Error(ex);
				ViewState = Helpers.ViewStateEnum.Error;
			}
		}

		private async void ExecuteOpenStoryActionsCommand()
		{
			await NavigationService.NavigateAsync(nameof(Views.Popup.PopupStoryActionsPage));
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

		public override void OnNavigatedTo(INavigationParameters parameters)
		{
			if (_storyKeep != null)
			{
				return;
			}
			else if (Story != null && Story != _serviceStory.GetCurrentStory())
			{
				this.InitStory();
			}
		}
	}
}
