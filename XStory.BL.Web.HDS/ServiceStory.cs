using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XStory.BL.Web.HDS.Contracts;
using XStory.BL.Web.HDS.Helpers;
using XStory.DAL.Web.HDS;
using XStory.DAL.Web.HDS.Contracts;
using XStory.DTO;
using XStory.Logger;

namespace XStory.BL.Web.HDS
{
	public class ServiceStory : IServiceStory
	{
		private IRepositoryWebHDS _repositoryWeb;

		public const string STORIES_XPATH = "/html/body/div/div[2]/div[4]/div/div[2]/div/div[3]/div";

		public const string STORY_CONTAINER_XPATH = "/html/body/div/div[2]/div[4]/div/div[1]";
		public const string STORY_CONTENT_XPATH = "/html/body/div/div[2]/div[4]/div/div[1]/div[5]/div[2]";

		public const string AVATAR_URL = "avatars/6265.jpg";
		public const string UNKNOWN_AUTHOR_AVATAR = "unknown_author_avatar";

		public const string IMAGE_XPATH = "/html/body/img";

		public ServiceStory()
		{
			_repositoryWeb = new RepositoryWebHDS();
		}

		public async Task<Story> GetStory(string path)
		{
			Story story = new Story();
			try
			{
				Uri uri = new Uri(_repositoryWeb.GetHttpClient().BaseAddress, path);

				// Whole page
				HtmlDocument html = new HtmlDocument();
				html.LoadHtml(await _repositoryWeb.GetHtmlPage(uri.ToString()));

				HtmlNode document = html.DocumentNode;

				story.Url = uri.ToString();

				// HEADER infos
				HtmlNode storyContainer = document.SelectSingleNode(STORY_CONTAINER_XPATH);
				this.InitStoryInfos(story, storyContainer);

				// HEADER Author avatar
				this.SetStoryAuthorAvatar(story);

				// CONTENT
				HtmlNode storyContentContainer = document.SelectSingleNode(STORY_CONTENT_XPATH);
				this.InitStoryContent(story, storyContentContainer);
			}
			catch (Exception ex)
			{
				story = null;
				Logger.ServiceLog.Error(ex);
			}
			return story;
		}

		private void InitStoryInfos(Story story, HtmlNode storyContainer)
		{
			// TITLE
			var titleNode = storyContainer.SelectSingleNode("h2");
			if (titleNode != null && titleNode.Attributes["class"]?.Value == "title")
			{
				string title = titleNode.InnerText;
				story.Title = title;
			}

			// AUTHOR
			if (story.Author == null)
			{
				// Avatar
				string authorAvatar = string.Empty;

				var autorAvatarNode = storyContainer.SelectSingleNode("div[2]/div[1]/img");
				if (autorAvatarNode != null && autorAvatarNode.Attributes["src"] != null)
				{
					authorAvatar = autorAvatarNode.Attributes["src"].Value;
				}


				var authorNode = storyContainer.SelectSingleNode("div[2]/div[2]/div[1]/a");
				if (authorNode != null && authorNode.Attributes["href"] != null)
				{
					string authorUrl = authorNode.Attributes["href"].Value;
					string authorName = authorUrl.Split('=')[1];
					Author author = new Author()
					{
						Name = authorName,
						Url = authorUrl,
						Avatar = authorAvatar,
					};

					story.Author = author;
				}

			}

			var publishNode = storyContainer.SelectSingleNode("div[3]/div[1]");
			if (publishNode != null && publishNode.Attributes["class"]?.Value == "published_at")
			{
				string publishInfo = publishNode.InnerText.Trim();
				//Histoire érotique Publiée sur HDS le 07-02-2023 dans la catégorie Entre-nous, hommes et femmes
				publishInfo = publishInfo.Replace("Histoire érotique Publiée sur HDS le ", "");
				//07-02-2023 dans la catégorie Entre-nous, hommes et femmes

				// RELEASE DATE
				if (string.IsNullOrWhiteSpace(story.ReleaseDate))
				{
					string releaseDate = publishInfo.Remove(10);
					story.ReleaseDate = releaseDate;
				}

				if (string.IsNullOrWhiteSpace(story.CategoryName))
				{
					//07-02-2023 dans la catégorie Entre-nous, hommes et femmes
					string category = publishInfo.Replace(" dans la catégorie ", "");
					category = string.Concat(category.Skip(10));

					story.CategoryName = category;

					story.CategoryUrl = Helpers.StaticUtils.StoryCategoryUrlDictionary[category];
				}
			}

			int viewsCountDivIndex = 2;

			// TAGS
			// /html/body/div/div[2]/div[3]/div/div[1]/div[3]/div[2]
			var tagsNode = storyContainer.SelectSingleNode("div[3]/div[2]");
			if (tagsNode != null && tagsNode.Attributes["class"]?.Value == "tags")
			{
				// TODO tags

				viewsCountDivIndex++;
			}

			// VIEWS
			var viewsNode = storyContainer.SelectSingleNode("div[3]/div[" + viewsCountDivIndex + "]/b");
			if (viewsNode != null)
			{
				string viewsCount = viewsNode.InnerText;
				story.ViewsNumber = long.Parse(string.Concat(viewsCount.Split(' ')));
			}
		}

