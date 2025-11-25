using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XStory.DAL.SQLite.Contracts
{
	public interface IRepositoryAuthorStory
	{
		Task<int> InsertAuthorStory(DTO.Story story);
		Task<int> InsertAuthorStoryTransac(DTO.Story story);
		object GetB(object obj);
		object GetC(object obj);
		object GetD(object obj);
	}
}
