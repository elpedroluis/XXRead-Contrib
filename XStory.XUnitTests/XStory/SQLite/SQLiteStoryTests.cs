using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStory.DAL.SQLite;

namespace XStory.XUnitTests.XStory.SQLite
{
	[TestClass]
	public class SQLiteStoryTests
	{

		[TestMethod]
		public void GetStoriesTest_OK()
		{
			DAL.SQLite.Contracts.IXXReadDatabase _database = new XXReadDatabase();

			BL.SQLite.Contracts.IServiceStory _serviceStory = new BL.SQLite.ServiceStory(new RepositoryStory(_database));

			Task<List<DTO.Story>> task = _serviceStory.GetStories();
			var result = task.Result;

			Assert.AreNotEqual(null, result);
			Assert.AreNotEqual(0, result.Count);
		}

		[TestMethod]
		public void GetStoryTest_OK()
		{
			DAL.SQLite.Contracts.IXXReadDatabase _database = new XXReadDatabase();

			BL.SQLite.Contracts.IServiceStory _serviceStory = new BL.SQLite.ServiceStory(new RepositoryStory(_database));

			var fakeStory = InitFakeDTOStory();

			Task<DTO.Story> task = _serviceStory.GetStory(fakeStory.Url);
			var story = task.Result;

			Assert.IsNotNull(story);
			Assert.AreEqual(fakeStory.Url, story.Url);
		}

		[TestMethod]
		public void InsertStoryTest_OK()
		{
			DAL.SQLite.Contracts.IXXReadDatabase _database = new XXReadDatabase();

			BL.SQLite.Contracts.IServiceStory _serviceStory = new BL.SQLite.ServiceStory(new RepositoryStory(_database));

			DTO.Story storyDTO = InitFakeDTOStory();

			Task<int> task = _serviceStory.InsertStoryItem(storyDTO);
			var result = task.Result;

			Assert.AreNotEqual(0, result);
		}

		[TestMethod]
		public void DeleteStoryTest_OK()
		{
			DAL.SQLite.Contracts.IXXReadDatabase _database = new XXReadDatabase();

			BL.SQLite.Contracts.IServiceStory _serviceStory = new BL.SQLite.ServiceStory(new RepositoryStory(_database));

			DTO.Story storyDTO = InitFakeDTOStory();

			Task<int> task = _serviceStory.DeleteStory(storyDTO);
			var result = task.Result;

			Assert.AreNotEqual(0, result);
		}

