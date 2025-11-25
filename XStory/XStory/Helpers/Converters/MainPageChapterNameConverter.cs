using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using XStory.DTO;

namespace XStory.Helpers.Converters
{
	/// <summary>
	/// On MainPage : Defines if separator + chapter name is visible or not if no value
	/// </summary>
	public class MainPageChapterNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrWhiteSpace(value as string))
			{

				return false;
			}
			else
			{
				return true;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
