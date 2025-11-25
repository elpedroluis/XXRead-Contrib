using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XStory.BL.Web.DSLocator.Contracts;
using XStory.DTO;

namespace XStory.BL.Web.DSLocator
{
	public class ServiceStory : IServiceStory
	{
		private BL.Web.XStory.Contracts.IServiceStory _serviceStoryXStory;
		private BL.Web.HDS.Contracts.IServiceStory _serviceStoryHDS;
		private BL.Web.Demo.Contracts.IServiceStory _serviceStoryDemo;

		public ServiceStory(
			BL.Web.XStory.Contracts.IServiceStory serviceStoryXStory,
			BL.Web.HDS.Contracts.IServiceStory serviceStoryHDS,
			BL.Web.Demo.Contracts.IServiceStory serviceStoryDemo)
		{
			_serviceStoryXStory = serviceStoryXStory;
			_serviceStoryHDS = serviceStoryHDS;
			_serviceStoryDemo = serviceStoryDemo;
		}

		public List<Story> FilterStories(string dataSource, List<Story> stories, List<string> hiddenCategories)
		{
			try
			{
				switch (dataSource)
				{
					case "XStory":
						return _serviceStoryXStory.FilterStories(stories, hiddenCategories);
					case "HDS":
						return _serviceStoryHDS.FilterStories(stories, hiddenCategories);
					case "Demo":
						return _serviceStoryDemo.FilterStories(stories, hiddenCategories);
					default /* "All" */:
						// TO BE IMPLEMENTED
						return null;
				}
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<Story> GetStory(string dataSource, string path)
		{
			try
			{
				switch (dataSource)
				{
					case "XStory":
						return await _serviceStoryXStory.GetStory(path);
					case "HDS":
						return await _serviceStoryHDS.GetStory(path);
					case "Demo":
						return await _serviceStoryDemo.GetStory(path);
					default /* "All" */:
						// TO BE IMPLEMENTED
						return null;
				}
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<List<Story>> GetStoriesPage(string dataSource, int page = 0, string category = "", string sortCriterion = "")
		{
			try
			{
				switch (dataSource)
				{
					case "XStory":
						return await _serviceStoryXStory.GetStoriesPage(page, category, sortCriterion);
					case "HDS":
						return await _serviceStoryHDS.GetStoriesPage(page, category, sortCriterion);
					case "Demo":
						return await _serviceStoryDemo.GetStoriesPage(page, category, sortCriterion);
					default /* "All" */:
						// TO BE IMPLEMENTED
						return null;
				}
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<string> GetAuthorAvatar(string dataSource, string authorId)
		{
			try
			{
				switch (dataSource)
				{
					case "XStory":
						return await _serviceStoryXStory.GetAuthorAvatar(authorId);
					case "HDS":
						return await Task.Run(() => _serviceStoryHDS.GetAuthorAvatar(authorId));
					case "Demo":
						return await _serviceStoryDemo.GetAuthorAvatar(authorId);
					default /* "All" */:
						// TO BE IMPLEMENTED
						return null;
				}
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}
	}
}
