#if IOS || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiLabel;
#elif ANDROID
using PlatformView = AndroidX.AppCompat.Widget.AppCompatTextView;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.TextBlock;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif

using Microsoft.Maui.Handlers;
using XXRead.Helpers.CustomControls;

namespace XXRead.Helpers.Handlers
{
//    public partial class LabelJustifyHandler
//    {
//        public static PropertyMapper<LabelJustify, LabelJustifyHandler> PropertyMapper
//            = new PropertyMapper<LabelJustify, LabelJustifyHandler>(ViewHandler.ViewMapper)
//            {
//                [nameof(LabelJustify.JustifyText)] = MapJustifyText
//            };

//        public CustomEntryHandler() : base(PropertyMapper)
//        {
//        }

//        public LabelJustifyHandler()
//        {
//            LabelHandler.Mapper.AppendToMapping("MyCustom", (handler, view) =>
//            {
//                if (view is LabelJustify)
//                {
//#if ANDROID
//                    handler.PlatformView.JustificationMode = Android.Text.JustificationMode.InterWord;
//#elif IOS || MACCATALYST
//                    //handler.PlatformView.EditingDidBegin += (s, e) =>
//                    //{
//                    //    handler.PlatformView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
//                    //};
//#elif WINDOWS
//                    //handler.PlatformView.GotFocus += (s, e) =>
//                    //{
//                    //	handler.PlatformView.TextAlignment = Windows.UI.Xaml.TextAlignment.Justify;
//                    //};
//#endif
//                }
//            });
//        }
//    }
}
