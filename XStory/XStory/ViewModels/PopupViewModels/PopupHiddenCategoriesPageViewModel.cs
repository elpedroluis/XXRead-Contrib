using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using XStory.DTO;
using XStory.Helpers;
using XStory.Logger;

namespace XStory.ViewModels.PopupViewModels
{
	public class PopupHiddenCategoriesPageViewModel : BasePopupViewModel
	{
		#region --- Fields ---

		private BL.Common.Contracts.IServiceCategory _serviceCategory;

		private ObservableCollection<DTO.Category> _categories;
		public ObservableCollection<DTO.Category> Categories
		{
			get { return _categories; }
			set { SetProperty(ref _categories, value); }
		}

		public DelegateCommand<DTO.Category> CategoriesItemTappedCommand { get; set; }
		#endregion

		public PopupHiddenCategoriesPageViewModel(INavigationService navigationService,
			BL.Common.Contracts.IServiceCategory serviceCategory) : base(navigationService)
		{
			_serviceCategory = serviceCategory;

			ClosePopupCommand = new DelegateCommand(ExecuteClosePopupCommand);
			CategoriesItemTappedCommand = new DelegateCommand<DTO.Category>((category) => ExecuteCategoriesItemTappedCommand(category));

			this.BuildCategories();
		}

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

			if (!category.IsEnabled)
			{
				_serviceCategory.SetCurrentCategory(null);
			}

			if (result >= 0)
			{
				List<Category> categoriesUpdated = Categories.OrderBy(c => c.Title).ToList();

				Categories.Clear();
				Categories = new ObservableCollection<Category>(categoriesUpdated);

				await _serviceCategory.InitHiddenCategories();
				AppSettings.HiddenCategoriesChanged = true;
			}
			else
			{
				// rollback value
				category.IsEnabled = baseState;
			}
		}
	}
}
