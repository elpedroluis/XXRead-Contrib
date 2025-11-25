using System;
using System.Collections.Generic;
using System.Text;
using XStory.DAL.SQLite.Contracts;

namespace XStory.DAL.SQLite
{
	internal class RepositorySettings : Repository
	{
		public RepositorySettings(IXXReadDatabase database) : base(database)
		{
			_database = database;
		}
	}
}
