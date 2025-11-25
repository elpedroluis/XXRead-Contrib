using System.Collections.ObjectModel;
using XStory.DTO;
using XXRead.Helpers;
using XStory.Logger;
using CommunityToolkit.Mvvm.Input;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels.PopupViewModels
{
	public class PopupHiddenCategoriesPageViewModel : BasePopupViewModel
	{
		#region --- Fields ---

		private XStory.BL.Common.Contracts.IServiceCategory _serviceCategory;

		private ObservableCollection<Category> _categories;
		public ObservableCollection<Category> Categories
		{
			get { return _categories; }
			set { SetProperty(ref _categories, value); }
		}

		public RelayCommand<Category> CategoriesItemTappedCommand { get; set; }
		#endregion

		#region --- Ctor ---

		public PopupHiddenCategoriesPageViewModel(Prism.Navigation.INavigationService navigationService,
			XStory.BL.Common.Contracts.IServiceCategory serviceCategory) : base(navigationService)
		{
			_serviceCategory = serviceCategory;

			ClosePopupCommand = new RelayCommand(ExecuteClosePopupCommand);
			CategoriesItemTappedCommand = new RelayCommand<Category>((category) => ExecuteCategoriesItemTappedCommand(category));

			this.BuildCategories();
		}
		#endregion

		private async void BuildCategories()
		{
			try
			{
				Categories = new ObservableCollection<Category>(await _serviceCategory.GetCategoriesDB(true));
			}
			catch (Exception ex)
			{
				Categories = null;
				ServiceLog.Error(ex);
			}
		}

		private async void ExecuteCategoriesItemTappedCommand(Category category)
		{
			bool baseState = category.IsEnabled;

			category.IsEnabled = baseState ? false : true;
			int result = await _serviceCategory.Save(category);

			var currentCategory = _serviceCategory.GetCurrentCategory();
			if (currentCategory?.Equals(category) ?? false && !category.IsEnabled)
			{
				_serviceCategory.SetCurrentCategory(null);
			}

			if (result >= 0)
			{
				//List<Category> categoriesUpdated = Categories.OrderBy(c => c.Title).ToList();

				//Categories.Clear();
				Categories = new ObservableCollection<Category>(Categories);

				await _serviceCategory.InitHiddenCategories();
				AppSettings.HiddenCategoriesChanged = true;
			}
			else
			{
				// rollback value
				category.IsEnabled = baseState;
			}
		}

		public override void ExecuteClosePopupCommand()
		{
			CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default.Send<Helpers.Messaging.ClosePopupMessage, string>(
				new Helpers.Messaging.ClosePopupMessage(0), "ClosePopup");
		}
	}
}
