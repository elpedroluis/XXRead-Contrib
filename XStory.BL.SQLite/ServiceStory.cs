using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XStory.BL.SQLite.Contracts;
using XStory.DAL.SQLite;
using XStory.DAL.SQLite.Contracts;
using XStory.DTO;
using XStory.Logger;

namespace XStory.BL.SQLite
{
	public class ServiceStory : IServiceStory
	{
		private IRepositoryStory _repositoryStory;
		private IRepositoryAuthorStory _repositoryAuthorStory;

		public ServiceStory(IRepositoryStory repositoryStory)
		{
			_repositoryStory = repositoryStory;
		}

		public ServiceStory(IRepositoryStory repositoryStory,
			IRepositoryAuthorStory repositoryAuthorStory)
		{
			_repositoryStory = repositoryStory;
			_repositoryAuthorStory = repositoryAuthorStory;
		}

		public async Task<List<Story>> GetStories()
		{
			return await _repositoryStory.GetStories();
		}

		public async Task<Story> GetStory(string url)
		{
			var story = await _repositoryStory.GetStory(url);
			return story;
		}

		public async Task<int> InsertStoryItem(Story story)
		{
			return await _repositoryStory.InsertStory(story);
		}

		/// <summary>
		/// Inserts in a transaction : 
		/// 1. Story
		/// 2. AuthorStory
		/// 3. Author
		/// --- Rollback transaction if error.
		/// </summary>
		/// <param name="story"></param>
		/// <returns></returns>
		public async Task<bool> InsertStoryWithAuthorTransac(Story story)
		{
			bool success = false;
			try
			{
				var insert = await _repositoryAuthorStory.InsertAuthorStoryTransac(story);
				if (insert > 0)
				{
					success = true;
				}
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
			}
			return success;
		}

		public async Task<int> DeleteStory(Story story)
		{
			return await _repositoryStory.DeleteStory(story);
		}
	}
}
