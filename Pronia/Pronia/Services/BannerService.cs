using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class BannerService:IBannerService
    {
        private readonly AppDbContext _context;

        public BannerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bannner>> GetAllAsync() => await _context.Bannners.Where(m => !m.SofDelete).ToListAsync();

        public async Task<Bannner> GetBannerById(int? id) => await _context.Bannners.Where(m => !m.SofDelete).FirstOrDefaultAsync(m => m.Id == id);

    }
}
