using System.Globalization;

namespace XXRead.Helpers.Converters
{
    public class MainPageCurrentCategoryLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = value as XStory.DTO.Category;

            return category?.Title ?? "Toutes";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
