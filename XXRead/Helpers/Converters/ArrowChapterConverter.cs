using System.Globalization;

namespace XXRead.Helpers.Converters
{
    public class ArrowChapterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string param = parameter as string;
                if (string.IsNullOrWhiteSpace(param)) throw new ArgumentNullException(nameof(parameter), @"Arrow parameter cannot be null. (Should be left/right)");

                var story = value as XStory.DTO.Story;
                if (story == null)
                {
                    throw new ArgumentException(nameof(value), "");
                }
                else
                {
                    int chapterNumber = story.ChapterNumber;
                    if (story.ChaptersList == null || story.ChaptersList.Count == 0)
                    {
                        return false;
                    }

                    switch (param)
                    {
                        case "left":
                            return story.ChaptersList.ElementAtOrDefault((chapterNumber - 1) - 1) != null ? true : false;

                        case "right":
                            return story.ChaptersList.ElementAtOrDefault((chapterNumber + 1) - 1) != null ? true : false;
                    }

                }
            }
            catch (ArgumentNullException ex)
            {
                XStory.Logger.ServiceLog.Error(ex);
            }
            catch (ArgumentException ex)
            {
                XStory.Logger.ServiceLog.Error(ex);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
