using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IBannerService
    {
        Task<List<Bannner>> GetAllAsync();
        Task<Bannner> GetBannerById(int? id);
    }
}
