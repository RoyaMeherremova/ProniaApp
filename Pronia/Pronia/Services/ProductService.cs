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
        public async Task<List<Product>> GetAll() => await _context.Products.Include(m => m.Images)
                                                                            .Include(m => m.ProductCategories)
                                                                            .ThenInclude(m => m.Category)
                                                                            .Include(m => m.ProductSizes)
                                                                            .Include(m => m.ProductTags)                                                                            
                                                                            .ThenInclude(m => m.Tag)
                                                                            .Include(m => m.Color)
                                                                            .Include(m => m.Comments)
                                                                            .Where(m => !m.SofDelete).ToListAsync();

        public async Task<Product> GetFullDataById(int? id) 
        { 
            Product productById = await _context.Products.Include(m => m.Images)
                                                                    .Include(m => m.ProductCategories)
                                                                    .ThenInclude(m => m.Category)
                                                                    .Include(m => m.ProductSizes)
                                                                    .ThenInclude(m => m.Size)
                                                                    .Include(m => m.ProductTags)
                                                                    .ThenInclude(m => m.Tag)
                                                                    .Include(m => m.Color)
                                                                    .Include(m => m.Comments)
                                                                    .FirstOrDefaultAsync(m => m.Id == id);
            return productById;
        } 

        public async Task<Product> GetById(int id) => await _context.Products.FindAsync(id);

        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();
        

        public async Task<List<Product>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Products.Include(m => m.Images)
                                        .Include(m => m.ProductCategories)
                                        .ThenInclude(m => m.Category)
                                        .Include(m => m.ProductSizes)
                                        .Include(m => m.ProductTags)
                                        .Include(m => m.Comments)
                                        .Where(m => !m.SofDelete)
                                        .Skip((page * take) - take).Take(take).ToListAsync();
        }

        public async Task<List<Product>> GetFeaturedProducts() => await _context.Products.Where(m => !m.SofDelete).OrderByDescending(m => m.Rate).ToListAsync();

        public async Task<List<Product>> GetBestsellerProducts() => await _context.Products.Where(m => !m.SofDelete).OrderByDescending(m => m.SaleCount).ToListAsync();

        public async Task<List<Product>> GetLatestProducts() => await _context.Products.Where(m => !m.SofDelete).OrderByDescending(m => m.CreadtedDate).ToListAsync();

        public async Task<List<Product>> GetNewProducts() => await _context.Products.Where(m => !m.SofDelete).OrderByDescending(m => m.CreadtedDate).Take(4).ToListAsync();

    
    }
}