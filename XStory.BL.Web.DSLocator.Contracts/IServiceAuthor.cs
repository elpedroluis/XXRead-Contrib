using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XStory.DTO;

namespace XStory.BL.Web.DSLocator.Contracts
{
	public interface IServiceAuthor
	{
		Task<DTO.Author> GetAuthorPage(string dataSource, Author author, int pagenumber = 1);
	}
}
