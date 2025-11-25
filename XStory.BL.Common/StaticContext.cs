using System;
using System.Collections.Generic;
using System.Text;
using XStory.DTO.Config;

namespace XStory.BL.Common
{
	internal static class StaticContext
	{
		public static DataSources DataSource { get; set; }
		public static DTO.Story CurrentStory { get; set; }
		public static DTO.Author CurrentAuthor { get; set; }
		public static DTO.Category CurrentCategory { get; set; }

		public static List<DTO.Story> ListAlreadyLoadedStories { get; set; } = new List<DTO.Story>();
		public static List<DTO.Author> ListAlreadyLoadedAuthors { get; set; } = new List<DTO.Author>();

		public static int PageNumber { get; set; } = 1;
		public static int PageNumberAuthor { get; set; } = 1;
		public static List<string> HiddenCategories { get; set; } = new List<string>();
	}
}
