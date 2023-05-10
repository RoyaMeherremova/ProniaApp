using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class AdvertisingService:IAdvertisingService
    {
        private readonly AppDbContext _context;

        public AdvertisingService(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<Advertising> GetAdvertisingById(int? id)=> await _context.Advertisings.Where(m => !m.SofDelete).FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<Advertising>> GetAll()
        {
            return await _context.Advertisings.ToListAsync();
        }
    }
}


