using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XStory.Helpers.Converters
{
	public class StoryDateTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrWhiteSpace((string)value))
			{ return ""; }

			DateTime date;

			switch (StaticContext.DATASOURCE)
			{
				case "HDS":
					date = DateTime.ParseExact(value as string, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
					break;

				default:
					date = DateTime.Parse(value as string);
					break;
			}

			return date.ToString("dd/MM/yyyy HH:mm");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
