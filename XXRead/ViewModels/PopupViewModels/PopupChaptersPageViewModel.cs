using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using XStory.DTO;
using XXRead.Helpers;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels.PopupViewModels
{
	public class PopupChaptersPageViewModel : BasePopupViewModel
	{
		#region --- Fields ---
		private XStory.BL.Common.Contracts.IServiceStory _serviceStory;

		private Story _selectedChapter;

		private string _storyTitle;
		public string StoryTitle
		{
			get { return _storyTitle; }
			set { SetProperty(ref _storyTitle, value); }
		}

		private ObservableCollection<Story> _chapters;
		public ObservableCollection<Story> Chapters
		{
			get { return _chapters; }
			set { SetProperty(ref _chapters, value); }
		}

		public RelayCommand<Story> ChapterTappedCommand { get; set; }
		#endregion

		public PopupChaptersPageViewModel(Prism.Navigation.INavigationService navigationService,
			XStory.BL.Common.Contracts.IServiceStory serviceStory) : base(navigationService)
		{
			_serviceStory = serviceStory;

			ClosePopupCommand = new RelayCommand(ExecuteClosePopupCommand);
			ChapterTappedCommand = new RelayCommand<Story>((chapter) => ExecuteChapterTappedCommand(chapter));

			this.InitChaptersList();
		}

		private void ExecuteChapterTappedCommand(Story chapter)
		{
			_selectedChapter = chapter;

			this.ClosePopupCommand.Execute(null);
		}

		public override async void ExecuteClosePopupCommand()
		{
			if (_selectedChapter == null)
			{
				RequestClose();
				return;
			}

			_serviceStory.SetCurrentStory(_selectedChapter);

			// If last nav Page is not StoryPage (so, usually AuthorPage)
			//if (PrismApplication.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(Views.StoryPage))
			if (Shell.Current.Navigation.NavigationStack.Last().GetType() != typeof(Views.StoryPage))
			{
				// go to StoryPage
				await NavigationService.NavigateAsync(nameof(Views.StoryPage));
			}

			// else : Close -> back to StoryPage
			RequestClose(_selectedChapter);
		}

		private void InitChaptersList()
		{
			try
			{
				Story story = _serviceStory.GetCurrentStory();

				if (story != null && story.ChaptersList != null)
				{
					StoryTitle = story.Title;
					Chapters = new ObservableCollection<Story>(story.ChaptersList);
				}
			}
			catch (Exception ex)
			{
				XStory.Logger.ServiceLog.Error(ex);
				Chapters = null;
			}
		}

		private void RequestClose(Story story = null)
		{
			CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default.Send<Helpers.Messaging.ClosePopupMessage, string>(
				new Helpers.Messaging.ClosePopupMessage(story), "ClosePopup");
		}
	}
}
