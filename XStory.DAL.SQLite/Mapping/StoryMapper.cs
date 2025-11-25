using System;
using System.Collections.Generic;
using System.Text;

namespace XStory.DAL.SQLite.Mapping
{
	public class StoryMapper
	{
		public static DTO.Story ToStoryDTO(SQLiteObjects.Story story)
		{
			return new DTO.Story()
			{
				CategoryName = story.CategoryName,
				CategoryUrl = story.CategoryUrl,
				ChapterName = story.ChapterName,
				ChapterNumber = story.ChapterNumber,
				Content = story.Content,
				Id = story.Id,
				LikesNumber = story.LikesNumber,
				ReleaseDate = story.ReleaseDate,
				ReviewsNumber = story.ReviewsNumber,
				Title = story.Title,
				Type = story.Type,
				Url = story.Url,
				ViewsNumber = story.ViewsNumber,
			};
		}

		public static SQLiteObjects.Story ToStorySQLite(DTO.Story story)
		{
			return new SQLiteObjects.Story()
			{
				AuthorId = story.Author.Id,
				CategoryName = story.CategoryName,
				CategoryUrl = story.CategoryUrl,
				ChapterName = story.ChapterName,
				ChapterNumber = story.ChapterNumber,
				Content = story.Content,
				Id = story.Id,
				LikesNumber = story.LikesNumber,
				ReleaseDate = story.ReleaseDate,
				ReviewsNumber = story.ReviewsNumber,
				Title = story.Title,
				Type = story.Type,
				Url = story.Url,
				ViewsNumber = story.ViewsNumber,
			};
		}
	}
}
