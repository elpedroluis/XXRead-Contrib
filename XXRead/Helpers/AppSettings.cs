using XXRead.Helpers.Constants;

namespace XXRead.Helpers
{
    public static class AppSettings
    {
        public static bool FirstRun
        {
            get => Preferences.Get(nameof(FirstRun), true);
            set => Preferences.Set(nameof(FirstRun), value);
        }

        public static string DataSource
        {
            get => Preferences.Get(nameof(DataSource), GlobalConstants.STORIES_SOURCE_XSTORY);
            set => Preferences.Set(nameof(DataSource), value);
        }

        public static bool DataSourceChanged
        {
            get => Preferences.Get(nameof(DataSourceChanged), false);
            set => Preferences.Set(nameof(DataSourceChanged), value);
        }

        public static bool HiddenCategoriesChanged
        {
            get => Preferences.Get(nameof(HiddenCategoriesChanged), false);
            set => Preferences.Set(nameof(HiddenCategoriesChanged), value);
        }

        public static string ThemeFontPrimary
        {
            get => Preferences.Get(nameof(ThemeFontPrimary), Themes.Theme.FontLightPrimary);
            set => Preferences.Set(nameof(ThemeFontPrimary), value);
        }

        public static string ThemeFontSecondary
        {
            get => Preferences.Get(nameof(ThemeFontSecondary), Themes.Theme.FontLightSecondary);
            set => Preferences.Set(nameof(ThemeFontSecondary), value);
        }

        public static string ThemePrimary
        {
            get => Preferences.Get(nameof(ThemePrimary), Themes.Theme.DarkPrimary);
            set => Preferences.Set(nameof(ThemePrimary), value);
        }

        public static string ThemeSecondary
        {
            get => Preferences.Get(nameof(ThemeSecondary), Themes.Theme.DarkSecondary);
            set => Preferences.Set(nameof(ThemeSecondary), value);
        }

        public static string ThemeMain
        {
            get => Preferences.Get(nameof(ThemeMain), Themes.Theme.MainRed);
            set => Preferences.Set(nameof(ThemeMain), value);
        }
    }
}
