using System.Globalization;
using XXRead.Helpers.Constants;

namespace XXRead.Helpers.Converters
{
    public class AuthorNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return MainPageConstants.MAINPAGE_BY_AUTHOR + (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
