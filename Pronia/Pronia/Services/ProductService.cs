using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAll() => await _context.Products.Include(m => m.Images).Where(m => !m.SofDelete).ToListAsync();

        public async Task<Product> GetFullDataById(int id) => await _context.Products.Include(m => m.Images)
                                                                                     .Include(m => m.ProductCategories)
                                                                                     .Include(m => m.ProductSizes)
                                                                                     .Include(m => m.ProductTags)
                                                                                     .Include(m => m.Comments)
                                                                                     .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<Product> GetById(int id) => await _context.Products.FindAsync(id);

        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();


        public async Task<List<Product>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Products.Include(m => m.Images)
                                          .Include(m => m.ProductCategories)
                                          .Include(m => m.ProductSizes)
                                          .Include(m => m.ProductTags)
                                          .Include(m => m.Comments)
                                          .Skip((page * take) - take).Take(take).ToListAsync();
        }

        public async Task<List<Product>> GetFeaturedProducts() => await _context.Products.Where(m => !m.SofDelete).OrderByDescending(m => m.Rate).ToListAsync();

        public async Task<List<Product>> GetBestsellerProducts() => await _context.Products.Where(m => !m.SofDelete).OrderByDescending(m => m.SaleCount).ToListAsync();

        public async Task<List<Product>> GetLatestProducts() => await _context.Products.Where(m => !m.SofDelete).OrderByDescending(m => m.CreadtedDate).ToListAsync();

        public async Task<List<Product>> GetNewProducts() => await _context.Products.Where(m => !m.SofDelete).OrderByDescending(m => m.CreadtedDate).Take(4).ToListAsync();

    }
}