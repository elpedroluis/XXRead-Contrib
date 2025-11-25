using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStory.BL.Web.HDS;
using XStory.BL.Web.HDS.Contracts;
using XStory.DTO;

namespace XStory.XUnitTests.HDS
{
	[TestClass]
	public class AuthorTests
	{
		[TestMethod]
		public void GetAuthorPageTest_OK()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "Janea",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=Janea"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.IsNotNull(result.Id);
		}

		[TestMethod]
		public void GetAuthorPageTest_OK2()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "Musty",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=Musty"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.IsNotNull(result.Id);
		}

		[TestMethod]
		public void GetAuthorPageTest_OK3()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "Abies",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=Abies"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.IsNotNull(result.Id);
		}

		[TestMethod]
		public void GetAuthorPageTest_FullInfos_OK()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Id = "215478",
				Name = "Lolo069",
				Avatar = "https://www.xstory-fr.com/forum/img/avatars/215478.png",
				Url = "https://www.xstory-fr.com/auteur,215478,Lolo069.html"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.IsNotNull(result.Id);
		}

		[TestMethod]
		public void GetAuthorPageTest_NoStories_OK()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Id = "220448",
				Name = "Skell Dan",
				Avatar = "https://www.xstory-fr.com/forum/img/avatars/220448.png",
				Url = "https://www.xstory-fr.com/auteur,220488,Skell-Dan.html"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.AreEqual(0, result.Stories.Count);
		}

		[TestMethod]
		public void GetAuthorPageTest_Stories_OK()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "Janea",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=Janea"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.AreNotEqual(result.Stories.Count, 0);
		}

		[TestMethod]
		public void GetAuthorPageTest_Stories2_OK()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "CHRIS71",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=CHRIS71"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.AreNotEqual(result.Stories.Count, 0);
		}

		[TestMethod]
		public void GetAuthorPageTest_Stories3_OK()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "GM34280",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=GM34280"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.AreNotEqual(result.Stories.Count, 0);
		}

		[TestMethod]
		public void GetAuthorPageTestNoInfos_OK()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "Janea",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=Janea"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.IsNotNull(result.Url);
		}

		[TestMethod]
		public void GetAuthorPageHasMorePages_OK()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "Nico T",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=Nico%20T"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.IsTrue(result.HasMorePages);
		}

		[TestMethod]
		public void GetAuthorPageHasMorePages_KO_IsLastPage()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "Nico T",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=Nico%20T&p=7"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.IsFalse(result.HasMorePages);
		}

		[TestMethod]
		public void GetAuthorPageHasMorePages_KO_NoPagination()
		{
			IServiceAuthor _serviceAuthor = new ServiceAuthor(new DAL.Web.HDS.RepositoryWebHDS(), new BL.Web.HDS.ServiceStory());

			Author author = new Author()
			{
				Name = "Janea",
				Url = "https://www.histoires-de-sexe.net/fiche.php?auteur=Janea"
			};

			Task<Author> task = _serviceAuthor.GetAuthorPage(author);
			var result = task.Result;

			Assert.IsFalse(result.HasMorePages);
		}
	}
}
