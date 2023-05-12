using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class SizeService:ISizeService
    {
        private readonly AppDbContext _context;
        public SizeService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Size>> GetAllSize()
        {
            return await _context.Sizes.Include(m => m.ProductSizes).Where(m => !m.SofDelete).ToListAsync();
        }

        public async Task<Size> GetById(int? id)
        {
            return await _context.Sizes.Where(m => !m.SofDelete).FirstOrDefaultAsync(m => m.Id == id);
        }


    }
}