		private void SetStoryAuthorAvatar(Story story)
		{
			string avatar = this.GetAuthorAvatar(story.Author.Avatar);
			story.Author.Avatar = avatar;
		}

		private void InitStoryContent(Story story, HtmlNode storyContentContainer)
		{
			// CONTENT
			string content = storyContentContainer.InnerText.Replace("<br>", Environment.NewLine);
			story.Content = content;
		}

		private async Task<List<Story>> GetStoriesBase(Uri uri)
		{
			try
			{
				HtmlDocument html = new HtmlDocument();
				html.LoadHtml(await _repositoryWeb.GetHtmlPage(uri.ToString()));

				HtmlNode document = html.DocumentNode;
				var storiesContainer = document.SelectNodes(STORIES_XPATH).Where(node => node.Attributes["class"].Value == "story abstract");

				List<Story> stories = this.GetStoriesFromContainer(storiesContainer);

				return stories;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + Environment.NewLine + ex.InnerException);
			}
			return null;
		}

		private List<Story> GetStoriesFromContainer(IEnumerable<HtmlNode> storiesContainer)
		{
			List<Story> stories = new List<Story>();
			foreach (var container in storiesContainer)
			{
				var publishedNode = container.SelectSingleNode("div[1]");
				var categoryNode = container.SelectSingleNode("div[2]");
				var authorNode = container.SelectSingleNode("div[3]/a");
				var titleNode = container.SelectSingleNode("div[4]/a");
				var textNode = container.SelectSingleNode("div[5]");

				Story story = new Story();

				// RELEASE DATE

				// Histoire erotique publiée sur Histoires De Sexe le 07-02-2023 à 09 heures
				string releaseDate = publishedNode.InnerText.Replace(" Histoire erotique publiée sur Histoires De Sexe le ", "");

				//07-02-2023 à 09 heures
				releaseDate = releaseDate.Replace(" à ", " ");

				//07-02-2023 09:00:00
				releaseDate = releaseDate.Replace(" heures", ":00:00");

				story.ReleaseDate = releaseDate;

				// TITLE
				story.Title = titleNode.InnerText;

				// URL
				story.Url = titleNode.Attributes["href"].Value;

				// CATEGORY
				string category = categoryNode?.InnerText?.Split(':')[1]?.Trim();

				if (!string.IsNullOrEmpty(category))
				{
					story.CategoryName = category;
					story.CategoryUrl = Helpers.StaticUtils.StoryCategoryUrlDictionary[category];
				}

				// AUTHOR
				Author author = new Author();
				author.Url = authorNode.Attributes["href"].Value;
				author.Name = authorNode.InnerText;
				story.Author = author;

				stories.Add(story);
			}

			return stories;
		}

		public async Task<List<Story>> GetStoriesPage(int page = 0, string categoryUrl = "", string sortCriterion = "")
		{
			try
			{
				string basePath = "/sexe/";
				string categoryPath = string.Empty;
				string pagePath = page > 1 ? "?p=" + page : "";
				string extensionPath = ".php";

				if (!string.IsNullOrWhiteSpace(categoryUrl))
				{
					categoryPath = StaticUtils.CategoryFromUrlDictionary[categoryUrl];
				}
				else
				{
					categoryPath = "histoires-par-date";
				}

				Uri uri = new Uri(_repositoryWeb.GetHttpClient().BaseAddress,
					string.Concat(basePath, categoryPath, extensionPath, pagePath, sortCriterion));
				return await GetStoriesBase(uri);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return null;
		}

		public List<Story> FilterStories(List<Story> stories, List<string> hiddenCategories)
		{
			if (hiddenCategories != null && hiddenCategories.Count > 0)
			{
				stories = stories.Where(story =>
				{
					bool isValid = true;
					foreach (var category in hiddenCategories)
					{
						if (story.CategoryUrl == category)
						{
							isValid = false;
							continue;
						}
					}
					return isValid;
				}).ToList();
			}

			return stories;
		}

		public string GetAuthorAvatar(string authorAvatar)
		{
			try
			{
				return string.Concat(_repositoryWeb.GetHttpClient().BaseAddress, authorAvatar);
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return UNKNOWN_AUTHOR_AVATAR;
			}
		}

		public List<Story> GetAuthorStories(IEnumerable<HtmlNode> authorStoriesContainer)
		{
			var stories = this.GetStoriesFromContainer(authorStoriesContainer);

			return stories;
		}
	}
}
