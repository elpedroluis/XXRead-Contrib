using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XStory.BL.SQLite.Contracts;
using XStory.DAL.SQLite;
using XStory.DAL.SQLite.Contracts;
using XStory.DTO;

namespace XStory.BL.SQLite
{
	public class ServiceAuthor : IServiceAuthor
	{
		private IRepositoryAuthor _repositoryAuthor;
		public ServiceAuthor(IRepositoryAuthor repositoryAuthor)
		{
			_repositoryAuthor = repositoryAuthor;
		}

		public async Task<List<Author>> GetAuthors()
		{
			return await _repositoryAuthor.GetAuthors();
		}

		public async Task<Author> GetAuthor(string id)
		{
			var author = await _repositoryAuthor.GetAuthor(id);
			return author;
		}

		public async Task<int> InsertAuthor(Author author)
		{
			return await _repositoryAuthor.InsertAuthor(author);
		}

		public async Task<int> DeleteAuthor(Author author)
		{
			return await _repositoryAuthor.DeleteAuthor(author);
		}
	}
}
