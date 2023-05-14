using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class SettingService:ISettingService
    {
        private readonly AppDbContext _context;

        public SettingService(AppDbContext context)
        {
            _context = context;
        }

        public List<Settings> GetSettingsAsync() => _context.Settings.Where(m => !m.SofDelete).ToList();
        public async Task<Settings> GetSettingByIdAsync(int? id) => await _context.Settings.Where(m => !m.SofDelete).FirstOrDefaultAsync(m => m.Id == id);

    }
}
