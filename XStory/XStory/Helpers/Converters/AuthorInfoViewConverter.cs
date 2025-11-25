using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace XStory.Helpers.Converters
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
