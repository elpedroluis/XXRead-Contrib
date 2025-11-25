using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStory.DAL.SQLite.Contracts;

namespace XStory.DAL.SQLite
{
	public class XXReadDatabase : IXXReadDatabase
	{
		public SQLiteAsyncConnection Database;


		public XXReadDatabase()
		{
			Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
			Task.Run(this.InitDatabase).Wait();
		}

		private async Task InitDatabase()
		{
			await Database.CreateTableAsync<SQLiteObjects.Author>();
			await Database.ExecuteAsync(
				@"CREATE TABLE if not exists AuthorStory (
					AuthorId varchar(50), 
					StoryId varchar(100), 
					PRIMARY KEY (AuthorId, StoryId))");
			await Database.CreateTableAsync<SQLiteObjects.Story>();
			await Database.CreateTableAsync<SQLiteObjects.Category>();
		}

		public SQLiteAsyncConnection GetInstance()
		{
			return Database;
		}
	}
}
