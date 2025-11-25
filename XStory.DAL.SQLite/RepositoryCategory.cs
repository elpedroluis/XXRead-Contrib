using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XStory.DAL.SQLite.Contracts;
using XStory.DAL.SQLiteObjects;

namespace XStory.DAL.SQLite
{
	public class RepositoryCategory : Repository, IRepositoryCategory
	{

		public RepositoryCategory(IXXReadDatabase database) : base(database)
		{
			_database = database;
		}

		public async Task<List<Category>> GetCategories()
		{
			try
			{
				return await _database.GetInstance().Table<Category>().ToListAsync();
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<List<Category>> GetCategoriesFromSource(string source)
		{
			try
			{
				return await _database.GetInstance().Table<Category>()
					.Where(c => c.Source == source)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<List<Category>> GetCategories(AsyncTableQuery<Category> query)
		{
			try
			{
				throw new NotImplementedException();
				//return await SQLConnection.QueryAsync().Table<Category>()..Where(query).ToListAsync();
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<Category> GetCategory(string source, string url)
		{
			try
			{
				return await _database.GetInstance().Table<Category>()
					.Where(c => c.Source == source)
					.FirstOrDefaultAsync(c => c.Url == url);
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}

		public async Task<int> Save(Category category)
		{
			try
			{
				return await _database.GetInstance().InsertOrReplaceAsync(category);
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return -1;
			}
		}

		public async Task<int> InsertCategory(Category category)
		{
			try
			{
				return await _database.GetInstance().InsertAsync(category);
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return -1;
			}
		}

		public async Task<int> InsertCategories(List<Category> categories)
		{
			try
			{
				return await _database.GetInstance().InsertAllAsync(categories, true);
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return -1;
			}
		}
	}
}
