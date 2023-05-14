using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services;
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
        private readonly IClientService _clientService;
        private readonly IBrandService _brandService;
        private readonly IBlogService _blogService;
        private readonly IBasketService _basketService;

        public HomeController(AppDbContext context,
                              ISliderService sliderService,
                              IAdvertisingService advertisingService,
                              IProductService productService,
                              IClientService clientService,
                              IBrandService brandService,
                              IBlogService blogService,
                              IBasketService basketService)
        {
            _sliderService = sliderService;
            _advertisingService = advertisingService;
            _context = context;
            _productService = productService;
            _clientService = clientService;
            _brandService = brandService;
            _blogService = blogService;
            _basketService = basketService;


        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();
            List<Advertising> advertisings = await _advertisingService.GetAll();
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Product> featuredProducts = await _productService.GetFeaturedProducts();
            List<Product> bestsellerProducts = await _productService.GetBestsellerProducts();
            List<Product> latestProducts = await _productService.GetLatestProducts();
            List<Bannner> banners = await _context.Bannners.ToListAsync();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Client> clients = await _clientService.GetClients();
            List<Brand> brands = await _brandService.GetBrands();
            List<Blog> blogs = await _blogService.GetBlogs();



            HomeVM model = new()
            {
                Sliders = sliders,
                Advertising = advertisings,
                HeaderBackgrounds = headerBackgrounds,
                BestsellerProduct = bestsellerProducts,
                FeaturedProduct = featuredProducts,
                LatestProduct = latestProducts,
                Banners = banners,
                NewProducts = newProducts,
                Clients = clients,
                Brands = brands,
                Blogs = blogs,



            };
            return View(model);
        }





        //BASKET
        [HttpPost]
        public async Task<IActionResult> AddBasket(int? id) 
        {


            if (id == null) return BadRequest();

            Product dbProduct = await _productService.GetById((int) id);


            if (dbProduct == null) return NotFound();    


            List<BasketVM> basket = _basketService.GetBasketDatas();   

            BasketVM? existProduct = basket.FirstOrDefault(m => m.Id == dbProduct.Id);  


            _basketService.AddProductToBasket(existProduct, dbProduct, basket);

            int basketCount = basket.Sum(m => m.Count);
            return Ok(basketCount);
        }
    }
}

