using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;

        public SliderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Slider>> GetAll() => await _context.Sliders.Where(m => !m.SofDelete).ToListAsync();

        public async Task<Slider> GetSliderById(int? id)=> await _context.Sliders.Where(m => !m.SofDelete).FirstOrDefaultAsync(m=>m.Id==id);
   
    }
}
