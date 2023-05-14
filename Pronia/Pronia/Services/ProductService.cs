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
                                                                    .FirstOrDefaultAsync(m => m.Id == id);
            return productById;
        }

        public async Task<Product> GetById(int id) => await _context.Products.FindAsync(id);

        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();


        public async Task<List<Product>> GetPaginatedDatas(int page, int take, int? cateId, int? tagId)
        {
            List<Product> products = null;

            if (cateId is null || tagId is null)
            {
                products = await _context.Products.Include(m => m.Images)
                                        .Include(m => m.ProductCategories)
                                        .ThenInclude(m => m.Category)
                                        .Include(m => m.ProductSizes)
                                        .Include(m => m.ProductTags)
                                        .Where(m => !m.SofDelete)
                                        .Skip((page * take) - take).Take(take).ToListAsync();
            }
            else
            {
                products = await _context.ProductCategories.Include(m=>m.Product).ThenInclude(m=>m.Images)
                                                                 .Where(m => m.Category.Id == cateId)
                                                                 .Select(m => m.Product)
                                                                 .Where(m => !m.SofDelete).Skip((page * take) - take).Take(take).ToListAsync();

            }

          
            if(tagId != null)
            {
                products = await _context.ProductTags.Include(m => m.Product).ThenInclude(m => m.Images)
                                                                 .Where(m => m.Tag.Id == tagId)
                                                                 .Select(m => m.Product)
                                                                 .Where(m => !m.SofDelete).Skip((page * take) - take).Take(take).ToListAsync();
            }



            return products;
        }

        public async Task<List<Product>> GetFeaturedProducts() => await _context.Products.Include(m => m.Images).Where(m => !m.SofDelete).OrderByDescending(m => m.Rate).ToListAsync();

        public async Task<List<Product>> GetBestsellerProducts() => await _context.Products.Include(m => m.Images).Where(m => !m.SofDelete).OrderByDescending(m => m.SaleCount).ToListAsync();

        public async Task<List<Product>> GetLatestProducts() => await _context.Products.Include(m => m.Images).Where(m => !m.SofDelete).OrderByDescending(m => m.CreadtedDate).ToListAsync();

        public async Task<List<Product>> GetNewProducts() => await _context.Products.Include(m => m.Images)
                                                                .Include(m => m.ProductCategories)
                                                                .ThenInclude(m => m.Category)
                                                                .Include(m => m.ProductSizes)
                                                                .Include(m => m.ProductTags)
                                                                .Where(m => !m.SofDelete)
                                                                 .OrderByDescending(m => m.CreadtedDate)
                                                                 .Take(4).ToListAsync();

    
    }
}