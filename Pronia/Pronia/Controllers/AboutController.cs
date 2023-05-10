using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAdvertisingService _advertisingService;

        public AboutController(AppDbContext context,IAdvertisingService advertisingService)
        {
            _context = context;
            _advertisingService = advertisingService;
        }

        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds
                                                                  .AsEnumerable()
                                                                  .ToDictionary(m => m.Key, m => m.Value);
            List<Advertising> advertisings = await _advertisingService.GetAll();
            AboutVM model = new()
            {
               HeaderBackgrounds= headerBackgrounds,
               Advertisings = advertisings

            };
            return View(model);
        }
    }
}
