using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using XStory.BL.Web.XStory.Contracts;
using XStory.BL.Web.XStory.Helpers;
using XStory.DAL.Web.XStory;
using XStory.DAL.Web.XStory.Contracts;
using XStory.DTO;
using XStory.Logger;

namespace XStory.BL.Web.XStory
{
	public class ServiceStory : IServiceStory
	{
		private IRepositoryWebXStory _repositoryWeb;

		private Uri _baseAdress;

		public const string HTML_BR = "br";
		public const string HTML_CLASS = "class";
		public const string HTML_DATETIME = "datetime";
		public const string HTML_DIV = "div";
		public const string HTML_FA_QUESTION = "fa fa-question";
		public const string HTML_FA_EYE = "fa fa-eye";
		public const string HTML_HREF = "href";

		public const string CHAPTER_TITLE_BEGIN = "Chapitre ";
		public const string UNIQUE = "unique";

		public const string XS_LIRE_HISTOIRE_PARAGRAPHE = "xs-lire-histoire-paragraphe";

		public const string STORIES_XPATH = "/html/body/div[1]/main/section[4]/div[2]/ul/li/div";

		public const string STORY_HEADER_XPATH = "/html/body/div[1]/main/div[1]/section[1]";
		public const string STORY_CONTENT_XPATH = "/html/body/div[1]/main/div[1]/section[2]";
		public const string STORY_FOOTER_XPATH = "/html/body/div[1]/main/div[1]/section[3]";

		public const string STORY_CHAPTERS_XPATH = "/html/body/div[1]/aside/div[1]/section[1]/div/ul/li";

		public const string AUTHOR_PAGE_STORIES = "/html/body/div[1]/main/div[2]/section[2]/div/ul/li/div";

		public ServiceStory()
		{
			_repositoryWeb = new RepositoryWebXStory();

			_baseAdress = _repositoryWeb.GetHttpClient().BaseAddress;
		}

