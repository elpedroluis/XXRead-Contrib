using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XStory.DAL.SQLite.Contracts
{
	public interface IXXReadDatabase
	{
		SQLiteAsyncConnection GetInstance();
	}
}
