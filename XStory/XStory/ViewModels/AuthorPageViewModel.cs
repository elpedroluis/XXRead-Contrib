using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Prism.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XStory.DTO;
using XStory.Helpers;
using XStory.Logger;

namespace XStory.ViewModels
{
	public class AuthorPageViewModel : BaseViewModel
	{
		#region --- Fields ---

		private BL.Common.Contracts.IServiceAuthor _serviceAuthor;
		private BL.Common.Contracts.IServiceStory _serviceStory;

		private bool _canLoadMorePages;
		public bool CanLoadMorePages
		{
			get { return _canLoadMorePages; }
			set { SetProperty(ref _canLoadMorePages, value); }
		}

		private Author _author;
		public Author Author
		{
			get { return _author; }
			set { SetProperty(ref _author, value); }
		}

		private ObservableCollection<Story> _authorStories;

		public ObservableCollection<Story> AuthorStories
		{
			get { return _authorStories; }
			set { SetProperty(ref _authorStories, value); }
		}

		public DelegateCommand<DTO.Story> AuthorStoryItemTappedCommand { get; set; }
		public DelegateCommand LoadMoreStoriesCommand { get; set; }

		#endregion

		#region --- Ctor ---
		public AuthorPageViewModel(INavigationService navigationService,
			BL.Common.Contracts.IServiceAuthor serviceAuthor,
			BL.Common.Contracts.IServiceStory serviceStory) : base(navigationService)
		{
			_serviceAuthor = serviceAuthor;
			_serviceStory = serviceStory;



			AuthorStoryItemTappedCommand = new DelegateCommand<DTO.Story>((story) => ExecuteAuthorStoryItemTappedCommand(story));
			LoadMoreStoriesCommand = new DelegateCommand(ExecuteLoadMoreStoriesCommand);

			this.InitAuthor();
		}
		#endregion

		private async void ExecuteAuthorStoryItemTappedCommand(Story story)
		{
			_serviceStory.SetCurrentStory(story);

			if (story.ChaptersList != null && story.ChaptersList.Count > 0)
			{
				// if multi sub chapters
				await NavigationService.NavigateAsync(nameof(Views.Popup.PopupChaptersPage));
			}
			else
			{
				// only one chapter
				await NavigationService.NavigateAsync(nameof(Views.StoryPage));
			}
		}

		private async void ExecuteLoadMoreStoriesCommand()
		{
			// Get next page stories
			_author = await _serviceAuthor.GetAuthorStoriesNextPage(_author);

			// Display / hide button "Load more stories"
			CanLoadMorePages = _author.HasMorePages;

			AuthorStories = new ObservableCollection<DTO.Story>(_author.Stories);
		}

		private async void InitAuthor()
		{
			try
			{
				ViewState = ViewStateEnum.Loading;

				DTO.Author currentAuthor = _serviceAuthor.GetCurrentAuthor();
				if (currentAuthor == null)
				{
					throw new Exception("Author must not be null.");
				}

				Title = currentAuthor.Name;

				var alreadyLoadedAuthor = _serviceAuthor.GetAlreadyLoadedAuthor(currentAuthor);
				if (alreadyLoadedAuthor != null)
				{
					Author = alreadyLoadedAuthor;
				}
				else
				{
					Author = await _serviceAuthor.InitAuthor();
				}

				if (Author == null)
				{
					throw new Exception("Story must not be null.");
				}

				_serviceAuthor.AddAlreadyLoadedAuthor(Author);

				AuthorStories = new ObservableCollection<Story>(_author.Stories);

				CanLoadMorePages = _author.HasMorePages;

				ViewState = ViewStateEnum.Display;
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				ViewState = Helpers.ViewStateEnum.Error;
			}
		}

		public override void OnNavigatedTo(INavigationParameters parameters)
		{
			if (Author != null && Author != _serviceAuthor.GetCurrentAuthor())
			{
				this.InitAuthor();
			}
		}
	}
}
