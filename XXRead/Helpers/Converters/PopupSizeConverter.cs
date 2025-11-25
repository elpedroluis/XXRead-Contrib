using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXRead.Helpers.Converters
{
	public class PopupSizeConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				return new Microsoft.Maui.Graphics.Size() { Height = 600, Width = 350 };
			}
			else if (DeviceInfo.Platform == DevicePlatform.WinUI)
			{
				return new Microsoft.Maui.Graphics.Size() { Height = 600, Width = 800 };
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