		public async Task<List<Story>> GetStoriesPage(int page = 0, string categoryUrl = "", string sortCriterion = "")
		{
			try
			{
				string basePath = "/histoires-erotiques";
				string categoryPath = string.Empty;
				string pagePath;
				string endPath = ".html";

				if (!string.IsNullOrWhiteSpace(categoryUrl))
				{
					categoryPath = StaticUtils.CategoryFromUrlDictionary[categoryUrl];
					pagePath = page == 0 ? "" : "," + page;
				}
				else
				{
					pagePath = page > 1 ? ",,," + page : "";
				}

				Uri uri = new Uri(_baseAdress, string.Concat(basePath, categoryPath, pagePath, sortCriterion, endPath));
				return await GetStoriesBase(uri);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return null;
		}

		public async Task<Story> GetStory(string storyUrl)
		{

			Story story = new Story();
			try
			{
				Uri uri = new Uri(storyUrl);

				// Whole page
				HtmlDocument html = new HtmlDocument();
				html.LoadHtml(await _repositoryWeb.GetHtmlPage(uri.ToString()));

				HtmlNode document = html.DocumentNode;

				story.Url = uri.ToString();

				// HEADER infos
				HtmlNode storyHeaderContainer = document.SelectSingleNode(STORY_HEADER_XPATH);
				this.InitStoryInfos(story, storyHeaderContainer);

				// HEADER title + chapter
				this.InitStoryTitleChapter(story, storyHeaderContainer);

				// HEADER category
				this.InitStoryCategory(story, storyHeaderContainer);

				// HEADER AUTHOR avatar
				await this.SetStoryAuthorAvatar(story);

				// CONTENT
				HtmlNode storyContentContainer = document.SelectSingleNode(STORY_CONTENT_XPATH);
				this.InitStoryContent(story, storyContentContainer);


				// CHAPTERS LIST
				var storyChaptersListContainer = document.SelectNodes(STORY_CHAPTERS_XPATH);

				if (storyChaptersListContainer != null)
				{
					GetStorySubChapters(story, storyChaptersListContainer);
				}

			}
			catch (Exception ex)
			{
				story = null;
				Logger.ServiceLog.Error(ex);
			}
			return story;
		}

		private async Task SetStoryAuthorAvatar(Story story)
		{
			string avatar = await this.GetAuthorAvatar(story.Author.Id);
			story.Author.Avatar = avatar;
		}

		private void InitStoryInfos(Story story, HtmlNode storyHeaderContainer)
		{
			// Begin <li>'s
			int headerLiTagCount = 1;
			const string LI_XPATH = "div/div[1]/ul/li[";

			// AUTHOR
			if (story.Author == null)
			{
				string headerAuthorXPath = LI_XPATH + headerLiTagCount + "]/a";
				Author author = new Author()
				{
					Id = storyHeaderContainer.SelectSingleNode(headerAuthorXPath).Attributes["data-author-id"].Value,
					Name = storyHeaderContainer.SelectSingleNode(headerAuthorXPath).InnerText,
					Url = string.Concat(_baseAdress, storyHeaderContainer.SelectSingleNode(headerAuthorXPath).Attributes[HTML_HREF].Value)
				};
				story.Author = author;

				headerLiTagCount++;
			}

			// TYPE
			string headerTypeXPath = LI_XPATH + headerLiTagCount + "]";
			var iType = storyHeaderContainer.SelectSingleNode(headerTypeXPath + "/i");
			if (iType.Attributes[HTML_CLASS].Value == HTML_FA_QUESTION)
			{
				string type = storyHeaderContainer.SelectSingleNode(headerTypeXPath + "/text()[2]").InnerText;
				type = type.Replace(@"\n", "").Replace(@"\t", "").Trim();
				story.Type = type;

				headerLiTagCount++;
			}

			// RELEASE DATE
			if (string.IsNullOrWhiteSpace(story.ReleaseDate))
			{
				string headerReleaseDateXPath = LI_XPATH + headerLiTagCount + "]/time";

				story.ReleaseDate = storyHeaderContainer.SelectSingleNode(headerReleaseDateXPath).Attributes[HTML_DATETIME].Value;
				headerLiTagCount++;
			}

			// VIEWS NUMBER
			string headerViewsNumberXPath = LI_XPATH + headerLiTagCount + "]";
			var iViewsNumber = storyHeaderContainer.SelectSingleNode(headerViewsNumberXPath + "/i");
			if (iViewsNumber.Attributes[HTML_CLASS].Value == HTML_FA_EYE)
			{
				story.ViewsNumber = long.Parse(string.Concat(storyHeaderContainer.SelectSingleNode(headerViewsNumberXPath + "/span").InnerText.Split(' ')));
				headerLiTagCount++;
			}

			// LIKES NUMBER
			string headerLikesNumberXPath = LI_XPATH + headerLiTagCount + "]/a/span";
			story.LikesNumber = long.Parse(storyHeaderContainer.SelectSingleNode(headerLikesNumberXPath).InnerText);
			headerLiTagCount++;

			// REVIEWS NUMBER
			string headerReviewsNumberXPath = LI_XPATH + headerLiTagCount + "]/a";
			story.ReviewsNumber = long.Parse(storyHeaderContainer.SelectSingleNode(headerReviewsNumberXPath).InnerText.Split(' ')[0]);

			// End of <li>'s
		}

		private void InitStoryTitleChapter(Story story, HtmlNode storyHeaderContainer)
		{
			int chapterh2TagCount = 1;
			const string TITLE_CHAPTERS_XPATH = "div/div[3]";

			story.Title = storyHeaderContainer.SelectSingleNode(TITLE_CHAPTERS_XPATH + "/h1").InnerText;

			// Chapter number ("Chapitre x")
			string chapterNumberXPath = TITLE_CHAPTERS_XPATH + "/h2[" + chapterh2TagCount + "]";
			string chapterTryNumber = storyHeaderContainer.SelectSingleNode(chapterNumberXPath).InnerText.Split(' ')[1];
			story.ChapterNumber = chapterTryNumber == UNIQUE ? 1 : int.Parse(chapterTryNumber);
			chapterh2TagCount++;

			// Chapter name                
			story.ChapterName = storyHeaderContainer.SelectSingleNode(chapterNumberXPath).InnerText;
			string chapterNameXPath = TITLE_CHAPTERS_XPATH + "/h2[" + chapterh2TagCount + "]";
			if (storyHeaderContainer.SelectSingleNode(chapterNameXPath) != null)
			{
				story.ChapterName +=
					string.Concat(" : "
					, storyHeaderContainer.SelectSingleNode(chapterNameXPath)?.InnerText ?? string.Empty);
			}
		}


		private void InitStoryCategory(Story story, HtmlNode storyHeaderContainer)
		{
			const string CATEGORY_XPATH = "div/div[3]/a";
			story.CategoryName = storyHeaderContainer.SelectSingleNode(CATEGORY_XPATH + "/div[2]").InnerText;
			story.CategoryUrl = storyHeaderContainer.SelectSingleNode(CATEGORY_XPATH).Attributes[HTML_HREF].Value;
		}

		/// <summary>
		/// Defines the story content.
		/// </summary>
		/// <param name="story">Story to build</param>
		/// <param name="storyContentContainer"></param>
		private void InitStoryContent(Story story, HtmlNode storyContentContainer)
		{
			var storyContentParagraphsContainer = storyContentContainer.SelectNodes(HTML_DIV).Nodes();

			string storyContent = string.Empty;

			// storyContent += storyContentContainer.InnerHtml; //←←← OK Mais zone pub + infos police en pied de texte à masquer...

			foreach (var element in storyContentParagraphsContainer)
			{
				if (element.Name == "p:p")
				{
					if (element.Attributes[HTML_CLASS].Value == XS_LIRE_HISTOIRE_PARAGRAPHE)
					{
						// ↓↓↓ OLD CODE A SUPPRIMER
						if (element.InnerLength == 1)
						{
							storyContent += Environment.NewLine + Environment.NewLine;
						}
						else
						{
							storyContent += Environment.NewLine + element.InnerText;
						}
						// ↑↑↑ OLD CODE A SUPPRIMER

						// ↓↓↓ NEW CODE A GARDER
						/*if (element.InnerLength == 1)
						{
							storyContent += "<br>" + "<br>";
						}
						else
						{
							storyContent += "<br>" + "<br>" + element.InnerText;
						}*/
						// ↑↑↑ NEW CODE A GARDER
					}

				}
				else if (element.Name == HTML_BR)
				{
					storyContent += Environment.NewLine + Environment.NewLine;
				}
			}
			story.Content = storyContent;
		}

		/// <summary>
		/// Get story related chapters.
		/// </summary>
		/// <param name="story">The current story</param>
		/// <param name="storyChaptersListContainer">The chapters HTML node</param>
		private void GetStorySubChapters(Story story, HtmlNodeCollection storyChaptersListContainer)
		{
			foreach (var storyChapter in storyChaptersListContainer)
			{
				Story chapterStory = new Story();
				chapterStory.Author = story.Author;

				// DATE
				string releaseDate = storyChapter.SelectSingleNode("time")?.Attributes[HTML_DATETIME]?.Value;
				chapterStory.ReleaseDate = releaseDate;

				var linkNode = storyChapter.Element("a");
				var secondLinkNode = storyChapter.SelectSingleNode("a[2]");

				// - CHAPTER CATEGORY
				string categoryName = linkNode.SelectSingleNode("i")
					?.Attributes[HTML_CLASS]?.Value
					?.Split(' ')?[1] ?? string.Empty;
				chapterStory.CategoryName = Helpers.StaticUtils.CategorySubChaptersDictionary[categoryName];

				string categoryUrl = string.IsNullOrWhiteSpace(categoryName)
					? string.Empty
					: Helpers.StaticUtils.CategorySubChaptersToCategoryUrlDictionary[categoryName];
				chapterStory.CategoryUrl = categoryUrl;

				string storyUrl = _baseAdress.ToString();
				int chapterNumber;
				string chapterName;

				if (secondLinkNode != null)
				{
					// -> Author Page
					// URL
					storyUrl += secondLinkNode.Attributes[HTML_HREF]?.Value ?? string.Empty;

					// CHAPTER NUMBER
					chapterNumber = int.Parse(secondLinkNode.InnerText.Split(' ')[1]);

					// CHAPTER NAME
					chapterName = storyChapter.
						SelectSingleNode("div[@class='xs-sub-chapitre-soustitre']")?.InnerText
						?? string.Empty;
				}
				else
				{
					// -> Story Page

					// URL
					storyUrl += linkNode.Attributes[HTML_HREF]?.Value ?? string.Empty;

					// CHAPTER NUMBER
					chapterNumber = int.Parse(linkNode.InnerText.Trim().Split(' ')[1]);

					// CHAPTER NAME
					chapterName = linkNode.Attributes["title"]?.Value ?? string.Empty;
				}

				chapterStory.Url = storyUrl;
				chapterStory.ChapterNumber = chapterNumber;
				chapterStory.ChapterName = chapterName;

				string chapterTitle = string.Empty;
				// Beautify display chapter title
				if (chapterNumber > 0)
				{
					if (!string.IsNullOrWhiteSpace(chapterStory.ChapterName))
					{
						chapterTitle = string.Concat(
							CHAPTER_TITLE_BEGIN,
							chapterStory.ChapterNumber,
							" : ",
							chapterStory.ChapterName);
					}
					else
					{
						chapterTitle = string.Concat(
							CHAPTER_TITLE_BEGIN,
							chapterStory.ChapterNumber);
					}
				}

				chapterStory.Title = chapterTitle;

				// - LIKES
				// (contains '(' character if AuthorPage)
				string likeNumberText = storyChapter.SelectSingleNode("div/span")?.InnerText ?? string.Empty;
				string[] splitParenthesis = likeNumberText.Split('(');
				likeNumberText = splitParenthesis.Count() > 1 ? splitParenthesis[1] : likeNumberText;
				long likeNumber = long.Parse(likeNumberText);
				chapterStory.LikesNumber = likeNumber;

				story.ChaptersList.Add(chapterStory);
			}
		}

		private async Task<List<Story>> GetStoriesBase(Uri uri)
		{
			try
			{
				HtmlDocument html = new HtmlDocument();
				html.LoadHtml(await _repositoryWeb.GetHtmlPage(uri.ToString()));

				List<Story> stories = new List<Story>();

				HtmlNode document = html.DocumentNode;
				var storiesContainer = document.SelectNodes(STORIES_XPATH);
				stories = this.GetStoriesFromContainer(storiesContainer);

				return stories;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + Environment.NewLine + ex.InnerException);
			}
			return null;
		}

