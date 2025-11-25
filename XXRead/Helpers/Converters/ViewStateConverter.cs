using System.Globalization;

namespace XXRead.Helpers.Converters
{
    public class ViewStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is ViewStateEnum.Loading)
            {
                if (parameter == null || (string)parameter == "error")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (value is ViewStateEnum.Error)
            {
                if (parameter == null || (string)parameter == "loading")
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else // (value is ViewStateEnum.Display)
            {
                if (parameter == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


        }

        // si value == false et parameter == null
        /*string loadingParameter = parameter as string;

        if((bool)value == true)
        {
            if (!string.IsNullOrWhiteSpace(loadingParameter))
            {
                return true;
            }
            return false;
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(loadingParameter))
            {
                return false;
            }
            return true;
        }*/


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

