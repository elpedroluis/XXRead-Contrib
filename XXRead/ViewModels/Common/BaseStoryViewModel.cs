using CommunityToolkit.Mvvm.Input;
using XStory.Logger;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels.Common
{
	public class BaseStoryViewModel : BaseViewModel
	{
		#region --- Fields ---
		XStory.BL.Common.Contracts.IServiceStory _serviceStory;

		private string _saveButtonText;
		public string SaveButtonText
		{
			get { return _saveButtonText; }
			set { SetProperty(ref _saveButtonText, value); }
		}

		private XStory.DTO.Story _story;
		public XStory.DTO.Story Story
		{
			get { return _story; }
			set { SetProperty(ref _story, value); }
		}
		#endregion

		#region --- Commands ---
		public RelayCommand SaveStoryCommand { get; set; }
		public RelayCommand ShareStoryCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public BaseStoryViewModel(Prism.Navigation.INavigationService navigationService,
			XStory.BL.Common.Contracts.IServiceStory serviceStory) : base(navigationService)
		{
			_serviceStory = serviceStory;

			SaveStoryCommand = new RelayCommand(ExecuteSaveStoryCommand);
			ShareStoryCommand = new RelayCommand(ExecuteShareStoryCommand);


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
				XStory.Logger.ServiceLog.Error(ex);
			}
		}
	}
}
