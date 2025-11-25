using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XStory.BL.SQLite.Contracts
{
	public interface IServiceAuthor
	{
		Task<List<DTO.Author>> GetAuthors();
		Task<DTO.Author> GetAuthor(string id);
		Task<int> InsertAuthor(DTO.Author author);
		Task<int> DeleteAuthor(DTO.Author author);
	}
}
