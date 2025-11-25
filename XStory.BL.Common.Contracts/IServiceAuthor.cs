using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XStory.BL.Common.Contracts
{
	public interface IServiceAuthor
	{
		Task<DTO.Author> InitAuthor();
		DTO.Author GetCurrentAuthor();
		void SetCurrentAuthor(DTO.Author author);

		/// <summary>
		/// Add story in cache, if not already present
		/// </summary>
		/// <param name="author"></param>
		void AddAlreadyLoadedAuthor(DTO.Author author);
		void UpdateAlreadyLoadedAuthor(DTO.Author author);
		DTO.Author GetAlreadyLoadedAuthor(DTO.Author author);
		Task<DTO.Author> GetAuthorStoriesNextPage(DTO.Author author);
	}
}
