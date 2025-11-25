using XStory.DTO;

namespace XXRead.Helpers
{
	/// <summary>
	/// Class used to keep and reuse some static data for the current app run.
	/// </summary>
	public static class StaticContext
	{
		/// <summary>
		/// List of Story already loaded during this app run.
		/// </summary>
		public static List<Story> ListAlreadyLoadedStories = new List<Story>();

		/// <summary>
		/// List of Author already loaded during this app run.
		/// </summary>
		public static List<Author> ListAlreadyLoadedAuthors = new List<Author>();

		/// <summary>
		/// Current datasource used for Content (ex : XStory, HDS...)
		/// </summary>
		public static string DATASOURCE;

		/// <summary>
		/// Current Main color (StatusBar, Windows' borders...)
		/// </summary>
		public static Color ThemeMain { get; set; }

		/// <summary>
		/// Current Theme primary color (Dark, Light...)
		/// </summary>
		public static Color ThemePrimary { get; set; }

		/// <summary>
		/// Current Theme secondary color (Dark shades, Light levels...)
		/// </summary>
		public static Color ThemeSecondary { get; set; }

		/// <summary>
		/// Current Theme Font primary color
		/// </summary>
		public static Color ThemeFontPrimary { get; set; }

		/// <summary>
		/// Current Theme Font secondary color
		/// </summary>
		public static Color ThemeFontSecondary { get; set; }
	}
}
