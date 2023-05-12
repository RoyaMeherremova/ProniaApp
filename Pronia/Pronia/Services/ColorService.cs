using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using System.Drawing;
using Color = Pronia.Models.Color;

namespace Pronia.Services
{
    public class ColorService : IColorService
    {
        private readonly AppDbContext _context;

        public ColorService(AppDbContext context)
        {
            _context = context;
        }
     
        public async Task<List<Color>> GetColors() => await _context.Colors.Include(m=>m.Products).Where(m => !m.SofDelete).ToListAsync();

        public async Task<Color> GetColorById(int? id) => await _context.Colors.Where(m => !m.SofDelete).FirstOrDefaultAsync(m => m.Id == id);
    }
}
