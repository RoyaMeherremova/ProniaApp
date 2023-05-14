using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class HeaderBackgroundService:IHeaderBackgroundService
    {
        private readonly AppDbContext _context;

        public HeaderBackgroundService(AppDbContext context)
        {
            _context = context;
        }

        public List<HeaderBackground> GetHeaderBackgroundsAsync() => _context.HeaderBackgrounds.Where(m => !m.SofDelete).ToList();
        public async Task<HeaderBackground> GetHeaderBackgroundByIdAsync(int? id) => await _context.HeaderBackgrounds.Where(m => !m.SofDelete).FirstOrDefaultAsync(m => m.Id == id);


    }
}

