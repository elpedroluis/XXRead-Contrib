using System;
using System.Collections.Generic;
using System.Text;

namespace XStory.DAL.SQLite.Mapping
{
	public class AuthorMapper
	{
		public static DTO.Author ToAuthorDTO(SQLiteObjects.Author author)
		{
			return new DTO.Author()
			{
				Id = author.Id,
				Name = author.Name,
				Url = author.Url
			};
		}

		public static SQLiteObjects.Author ToAuthorSQLite(DTO.Author author)
		{
			return new SQLiteObjects.Author()
			{
				Id = author.Id,
				Name = author.Name,
				Url = author.Url
			};
		}
	}
}
