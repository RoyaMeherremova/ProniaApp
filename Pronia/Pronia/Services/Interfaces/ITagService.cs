using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetTagById(int? id);
    }
}
