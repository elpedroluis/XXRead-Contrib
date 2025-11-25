using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using XStory.Logger;

namespace XStory.ViewModels.Common
{
	public class BaseStoryViewModel : BaseViewModel
	{
		#region --- Fields ---
		BL.Common.Contracts.IServiceStory _serviceStory;

		private string _saveButtonText;
		public string SaveButtonText
		{
			get { return _saveButtonText; }
			set { SetProperty(ref _saveButtonText, value); }
		}

		private DTO.Story _story;
		public DTO.Story Story
		{
			get { return _story; }
			set { SetProperty(ref _story, value); }
		}
		#endregion

		#region --- Commands ---
		public DelegateCommand SaveStoryCommand { get; set; }
		public DelegateCommand ShareStoryCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public BaseStoryViewModel(INavigationService navigationService,
			BL.Common.Contracts.IServiceStory serviceStory) : base(navigationService)
		{
			_serviceStory = serviceStory;

			SaveStoryCommand = new DelegateCommand(ExecuteSaveStoryCommand);
			ShareStoryCommand = new DelegateCommand(ExecuteShareStoryCommand);


			this.SetSaveButtonText();
		}
		#endregion

		public async void SetSaveButtonText()
		{
			SaveButtonText = await _serviceStory.GetStorySQLite(_serviceStory.GetCurrentStory()) == null
				? Helpers.Constants.StoryPageConstants.STORYPAGE_SAVE // if not in saved in DB
				: Helpers.Constants.StoryPageConstants.STORYPAGE_DELETE;// if saved in DB
		}

		public async void ExecuteSaveStoryCommand()
		{
			try
			{
				int result = await _serviceStory.InsertStorySQLite(Story);

				this.SetSaveButtonText();
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
			}
		}

		public async void ExecuteShareStoryCommand()
		{
			try
			{
				await Share.RequestAsync(new ShareTextRequest()
				{
					Uri = Story.Url,
					Text = Story.Title,
					Title = "Partager ce récit"
				});
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
			}
		}
	}
}
