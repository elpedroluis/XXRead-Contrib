using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XStory.BL.SQLite.Contracts
{
    public interface IServiceStory
    {
        Task<List<DTO.Story>> GetStories();
        Task<DTO.Story> GetStory(string url);
        Task<int> InsertStoryItem(DTO.Story story);

        /// <summary>
        /// Inserts in a transaction : 
        /// 1. Story
        /// 2. AuthorStory
        /// 3. Author
        /// --- Rollback transaction if error.
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        Task<bool> InsertStoryWithAuthorTransac(DTO.Story story);
        Task<int> DeleteStory(DTO.Story story);
    }
}
