using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IColorService _colorService;
        public ShopController(AppDbContext context,
                              ICategoryService categoryService,
                              IProductService productService, 
                              IColorService colorService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _colorService = colorService;
            _context= context;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _categoryService.GetCategories();
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Color> colors = await _colorService.GetColors();
            List<Product> products = await _productService.GetAll();
            ShopVM model = new()
            {
                Categories = categories,
                NewProducts = newProducts,
                Colors = colors,
                HeaderBackgrounds= headerBackgrounds,
                Products = products

            };
            return View(model);
        }


        public async Task<IActionResult> GetProductsByCategory(int? id)
        {
            List<Product> products = await _context.ProductCategories.Where(m => m.Category.Id == id).Select(m=>m.Product).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }
    }
}
