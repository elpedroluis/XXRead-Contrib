using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using XStory.BL.Common.Contracts;
using XStory.DTO;
using XStory.Logger;

namespace XStory.BL.Common
{
	public class ServiceStory : BL.Common.Contracts.IServiceStory
	{
		private BL.Web.DSLocator.Contracts.IServiceStory _dsServiceStory;
		private BL.SQLite.Contracts.IServiceStory _serviceStorySQLite;
		private BL.SQLite.Contracts.IServiceAuthor _serviceAuthorSQLite;

		public ServiceStory(BL.Web.DSLocator.Contracts.IServiceStory dsServiceStory,
			BL.SQLite.Contracts.IServiceStory serviceStorySQLite)
		{
			_dsServiceStory = dsServiceStory;
			_serviceStorySQLite = serviceStorySQLite;
		}

		private async Task<List<DTO.Story>> GetStories(int pageNumber = -1)
		{
			pageNumber = pageNumber != -1 ? pageNumber : StaticContext.PageNumber;

			var stories = await _dsServiceStory.GetStoriesPage(StaticContext.DataSource.ToString(), pageNumber, StaticContext.CurrentCategory?.Url);

			if (StaticContext.CurrentCategory == null)
			{
				// If CurrentCategory is defined, no need to filter because the category is already chosen, so valid
				stories = _dsServiceStory.FilterStories(StaticContext.DataSource.ToString(), stories, StaticContext.HiddenCategories);
			}

			return stories;
		}

		public async Task<List<Story>> InitStories()
		{
			try
			{
				List<Story> stories = await this.GetStories();

				return stories;
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<DTO.Story> InitStory()
		{
			try
			{
				DTO.Story story = await _dsServiceStory.GetStory(StaticContext.DataSource.ToString(), StaticContext.CurrentStory?.Url);
				return story;
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return null;
			}
		}

		public List<DTO.Story> DistinctStories(List<DTO.Story> stories)
		{
			return stories.GroupBy(s => s.Url)
				.Select(s => s.First())
				.ToList();
		}

		public async Task<List<Story>> LoadMoreStories()
		{
			this.IncrementPageNumber();

			List<Story> storiesNext = await this.GetStories();

			return storiesNext;
		}

		public async Task<List<Story>> RefreshStories(Story firstStory)
		{
			int pageNumber = 1;

			List<DTO.Story> storiesRefresh = await this.GetStories(pageNumber);

			if (storiesRefresh.FirstOrDefault()?.Url == firstStory.Url)
			{
				return new List<Story>();
			}

			return storiesRefresh;

		}

		public void SetCurrentStory(Story story)
		{
			StaticContext.CurrentStory = story;
		}

		public DTO.Story GetCurrentStory()
		{
			return StaticContext.CurrentStory;
		}

		/// <summary>
		/// Store in cache, if not already
		/// </summary>
		/// <param name="story"></param>
		public void AddAlreadyLoadedStory(Story story)
		{
			if (!StaticContext.ListAlreadyLoadedStories.Exists(als => als.Url == story.Url))
			{
				StaticContext.ListAlreadyLoadedStories.Add(story);
			}

		}

		public Story GetAlreadyLoadedStory(DTO.Story story)
		{
			return StaticContext.ListAlreadyLoadedStories.FirstOrDefault(als => als.Url.Contains(story.Url));
		}

		public void SetPageNumber(int value)
		{
			StaticContext.PageNumber = value;
		}

		public void IncrementPageNumber()
		{
			StaticContext.PageNumber++;
		}

		public void ResetPageNumber()
		{
			StaticContext.PageNumber = 1;
		}


		/* SQLite */

		public async Task<List<DTO.Story>> GetStoriesSQLite()
		{
			List<DTO.Story> storiesSQLite = new List<Story>();
			try
			{
				storiesSQLite = await _serviceStorySQLite.GetStories();

				storiesSQLite = storiesSQLite.OrderByDescending(s => s.ReleaseDate).ToList();
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return null;
			}
			return storiesSQLite;
		}

		public async Task<Story> GetStorySQLite(DTO.Story story)
		{
			try
			{
				if (story == null || string.IsNullOrWhiteSpace(story.Url))
				{
					throw new ArgumentException("Parameter is not valid.");
				}
				DTO.Story storySQLite = await _serviceStorySQLite.GetStory(story.Url);
				return storySQLite;
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<int> InsertStorySQLite(DTO.Story story)
		{
			int affectedRows;

			try
			{
				affectedRows = await _serviceStorySQLite.InsertStoryItem(story);
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				affectedRows = -1;
			}
			return affectedRows;
		}

		public async Task<int> DeleteStorySQLite(DTO.Story story)
		{
			int affectedRows;

			try
			{
				affectedRows = await _serviceStorySQLite.DeleteStory(story);
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				affectedRows = -1;
			}
			return affectedRows;
		}
	}
}