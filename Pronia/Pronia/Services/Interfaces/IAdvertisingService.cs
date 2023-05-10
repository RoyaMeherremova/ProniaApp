using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IAdvertisingService
    {
        Task<List<Advertising>> GetAll();

        Task<Advertising> GetAdvertisingById(int? id);

    }
}
