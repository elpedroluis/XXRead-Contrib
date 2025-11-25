using System.Globalization;

namespace XXRead.Helpers.Converters
{
	public class DataSourceImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (DeviceInfo.Current.Platform == DevicePlatform.Android)
			{
				return value;
			}
			else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
			{
				return "";
			}
			else
			{
				return "une string";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
