namespace XXRead.Helpers.Styles
{
	public class FrameStyles
	{
		public static Style MainFrameStyle = new Style(typeof(Frame))
		{
			Setters =
			{
				new Setter() { Property = Frame.HasShadowProperty, Value = true },
				new Setter() { Property = Frame.CornerRadiusProperty, Value = 3 },
				new Setter() { Property = Layout.PaddingProperty, Value = new Thickness(5) },
				new Setter() { Property = View.MarginProperty, Value = new Thickness(13) },
				new Setter() { Property = Frame.BorderColorProperty, Value = Colors.Transparent },
				new Setter() { Property = Frame.BackgroundColorProperty, Value = Colors.Transparent },
			}
		};
	}
}