		private List<Story> GetStoriesFromContainer(HtmlNodeCollection storiesContainer, bool authorPage = false)
		{
			List<Story> stories = new List<Story>();

			if (storiesContainer == null)
			{
				return stories;
			}

			foreach (var container in storiesContainer)
			{
				var categoryNode = container.SelectSingleNode("a[1]");
				var titleNode = container.SelectSingleNode("a[2]");
				var titleMedalNode = container.SelectSingleNode("a[3]");// if story has medal, title node is next 'a' node
				var infosNode = container.SelectSingleNode("div[2]");
				var authorNode = infosNode.SelectSingleNode("div/a");

				Story story = new Story();

				// CategoryUrl
				string categoryUrl = categoryNode.Attributes["href"].Value;
				story.CategoryUrl = categoryUrl;

				//CategoryName
				string categoryName = categoryNode.Attributes["title"].Value.Split('«')[1].Split('»')[0].Trim();
				story.CategoryName = categoryName;

				// Title
				string title = titleNode.Element("h2")?.InnerText
					?? titleMedalNode?.Element("h2")?.InnerText;

				if (string.IsNullOrWhiteSpace(title))
				{
					// If title is still empty, we are in Author stories scenario
					// and want to get the story name without "(<NB> chapitres)"
					var subChaptersSpan = titleNode.SelectSingleNode("span[@class='xs-nb-chapitres']");
					if (subChaptersSpan != null)
					{
						titleNode.Element(subChaptersSpan.Name).Remove();
					}
					title = titleNode.InnerText;
				}

				// Chapter name
				string chapterName = string.Empty;
				if (titleNode.Attributes["title"] != null)
				{
					chapterName = titleNode.Attributes["title"].Value.Contains("«")
						? titleNode.Attributes["title"].Value.Split('«')[1].Split('»')[0].Trim()
						: string.Empty;
				}
				story.Title = title;
				story.ChapterName = chapterName;

				// Url
				string url = titleNode.Attributes["href"]?.Value;
				story.Url = string.Concat(_baseAdress, url);

				// Release date
				string releaseDate = infosNode.Element("time")?.Attributes["datetime"]?.Value ?? string.Empty;
				story.ReleaseDate = releaseDate;


				// Sub chapters (AUTHOR PAGE)
				if (authorPage)
				{
					var subChaptersContainer = container.SelectNodes("ul/li");

					if (subChaptersContainer != null)
					{
						this.GetStorySubChapters(story, subChaptersContainer);
					}
				}
				else
				{
					Author author = new Author();
					author.Id = authorNode.Attributes["data-author-id"].Value;
					author.Url = string.Concat(_baseAdress, authorNode.Attributes["href"].Value);
					author.Name = authorNode.InnerHtml;

					story.Author = author;
				}

				stories.Add(story);
			}

			return stories;
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

		public async Task<string> GetAuthorAvatar(string authorId)
		{
			try
			{
				Uri uri = new Uri(_baseAdress, string.Concat($"ajax.php?action=auteur_infos&id={authorId}"));

				HtmlDocument html = new HtmlDocument();
				var s = await _repositoryWeb.GetHtmlPage(uri.ToString());
				html.LoadHtml(await _repositoryWeb.GetHtmlPage(uri.ToString()));

				HtmlNode document = html.DocumentNode;

				var avatarUri = new Uri(_baseAdress, document.SelectSingleNode("img").Attributes["src"].Value);

				return avatarUri.ToString();
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<List<Story>> GetAuthorStories(string authorPageUrl)
		{
			HtmlDocument html = new HtmlDocument();

			var uri = new Uri(authorPageUrl);

			html.LoadHtml(await _repositoryWeb.GetHtmlPage(uri.ToString()));

			HtmlNode document = html.DocumentNode;
			var storiesContainer = document.SelectNodes(AUTHOR_PAGE_STORIES);

			List<Story> stories = this.GetStoriesFromContainer(storiesContainer, true);

			return stories;
		}
	}
}
