namespace XXRead.Helpers.Styles
{
	public class ImageStyles
	{
		public static Style CategoryThumbnailImageStyle = new Style(typeof(Image))
		{
			Setters =
			{
				new Setter() { Property = VisualElement.HeightRequestProperty, Value = 25 },
				new Setter() { Property = VisualElement.WidthRequestProperty, Value = 25 }
			}
		};

		public static Style TitleImageStyle = new Style(typeof(Image))
		{
			Setters =
			{
				new Setter() { Property = VisualElement.HeightRequestProperty, Value = 50 },
				new Setter() { Property = VisualElement.WidthRequestProperty, Value = 50 }
			}
		};
	}
}
