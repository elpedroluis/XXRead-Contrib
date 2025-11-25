using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XStory.DAL.SQLite.Contracts;
using XStory.DAL.SQLite.Mapping;
using XStory.DAL.SQLiteObjects;
using XStory.DTO;
using XStory.Logger;

namespace XStory.DAL.SQLite
{
	public class RepositoryAuthor : Repository, IRepositoryAuthor
	{
		public RepositoryAuthor(IXXReadDatabase database) : base(database)
		{
			_database = database;
		}
		public async Task<List<DTO.Author>> GetAuthors()
		{
			throw new NotImplementedException();
		}

		public async Task<DTO.Author> GetAuthor(string id)
		{
			try
			{
				SQLiteObjects.Author dbAuthor = await _database.GetInstance().GetAsync<SQLiteObjects.Author>(id);

				if (dbAuthor == null)
				{
					throw new Exception($"Could not find specified Author (using id:{id}) in database.");

				}
				return AuthorMapper.ToAuthorDTO(dbAuthor);
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<int> InsertAuthor(DTO.Author author)
		{
			try
			{
				SQLiteObjects.Author authorSQL = Mapping.AuthorMapper.ToAuthorSQLite(author);

				return await _database.GetInstance().InsertAsync(authorSQL);
			}
			catch (Exception ex)
			{
				ServiceLog.Error(ex);
				return -1;
			}
		}

		public async Task<int> DeleteAuthor(DTO.Author author)
		{
			return await _database.GetInstance().DeleteAsync(author.Id);
		}
	}
}
