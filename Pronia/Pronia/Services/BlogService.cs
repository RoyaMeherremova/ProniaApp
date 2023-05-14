using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;

        public BlogService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<int> GetCountAsync() => await _context.Blogs.CountAsync();

        public async Task<List<Blog>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Blogs.Include(m => m.Images)
                                       .Include(m => m.Author)
                                        .Where(m => !m.SofDelete)
                                        .Skip((page * take) - take).Take(take).ToListAsync();
        }



        public async Task<List<Blog>> GetBlogs() => await _context.Blogs.Where(m => !m.SofDelete)
                                                                   .Include(m => m.Images)
                                                                   .Include(m => m.Author)
                                                                   .ToListAsync();

        public async Task<Blog> GetBlogdById(int? id) => await _context.Blogs.Include(m => m.Images)
                                                                            .Include(m => m.Author)
                                                                            .Where(m => !m.SofDelete)
                                                                            .FirstOrDefaultAsync(m => m.Id == id);
     
    }
}


