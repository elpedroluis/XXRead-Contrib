using System.Collections.Generic;
using System.Threading.Tasks;
using XStory.DTO;

namespace XStory.BL.Common.Contracts
{
	public interface IServiceStory
	{
		Task<List<DTO.Story>> InitStories();
		Task<List<DTO.Story>> LoadMoreStories();
		Task<List<DTO.Story>> RefreshStories(DTO.Story firstStory);
		List<DTO.Story> DistinctStories(List<DTO.Story> stories);

		Task<DTO.Story> InitStory();
		DTO.Story GetCurrentStory();
		void SetCurrentStory(DTO.Story story);
		/// <summary>
		/// Add story in cache, if not already present
		/// </summary>
		/// <param name="story"></param>
		void AddAlreadyLoadedStory(Story story);
		DTO.Story GetAlreadyLoadedStory(Story story);

		void SetPageNumber(int value);
		void IncrementPageNumber();
		void ResetPageNumber();

		/* SQLite */

		Task<List<DTO.Story>> GetStoriesSQLite();
		Task<DTO.Story> GetStorySQLite(DTO.Story story);
		Task<int> InsertStorySQLite(DTO.Story story);
		Task<int> DeleteStorySQLite(DTO.Story story);
	}
}