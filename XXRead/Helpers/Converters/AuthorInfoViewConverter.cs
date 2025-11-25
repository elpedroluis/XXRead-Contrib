using System.Globalization;

namespace XXRead.Helpers.Converters
{
	public class AuthorInfoViewConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (Helpers.StaticContext.DATASOURCE)
			{
				case "HDS":
					return new Views.ContentViews.Author.AuthorInfoHDSView();

				default/*XStory*/:
					return new Views.ContentViews.Author.AuthorInfoXStoryView();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
