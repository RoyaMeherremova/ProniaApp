using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class AuthorService:IAuthorService
    {
        private readonly AppDbContext _context;

        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAll() => await _context.Authors.Include(m=>m.Blogs).Where(m => !m.SofDelete).ToListAsync();


        public async Task<Author> GetById(int? id)
        {
            return await _context.Authors.Where(m => !m.SofDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
