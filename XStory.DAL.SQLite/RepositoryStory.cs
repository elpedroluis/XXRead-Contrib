using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XStory.DAL.SQLite.Contracts;
using XStory.DAL.SQLite.Mapping;
using XStory.DTO;
using XStory.Logger;

namespace XStory.DAL.SQLite
{
	public class RepositoryStory : Repository, IRepository, IRepositoryStory
	{
		public RepositoryStory(IXXReadDatabase database) : base(database)
		{
			_database = database;
		}

		public async Task<List<Story>> GetStories()
		{
			List<DTO.Story> stories = new List<DTO.Story>();

			List<SQLiteObjects.Story> dbStories = await _database.GetInstance().QueryAsync<SQLiteObjects.Story>("select * from Story");

			dbStories.ForEach(storySQL =>
			{
				stories.Add(StoryMapper.ToStoryDTO(storySQL));
			});

			return stories;
		}

		public async Task<Story> GetStory(string url)
		{
			try
			{
				SQLiteObjects.Story dbStory = await _database.GetInstance().GetAsync<SQLiteObjects.Story>(url);

				if (dbStory == null)
				{
					throw new Exception("Could not find Story in database.");

				}
				return StoryMapper.ToStoryDTO(dbStory);
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<int> InsertStory(Story story)
		{
			try
			{
				SQLiteObjects.Story storySQL = Mapping.StoryMapper.ToStorySQLite(story);
				return await _database.GetInstance().InsertOrReplaceAsync(storySQL);
			}
			catch (Exception ex)
			{

				return -1;
			}
		}

		public async Task<int> DeleteStory(Story story)
		{
			SQLiteObjects.Story storySQL = Mapping.StoryMapper.ToStorySQLite(story);
			return await _database.GetInstance().DeleteAsync(storySQL);
		}
	}
}
