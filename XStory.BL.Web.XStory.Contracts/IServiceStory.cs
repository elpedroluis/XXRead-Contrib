using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XStory.DTO;

namespace XStory.BL.Web.XStory.Contracts
{
	public interface IServiceStory
	{
		Task<Story> GetStory(string storyUrl);
		Task<List<Story>> GetStoriesPage(int page = 0, string categoryUrl = "", string sortCriterion = "");
		Task<List<Story>> GetAuthorStories(string authorPageUrl);
		List<Story> FilterStories(List<Story> stories, List<string> hiddenCategories);
		Task<string> GetAuthorAvatar(string authorId);
	}
}
