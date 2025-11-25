using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XStory.DTO;

namespace XStory.BL.Web.HDS.Contracts
{
	public interface IServiceStory
	{
		Task<Story> GetStory(string path);
		Task<List<Story>> GetStoriesPage(int page = 0, string categoryUrl = "", string sortCriterion = "");
		List<Story> FilterStories(List<Story> stories, List<string> hiddenCategories);
		string GetAuthorAvatar(string authorId);
		List<Story> GetAuthorStories(IEnumerable<HtmlNode> authorStoriesContainer);
	}
}
