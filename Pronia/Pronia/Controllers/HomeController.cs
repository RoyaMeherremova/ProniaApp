using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISliderService _sliderService;
        private readonly IAdvertisingService _advertisingService;
        private readonly IProductService _productService;
        
        public HomeController(AppDbContext context,
                              ISliderService sliderService, 
                              IAdvertisingService advertisingService, IProductService productService)
        {
            _sliderService = sliderService;
            _advertisingService = advertisingService;
            _context= context;
            _productService= productService;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();
            List<Advertising> advertisings= await _advertisingService.GetAll();
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Product> featuredProducts = await _productService.GetFeaturedProducts();
            List<Product> bestsellerProducts = await _productService.GetBestsellerProducts();
            List<Product> latestProducts = await _productService.GetLatestProducts();
            HomeVM model = new()
            {
                Sliders= sliders,
                Advertising = advertisings,
                HeaderBackgrounds = headerBackgrounds,
                BestsellerProduct= bestsellerProducts,
                FeaturedProduct=featuredProducts,
                LatestProduct= latestProducts,
            };
            return View(model);
        }
    }
}
