using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<Product> GetFullDataById(int? id);
        Task<Product> GetById(int id);
        Task<int> GetCountAsync();

        Task<List<Product>> GetFeaturedProducts();

        Task<List<Product>> GetBestsellerProducts();
        Task<List<Product>> GetLatestProducts();
        Task<List<Product>> GetNewProducts();

         Task<List<Product>> GetPaginatedDatas(int page, int take, int? cateId);




    }
}
