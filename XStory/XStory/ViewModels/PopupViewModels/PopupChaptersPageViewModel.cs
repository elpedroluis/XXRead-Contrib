using Prism.Commands;
using Prism.Navigation;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using XStory.DTO;
using XStory.Helpers;

namespace XStory.ViewModels.PopupViewModels
{
	public class PopupChaptersPageViewModel : BasePopupViewModel
	{
		#region --- Fields ---
		private BL.Common.Contracts.IServiceStory _serviceStory;

		private Story _selectedChapter;

		private string _storyTitle;
		public string StoryTitle
		{
			get { return _storyTitle; }
			set { SetProperty(ref _storyTitle, value); }
		}

		private ObservableCollection<DTO.Story> _chapters;
		public ObservableCollection<DTO.Story> Chapters
		{
			get { return _chapters; }
			set { SetProperty(ref _chapters, value); }
		}

		public DelegateCommand<DTO.Story> ChapterTappedCommand { get; set; }
		#endregion

		public PopupChaptersPageViewModel(INavigationService navigationService, 
			BL.Common.Contracts.IServiceStory serviceStory) : base(navigationService)
		{
			_serviceStory = serviceStory;

			ClosePopupCommand = new DelegateCommand(ExecuteClosePopupCommand);
			ChapterTappedCommand = new DelegateCommand<DTO.Story>((chapter) => ExecuteChapterTappedCommand(chapter));

			this.InitChaptersList();
		}

		private void ExecuteChapterTappedCommand(Story chapter)
		{
			_selectedChapter = chapter;

			this.ClosePopupCommand.Execute();
		}

		public override async void ExecuteClosePopupCommand()
		{
			if (_selectedChapter == null)
			{
				await NavigationService.GoBackAsync();
				return;
			}

			_serviceStory.SetCurrentStory(_selectedChapter);

			// If last nav Page is not StoryPage (so, usually AuthorPage)
			if (PrismApplication.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(Views.StoryPage))
			{
				// go to StoryPage
				await NavigationService.NavigateAsync(nameof(Views.StoryPage));
			}

			// else : Back to StoryPage
			await NavigationService.GoBackAsync();
		}

		private void InitChaptersList()
		{
			try
			{
				Story story = _serviceStory.GetCurrentStory();

				if (story != null && story.ChaptersList != null)
				{
					StoryTitle = story.Title;
					Chapters = new ObservableCollection<DTO.Story>(story.ChaptersList);
				}
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				Chapters = null;
			}
		}
	}
}