		[TestMethod]
		public void InsertStoryWithAuthorTest_UsingTransaction_OK()
		{
			DAL.SQLite.Contracts.IXXReadDatabase _database = new XXReadDatabase();

			BL.SQLite.Contracts.IServiceStory _serviceStory =
				new BL.SQLite.ServiceStory(
					new RepositoryStory(_database),
					new RepositoryAuthorStory(_database));

			var fakeStory = InitFakeDTOStory2();

			Task<bool> task = _serviceStory.InsertStoryWithAuthorTransac(fakeStory);
			var result = task.Result;

			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public void InsertStoryWithAuthorTest_UsingTransaction_ERROR()
		{
			DAL.SQLite.Contracts.IXXReadDatabase _database = new XXReadDatabase();

			BL.SQLite.Contracts.IServiceStory _serviceStory =
				new BL.SQLite.ServiceStory(
					new RepositoryStory(_database),
					new RepositoryAuthorStory(_database));

			var fakeStory = InitFakeDTOStory2();

			Task<bool> task = _serviceStory.InsertStoryWithAuthorTransac(fakeStory);
			var result = task.Result;

			Assert.AreEqual(false, result);
		}

		private DTO.Story InitFakeDTOStory()
		{
			return new DTO.Story()
			{
				Author = new DTO.Author()
				{
					Id = "214762",
					Name = "Simia31",
					Url = "auteur,214762,Simia31.html"
				},
				CategoryName = "Hétéro",
				CategoryUrl = "histoires-erotiques,h%E9t%E9ro,1.html",
				ChapterName = "Chapitre 1",
				ChapterNumber = 1,
				ChaptersList = null,
				Content = @"\r\nMa femme est une chienne. \r\n\r\n\r\nLe titre de cette nouvelle peut paraître racoleur, pourtant il n’en est rien. J’ai le bonheur de partager ma vie avec une véritable déesse du sexe, une magnifique femme qui respire et inspire le sexe, sous toutes ses formes. \r\n\r\n\r\nNotre rencontre fut des plus classiques, nous partagions les mêmes cours à la fac et j’ai pris contact avec elle via un réseau social. Deux trois messages, quelques allusions bien senties, et j’étais chez elle le soir même, à lécher sa chatte trempée avant de la baiser une bonne partie de la nuit. Notre histoire a commencé comme cela, et si nous nous complétons sur bien des points, c’est bien notre amour réciproque pour le sexe qui nous a rapprochés aussi rapidement. Nous passions toutes nos journées à baiser, tant et si bien que nous ne parlions pas des masses au début, tant sa bouche était occupée par ma queue.\r\n\r\n\r\nSon corps est un bonheur, elle est élancée, brune, avec une paire de seins percée, ferme et délicieuse à croquer. Son cul est rebondi, son sourire ravageur, et elle a un talent inné pour bouger lorsqu’elle se met à califourchon sur ma queue dressée. Ses gémissements sont un bonheur pour mes oreilles, et elle adore lorsque je la remplis de mon foutre. Son cul est presque aussi accueillant que sa chatte, tant et si bien que parfois je ne sais que choisir. Alors je la baise par les deux trous, de toute façon elle en raffole. \r\n\r\n\r\nMais ce n’est pas pour cela que j’aime autant le sexe avec elle, c’est, car en réalité ma femme est une véritable salope, une chienne, une pute qui adore la queue et l’assume. J’ai eu des expériences avec des femmes par le passé, mais elle, c’est autre chose. Elle assume d’être une chienne, en redemande à chaque fois, et s’abandonne à moi lors de chaque baise. Son corps est ma propriété, et c’est ça qui l’excite au plus haut point, elle m’appartient, et je la baise quand je veux. Je ne compte pas le nombre d’endroits où cette pétasse a fait glisser ma queue dans sa gorge, à me sucer jusqu’à lui remplir la bouche de mon foutre. Les trajets en voiture, en pleine nature, dans une cabine d’essayage, dans un couloir, un ascenseur, et j’en passe. Pourtant, je crois que ce que je préfère, c’est quand cette pute me donne accès à ses orifices, elle aime aussi bien se faire baiser que se faire enculer, et c’est un bonheur, vous pouvez me croire. \r\n\r\n\r\nJ’aime l’enculer dans un coin perdu, contre notre voiture, je vois sa chatte s’écarter et se lubrifier pendant que ma queue entre et sort de son cul, comme si elle voulait se faire remplir elle aussi, alors j’y enfonce mes doigts, je caresse son point g pendant que je continue de la baiser. Nos balades dominicales sont souvent des baises mémorables. \r\n\r\n\r\nJ’aime sentir son anus se resserrer sur ma queue, cela me donne envie de la sodomiser encore plus fort, elle aime ça autant que moi. Elle raffole de mon sperme, j’ai souillé toutes les parties de son corps, de son visage jusqu’à ses pieds. J’aime quand elle pisse devant moi et me donne sa chatte juste après, ça m’excite encore plus de pénétrer sa chatte chaude et trempée. Ses orgasmes sont puissants, sont bestiaux, et j’aime quand elle se donne totalement à moi. On se filme souvent pendant l’acte, des vidéos rudimentaires certes, mais on s’amuse souvent à les regarder et à se demander comment elle peut mouiller autant pendant l’acte. Parfois quand on baise en pleine nature, certains hommes s’arrêtent pas loin de nous, ils se branlent et on s’en moque, au contraire. On baise comme s’ils n’étaient pas là, ça nous plaît et je crois que ça leur plaît aussi. \r\n\r\n\r\nOn aimerait trouver une autre femme, afin de s’amuser à trois, mais nous sommes compliqués, et trouver quelqu’un aimant autant le sexe que nous est tout de même compliqué. Je ne dirai pas que notre sexualité est dépravée, mais débridée, on baise de partout, et surtout quand on en a envie. Peu importe le moment, ou l’endroit. Elle porte très rarement des sous-vêtements, c’est une perte de temps, et j’aime quand elle mouille sous ses robes, elle est encore plus simple à baiser. \r\n\r\n\r\nJ’ai le souvenir d’une partie de jambes en l’air qui reste, à cette heure, la meilleure baise de ma vie. Ma douce et moi-même nous retrouvions après quasiment un mois d’absence, et il nous a fallu seulement 5 minutes pour finir à poil et baptiser les nouveaux canapés de mes parents. Après qu’elle m’ait gâté de sa bouche pendant de longues minutes, j’ai fait de même en me délectant de sa mouille et de son léger goût salé afin de bien l’ouvrir et la lubrifier. S’en est suivi une baise rapide, mais sauvage, ses « fais de moi ta chienne » résonnent encore dans mes oreilles, et me donnent envie de la baiser une fois de plus. J’ai commencé par visiter sa chatte en tapant au fond comme elle aime, ses fesses devenaient rouges à force de les fesser et des claques s’abattaient sur son visage. Après l’avoir prise en missionnaire, je l’ai mise sur le ventre pour bien la pénétrer et stimuler son point g. \r\n\r\n\r\nJ’ai fini par éjaculer sur son anus et sa chatte, pendant qu’elle jouissait après cette baise intense. \r\n\r\n\r\nJe vous souhaite de trouver chaussure à votre pied comme j’ai trouvé au mien. Je reviendrai peut-être avec une autre nouvelle, si le c\u009cur nous en dit, et surtout si nous avons d’autres choses à vous partager. Merci de votre lecture.\r\n\r\n",
				Id = null,
				LikesNumber = 241,
				ReleaseDate = "2022-08-05 15:33",
				Reviews = null,
				ReviewsNumber = 6,
				Title = "Ma femme est une chienne",
				Type = "Histoire vraie",
				Url = @"https://www.xstory-fr.com/lire-histoire,femme-est-une-chienne,53291.html",
				ViewsNumber = 21157
			};
		}

		private DTO.Story InitFakeDTOStory2()
		{
			return new DTO.Story()
			{
				Author = new DTO.Author()
				{
					Id = "203770",
					Name = "beau gosse 35 ans",
					Url = "auteur,203770,beau-gosse-35-ans.html"
				},
				CategoryName = "Hétéro",
				CategoryUrl = "histoires-erotiques,h%E9t%E9ro,1.html",
				ChapterName = "Chapitre 2",
				ChapterNumber = 2,
				ChaptersList = null,
				Content = @"\r\nSimon reste tétanisé. Cette superbe femme qui s’offre à lui est évidemment tentante mais c’est aussi l’usurpatrice, celle qui lui a barré la route, celle avec qui il a juré de ne pas sympathiser. Il pose son plateau et dit froidement: \r\n\r\n\r\n-Je vais vous demander de vous rhabiller et de partir! \r\n\r\n\r\n-Je vais mettre les choses au point Simon. Je sais que vous vouliez ce poste. Mais je vous jure que je ne le dois qu’à mes états de service. Si vous en doutez, je peux vous montrer mon dossier. Maintenant, si comme on me l’a dit, vous faites un bon boulot, je ferai de bons rapports sur vous et vous montrez en grade.\r\n\r\n\r\nSon corps est un bonheur, elle est élancée, brune, avec une paire de seins percée, ferme et délicieuse à croquer. Son cul est rebondi, son sourire ravageur, et elle a un talent inné pour bouger lorsqu’elle se met à califourchon sur ma queue dressée. Ses gémissements sont un bonheur pour mes oreilles, et elle adore lorsque je la remplis de mon foutre. Son cul est presque aussi accueillant que sa chatte, tant et si bien que parfois je ne sais que choisir. Alors je la baise par les deux trous, de toute façon elle en raffole. \r\n\r\n\r\nMais ce n’est pas pour cela que j’aime autant le sexe avec elle, c’est, car en réalité ma femme est une véritable salope, une chienne, une pute qui adore la queue et l’assume. J’ai eu des expériences avec des femmes par le passé, mais elle, c’est autre chose. Elle assume d’être une chienne, en redemande à chaque fois, et s’abandonne à moi lors de chaque baise. Son corps est ma propriété, et c’est ça qui l’excite au plus haut point, elle m’appartient, et je la baise quand je veux. Je ne compte pas le nombre d’endroits où cette pétasse a fait glisser ma queue dans sa gorge, à me sucer jusqu’à lui remplir la bouche de mon foutre. Les trajets en voiture, en pleine nature, dans une cabine d’essayage, dans un couloir, un ascenseur, et j’en passe. Pourtant, je crois que ce que je préfère, c’est quand cette pute me donne accès à ses orifices, elle aime aussi bien se faire baiser que se faire enculer, et c’est un bonheur, vous pouvez me croire. \r\n\r\n\r\nJ’aime l’enculer dans un coin perdu, contre notre voiture, je vois sa chatte s’écarter et se lubrifier pendant que ma queue entre et sort de son cul, comme si elle voulait se faire remplir elle aussi, alors j’y enfonce mes doigts, je caresse son point g pendant que je continue de la baiser. Nos balades dominicales sont souvent des baises mémorables. \r\n\r\n\r\nJ’aime sentir son anus se resserrer sur ma queue, cela me donne envie de la sodomiser encore plus fort, elle aime ça autant que moi. Elle raffole de mon sperme, j’ai souillé toutes les parties de son corps, de son visage jusqu’à ses pieds. J’aime quand elle pisse devant moi et me donne sa chatte juste après, ça m’excite encore plus de pénétrer sa chatte chaude et trempée. Ses orgasmes sont puissants, sont bestiaux, et j’aime quand elle se donne totalement à moi. On se filme souvent pendant l’acte, des vidéos rudimentaires certes, mais on s’amuse souvent à les regarder et à se demander comment elle peut mouiller autant pendant l’acte. Parfois quand on baise en pleine nature, certains hommes s’arrêtent pas loin de nous, ils se branlent et on s’en moque, au contraire. On baise comme s’ils n’étaient pas là, ça nous plaît et je crois que ça leur plaît aussi. \r\n\r\n\r\nOn aimerait trouver une autre femme, afin de s’amuser à trois, mais nous sommes compliqués, et trouver quelqu’un aimant autant le sexe que nous est tout de même compliqué. Je ne dirai pas que notre sexualité est dépravée, mais débridée, on baise de partout, et surtout quand on en a envie. Peu importe le moment, ou l’endroit. Elle porte très rarement des sous-vêtements, c’est une perte de temps, et j’aime quand elle mouille sous ses robes, elle est encore plus simple à baiser. \r\n\r\n\r\nJ’ai le souvenir d’une partie de jambes en l’air qui reste, à cette heure, la meilleure baise de ma vie. Ma douce et moi-même nous retrouvions après quasiment un mois d’absence, et il nous a fallu seulement 5 minutes pour finir à poil et baptiser les nouveaux canapés de mes parents. Après qu’elle m’ait gâté de sa bouche pendant de longues minutes, j’ai fait de même en me délectant de sa mouille et de son léger goût salé afin de bien l’ouvrir et la lubrifier. S’en est suivi une baise rapide, mais sauvage, ses « fais de moi ta chienne » résonnent encore dans mes oreilles, et me donnent envie de la baiser une fois de plus. J’ai commencé par visiter sa chatte en tapant au fond comme elle aime, ses fesses devenaient rouges à force de les fesser et des claques s’abattaient sur son visage. Après l’avoir prise en missionnaire, je l’ai mise sur le ventre pour bien la pénétrer et stimuler son point g. \r\n\r\n\r\nJ’ai fini par éjaculer sur son anus et sa chatte, pendant qu’elle jouissait après cette baise intense. \r\n\r\n\r\nJe vous souhaite de trouver chaussure à votre pied comme j’ai trouvé au mien. Je reviendrai peut-être avec une autre nouvelle, si le c\u009cur nous en dit, et surtout si nous avons d’autres choses à vous partager. Merci de votre lecture.\r\n\r\n",
				Id = null,
				LikesNumber = 195,
				ReleaseDate = "2022-09-07 09:40",
				Reviews = null,
				ReviewsNumber = 1,
				Title = " Brigade du stupre ",
				Type = "Fantasme",
				Url = @"https://www.xstory-fr.com/lire-histoire,brigade-stupre,53153.html",
				ViewsNumber = 3310
			};
		}
	}
}
