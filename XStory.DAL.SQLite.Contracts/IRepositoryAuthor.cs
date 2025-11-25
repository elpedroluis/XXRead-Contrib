using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XStory.DAL.SQLite.Contracts
{
	public interface IRepositoryAuthor
	{
		Task<List<DTO.Author>> GetAuthors();
		Task<DTO.Author> GetAuthor(string id);
		Task<int> InsertAuthor(DTO.Author author);
		Task<int> DeleteAuthor(DTO.Author author);
	}
}
