using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XStory.DTO;
using XStory.Helpers;
using XStory.Helpers.Constants;

namespace XStory.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
		#region --- Fields ---

		private IPageDialogService _pageDialogService;

		private BL.Common.Contracts.IServiceStory _serviceStory;
		private BL.Common.Contracts.IServiceCategory _serviceCategory;
		private BL.Common.Contracts.IServiceConfig _serviceConfig;

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
		public DelegateCommand CategoryTappedCommand { get; set; }
		public DelegateCommand GetDbStoriesCommand { get; set; }
		public DelegateCommand LoadMoreStoriesCommand { get; set; }
		public DelegateCommand<DTO.Story> StoriesItemTappedCommand { get; set; }
		public DelegateCommand StoriesRefreshCommand { get; set; }
		public DelegateCommand SettingsCommand { get; set; }
		#endregion

		#region --- Ctor ---
		public MainPageViewModel(INavigationService navigationService,
			IPageDialogService pageDialogService,
			BL.Common.Contracts.IServiceStory serviceStory,
			BL.Common.Contracts.IServiceCategory serviceCategory,
			BL.Common.Contracts.IServiceConfig serviceConfig)
			: base(navigationService)
		{
			_pageDialogService = pageDialogService;

			_serviceStory = serviceStory;
			_serviceCategory = serviceCategory;
			_serviceConfig = serviceConfig;

			Title = MainPageConstants.MAINPAGE_TITLE;
			ViewState = ViewStateEnum.Loading;

			AppearingCommand = new DelegateCommand(ExecuteAppearingCommand);
			CategoryTappedCommand = new DelegateCommand(ExecuteCategoryTappedCommand);
			GetDbStoriesCommand = new DelegateCommand(ExecuteGetDbStoriesCommand);
			LoadMoreStoriesCommand = new DelegateCommand(ExecuteLoadMoreStoriesCommand);
			SettingsCommand = new DelegateCommand(ExecuteSettingsCommand);
			StoriesItemTappedCommand = new DelegateCommand<DTO.Story>((story) => ExecuteStoriesItemTappedCommand(story));
			StoriesRefreshCommand = new DelegateCommand(ExecuteStoriesRefreshCommand);
			TryAgainCommand = new DelegateCommand(ExecuteTryAgainCommand);

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
			_serviceConfig.SetCurrentDataSource((DTO.Config.DataSources)Enum.Parse(typeof(DTO.Config.DataSources), AppSettings.DataSource));

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
			Device.BeginInvokeOnMainThread(async () =>
			{
				await _pageDialogService.DisplayAlertAsync(
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

			this.InitStories();
		}

		/// <summary>
		/// Display Categories selection popup
		/// </summary>
		private async void ExecuteCategoryTappedCommand()
		{

			await NavigationService.NavigateAsync(nameof(Views.Popup.PopupSelectCategoryPage));
		}

		private async void ExecuteSettingsCommand()
		{
			await NavigationService.NavigateAsync(nameof(Views.Popup.PopupFlyoutMenuPage));
		}

		private async void ExecuteStoriesItemTappedCommand(DTO.Story story)
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

				List<DTO.Story> refreshList = await _serviceStory.RefreshStories(Stories.First());
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
				Logger.ServiceLog.Error(ex);
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
			if (AppSettings.DataSourceChanged || AppSettings.HiddenCategoriesChanged)
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
					Logger.ServiceLog.Error(ex);
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
				Logger.ServiceLog.Error(ex);
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

		public override void OnNavigatedTo(INavigationParameters parameters)
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
		}
	}
}
