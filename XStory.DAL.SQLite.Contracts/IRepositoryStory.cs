using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XStory.DAL.SQLite.Contracts
{
    public interface IRepositoryStory
    {
        Task<List<DTO.Story>> GetStories();
        Task<DTO.Story> GetStory(string url);
        Task<int> InsertStory(DTO.Story story);
        Task<int> DeleteStory(DTO.Story story);
    }
}
