using CommunityToolkit.Mvvm.Messaging;

namespace XXRead.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupSelectCategoryPage : CommunityToolkit.Maui.Views.Popup
    {
        public PopupSelectCategoryPage(ViewModels.PopupViewModels.PopupSelectCategoryPageViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = viewModel;

            this.RegisterCloseMessage();

            this.ResultWhenUserTapsOutsideOfPopup = new object();
        }

        private void RegisterCloseMessage()
        {
            CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default
                .Register<PopupSelectCategoryPage, Helpers.Messaging.ClosePopupMessage, string>(
                this,
                "ClosePopup",
                async (recipient, message) =>
                {
                    await recipient.Dispatcher.DispatchAsync(
                        async () =>
                        {
                            await recipient.CloseAsync(message.Value);
                        });
                });
        }

        protected override Task OnClosed(object? result, bool wasDismissedByTappingOutsideOfPopup, CancellationToken token = default)
        {
            CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default
                                .Unregister<Helpers.Messaging.ClosePopupMessage, string>(this, "ClosePopup");

            return base.OnClosed(result, wasDismissedByTappingOutsideOfPopup, token);
        }
    }
}