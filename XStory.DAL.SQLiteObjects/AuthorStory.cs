using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XStory.DAL.SQLiteObjects
{
	[Table("AuthorStory")]
	public class AuthorStory
	{
		[Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
		public string AuthorId { get; set; }

		[Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
		public string StoryId { get; set; }
	}
}
