using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using XStory.DTO;
using XXRead.Helpers.Services;

namespace XXRead.ViewModels.PopupViewModels
{
    public class PopupSelectCategoryPageViewModel : BasePopupViewModel
    {
        private XStory.BL.Common.Contracts.IServiceCategory _serviceCategory;

        #region --- Fields ---
        private Category _currentCategory;
        private Category _selectedCategory;
        private bool _resetCategories;

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }
        #endregion

        public RelayCommand<Category> CategoriesItemTappedCommand { get; set; }
        public RelayCommand ResetCategoriesCommand { get; set; }

        public PopupSelectCategoryPageViewModel(Prism.Navigation.INavigationService navigationService,
            XStory.BL.Common.Contracts.IServiceCategory serviceCategory) : base(navigationService)
        {
            _serviceCategory = serviceCategory;

            ClosePopupCommand = new RelayCommand(ExecuteClosePopupCommand);
            CategoriesItemTappedCommand = new RelayCommand<Category>((category) => ExecuteCategoriesItemTappedCommand(category));
            ResetCategoriesCommand = new RelayCommand(ExecuteResetCategoriesCommand);

            this.InitCategories();
            _currentCategory = _serviceCategory.GetCurrentCategory();
        }

        private async void InitCategories()
        {
            var categs = await _serviceCategory.GetCategoriesDB(false);
            Categories = new ObservableCollection<Category>(categs);
        }

        private void ExecuteCategoriesItemTappedCommand(Category category)
        {
            if (category != null)
            {
                RequestClose(category);
            }
        }

        private void ExecuteResetCategoriesCommand()
        {
            RequestClose(null);
        }

        public override void ExecuteClosePopupCommand()
        {
            RequestClose(_currentCategory);
        }

        private void RequestClose(Category category = null)
        {
            CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default.Send<Helpers.Messaging.ClosePopupMessage, string>(
                new Helpers.Messaging.ClosePopupMessage(category), "ClosePopup");
        }
    }
}
