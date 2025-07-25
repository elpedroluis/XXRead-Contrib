using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using XStory.DTO;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels
{
	public class StoryInfoPageViewModel : BaseViewModel
	{
		#region --- Fields ---
		private Story _story;
		public Story Story
		{
			get { return _story; }
			set { SetProperty(ref _story, value); }
		}

		private bool _isChapterListVisible;
		public bool IsChapterListVisible
		{
			get { return _isChapterListVisible; }
			set { SetProperty(ref _isChapterListVisible, value); }
		}
		#endregion

		#region --- Commands ---
		public RelayCommand<string> ChapterSelectionCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public StoryInfoPageViewModel(Prism.Navigation.INavigationService navigationService) : base(navigationService)
		{
			IsChapterListVisible = true;

			ChapterSelectionCommand = new RelayCommand<string>((url) => ExecuteChapterSelectionCommand(url));
		} 
		#endregion

		private async void ExecuteChapterSelectionCommand(string url)
		{
			var navigationParams = new NavigationParameters()
			{
				{ "storyUrl", url }
			};

			await NavigationService.NavigateAsync(nameof(Views.StoryPage), navigationParams);
		}

		/*public override void OnNavigatedTo(INavigationParameters parameters)
		{
			try
			{
				if (parameters.TryGetValue<Story>("story", out Story story))
				{
					Story = story;
					if (Story != null)
					{
						Title = Story.Title;

						if (Story.ChaptersList.Count == 0)
						{
							IsChapterListVisible = false;
						}
					}
				}
			}
			catch (Exception e)
			{
				Story = null;
			}

		}*/
	}
}
