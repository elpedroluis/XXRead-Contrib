
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Primitives;

namespace XXRead.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupDataSourceSelectionPage : CommunityToolkit.Maui.Views.Popup
    {
        public PopupDataSourceSelectionPage(ViewModels.PopupViewModels.PopupDataSourceSelectionPageViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = viewModel;

            this.RegisterCloseMessage();
        }

        private void RegisterCloseMessage()
        {
            CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default
                .Register<PopupDataSourceSelectionPage, Helpers.Messaging.ClosePopupMessage, string>(
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