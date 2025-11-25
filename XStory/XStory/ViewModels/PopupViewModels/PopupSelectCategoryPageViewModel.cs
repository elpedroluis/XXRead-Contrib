using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using XStory.DTO;
using XStory.Helpers;

namespace XStory.ViewModels.PopupViewModels
{
	public class PopupSelectCategoryPageViewModel : BasePopupViewModel
	{
		private BL.Common.Contracts.IServiceCategory _serviceCategory;

		#region --- Fields ---
		private Category _selectedCategory;
		private bool _resetCategories;

		private ObservableCollection<DTO.Category> _categories;
		public ObservableCollection<DTO.Category> Categories
		{
			get { return _categories; }
			set { SetProperty(ref _categories, value); }
		}
		#endregion

		public DelegateCommand<DTO.Category> CategoriesItemTappedCommand { get; set; }
		public DelegateCommand ResetCategoriesCommand { get; set; }

		public PopupSelectCategoryPageViewModel(INavigationService navigationService,
			BL.Common.Contracts.IServiceCategory serviceCategory) : base(navigationService)
		{
			_serviceCategory = serviceCategory;

			ClosePopupCommand = new DelegateCommand(ExecuteClosePopupCommand);
			CategoriesItemTappedCommand = new DelegateCommand<DTO.Category>((category) => ExecuteCategoriesItemTappedCommand(category));
			ResetCategoriesCommand = new DelegateCommand(ExecuteResetCategoriesCommand);

			this.InitCategories();
		}

		private async void InitCategories()
		{
			Categories = new ObservableCollection<Category>(await _serviceCategory.GetCategoriesDB(false));
		}

		private void ExecuteCategoriesItemTappedCommand(Category category)
		{
			if (category != null)
			{
				_serviceCategory.SetCurrentCategory(category);
			}

			ClosePopupCommand.Execute();
		}

		private void ExecuteResetCategoriesCommand()
		{
			_serviceCategory.SetCurrentCategory(null);

			ClosePopupCommand.Execute();
		}
	}
}
