using SQLite;
using System;
using System.Threading.Tasks;
using XStory.DAL.SQLite.Contracts;

namespace XStory.DAL.SQLite
{
	public class Repository
	{
		protected IXXReadDatabase _database;

		public Repository(IXXReadDatabase database)
		{
			_database = database;
		}
	}
}
