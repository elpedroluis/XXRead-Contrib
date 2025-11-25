using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using XStory.Helpers.CustomRenderers;
using XStory.UWP.CustomRenderers;

[assembly: ExportRenderer(typeof(LabelJustify), typeof(LabelJustifyRenderer))]
namespace XStory.UWP.CustomRenderers
{
	public class LabelJustifyRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.TextAlignment = Windows.UI.Xaml.TextAlignment.Justify;
			}
		}
	}
}
