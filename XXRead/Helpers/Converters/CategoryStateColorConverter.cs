using System.Globalization;

namespace XXRead.Helpers.Converters
{
	/// <summary>
	/// <br>Returns the Color of the Category icon regarding its state (Enabled/Disabled).</br>
	/// <br>If enabled : Main Color</br>
	/// <br>If disabled : Gray Color</br>
	/// </summary>
	public class CategoryStateColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return Color.FromArgb(AppSettings.ThemeMain);
			}
			else
			{
				return Color.FromArgb("#5E5E5E");
			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
