using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XStory.BL.Web.XStory.Contracts;
using XStory.DAL.Web.XStory;
using XStory.DAL.Web.XStory.Contracts;
using XStory.DTO;
using XStory.Logger;

namespace XStory.BL.Web.XStory
{
	public class ServiceAuthor : IServiceAuthor
	{
		private IRepositoryWebXStory _repositoryWeb;

		public const string AUTHOR_INFOS_XPATH = "/html/body/div[1]/main/div[2]/section[1]/div/ul/li";
		public const string AUTHOR_STORIES_XPATH = "/html/body/div[1]/main/div[2]/section[2]/div/ul/li";

		public ServiceAuthor()
		{
			_repositoryWeb = new RepositoryWebXStory();
		}

		public async Task<Author> GetAuthorPage(Author author)
		{
			try
			{
				var uri = new Uri(author.Url);

				HtmlDocument html = new HtmlDocument();
				html.LoadHtml(await _repositoryWeb.GetHtmlPage(uri.ToString()));

				HtmlNode document = html.DocumentNode;

				var authorInfosContainer = document.SelectNodes(AUTHOR_INFOS_XPATH);
				var authorStoriesContainer = document.SelectNodes(AUTHOR_STORIES_XPATH);

				// Infos
				this.GetAuthorInfos(author, authorInfosContainer);

				// Is certified ?
				this.GetAuthorCertified(author, document);

				// Stories
				await this.GetAuthorStories(author, authorStoriesContainer);

				return author;
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return author;
			}
		}

		private void GetAuthorCertified(Author author, HtmlNode document)
		{
			var certified = document.SelectSingleNode("//div[@id='badge']");

			author.IsCertified = certified != null;
		}

		/// <summary>
		/// Gets the stories written by the author. And for each, sets Story.Author from Author
		/// </summary>
		/// <param name="author"></param>
		/// <param name="authorStoriesContainer"></param>
		/// <returns></returns>
		private async Task GetAuthorStories(Author author, HtmlNodeCollection authorStoriesContainer)
		{
			IServiceStory serviceStory = new ServiceStory();

			author.Stories = await serviceStory.GetAuthorStories(author.Url);

			// and sets Author value for each Story item
			if (author.Stories != null)
			{
				foreach (var story in author.Stories)
				{
					story.Author = author;
				}
			}
		}

		private void GetAuthorInfos(Author author, HtmlNodeCollection authorInfosContainer)
		{
			foreach (var liNode in authorInfosContainer)
			{
				switch (liNode.SelectSingleNode("label").InnerText.Trim())
				{
					case "Statut:":
						string status = liNode.SelectSingleNode("div")?.InnerText ?? null;
						author.Status = status;
						break;

					case "Classement auteur:":
						string rank = liNode.SelectSingleNode("div")?.InnerText;
						if (string.IsNullOrWhiteSpace(rank)) break;

						// Rank 30 days
						string rank30Days = rank.Split('(')[0].Trim();
						author.Rank30Days = rank30Days;

						// Rank all time
						string rankAllTime = rank
							.Split(new string[] { "classement" }, StringSplitOptions.RemoveEmptyEntries)[1]
							.Split('(')[0].Trim();
						author.RankAllTime = rankAllTime;
						break;

					case "Suivi par:":
						string followedBy = string.Concat(
							liNode.SelectSingleNode("div")?.InnerText ?? null,
							" ",
							liNode.SelectSingleNode("div[3]")?.InnerText ?? null);
						author.FollowedBy = string.IsNullOrWhiteSpace(followedBy) ? null : followedBy;
						break;

					case "Date d'inscription:":
						string registerDate = liNode.SelectSingleNode("div")?.InnerText?
					.Split(' ')[1] ?? null;
						author.RegisterDate = registerDate;
						break;

					case "Genre:":
						string gender = liNode.SelectSingleNode("div")?.InnerText
										?? liNode.SelectSingleNode("div/i").InnerText
										?? null;
						author.Gender = gender;
						break;

					case "Âge:":
						string age = liNode.SelectSingleNode("div")?.InnerText
									 ?? liNode.SelectSingleNode("div/i")?.InnerText
									 ?? null;
						author.Age = age;
						break;

					case "Lieu:":
						string location = liNode.SelectSingleNode("div/a")?.InnerText
										  ?? liNode.SelectSingleNode("div/i")?.InnerText
										  ?? null;
						author.Location = location;
						break;

					default:
						break;
				}
			}
		}
	}
}
