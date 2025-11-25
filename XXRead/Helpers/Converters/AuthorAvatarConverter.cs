using System.Globalization;

namespace XXRead.Helpers.Converters
{
	public class AuthorAvatarConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrWhiteSpace((string)value))
			{
				if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
				{
					return "asset/unknown_author_avatar";
				}
				else
				{
					return "unknown_author_avatar";
				}
			}
			else
			{
				return value;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
