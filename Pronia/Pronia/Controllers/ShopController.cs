using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Helpers;
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
        private readonly ITagService _tagService;
        private readonly IAdvertisingService _advertisingService;
        public ShopController(AppDbContext context,
                              ICategoryService categoryService,
                              IProductService productService,
                              IColorService colorService,
                              ITagService tagService,
                              IAdvertisingService advertisingService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _colorService = colorService;
            _context = context;
            _tagService = tagService;
            _advertisingService = advertisingService;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 5, int? cateId = null, int? tagId = null)
        {

            List<Product> paginateProducts = await _productService.GetPaginatedDatas(page, take, cateId,tagId);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Product> paginatedDatas = new(paginateProducts, page, pageCount);
           
            List<Category> categories = await _categoryService.GetCategories();
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Color> colors = await _colorService.GetColors();
            List<Tag> tags = await _tagService.GetAllAsync();
            ShopVM model = new()
            {
                Categories = categories,
                NewProducts = newProducts,
                Colors = colors,
                HeaderBackgrounds = headerBackgrounds,
                PaginatedDatas = paginatedDatas,
                Tags= tags

            };
            return View(model);
        }


        //PRODUCT BY CATEGORY
        public async Task<IActionResult> GetProductsByCategory(int? id)
        {
            List<Product> products = await _context.ProductCategories.Include(pc=>pc.Product).ThenInclude(p=>p.Images).Where(m => m.Category.Id == id).Select(m => m.Product).ToListAsync();
            return PartialView("_ProductsPartial", products);
        }

        //GET ALL PRODUCTS
        public async Task<IActionResult> GetAllProducts()
        {
            List<Product> products = await _productService.GetAll();
            return PartialView("_ProductsPartial", products);
        }

        //PRODUCT BY COLOR
        public async Task<IActionResult> GetProductsByColor(int? id)
        {
            List<Product> products = await _context.Products.Include(m => m.Color).Where(m => m.Color.Id == id).ToListAsync();
            return PartialView("_ProductsPartial", products);
        }

        private async Task<int> GetPageCountAsync(int take)  
        {
            var productCount = await _productService.GetCountAsync();  
            return (int)Math.Ceiling((decimal)productCount / take);  
        }


        //SEARCH


        public async Task<IActionResult> Search(string searchText)
        {
            List<Product> products = await _context.Products.Include(m => m.Images)
                                            .Include(m => m.ProductCategories)
                                            .Include(m => m.ProductSizes)
                                            .Include(m => m.ProductTags)
                                            .Include(m => m.Comments)
                                            .Where(m => m.Name.ToLower().Contains(searchText.ToLower()))
                                            .Take(5)
                                            .ToListAsync();
            return PartialView("_SearchPartial", products);
        }


        public async Task<IActionResult> ProductDetail(int? id)
       
        {

            Product product = await _productService.GetFullDataById((int)id);
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Advertising> advertisings = await _advertisingService.GetAll();
            List<Category> categories = await _categoryService.GetCategories();
            List<Product> relatedproducts = new();
            foreach(var category in categories)
            {
              
                
                    Product reProduct = await _context.ProductCategories.Where(m=>m.Category.Id==category.Id).Select(m => m.Product).FirstAsync();
                    relatedproducts.Add(reProduct);
                
            }
            ProductDetailVM model = new()
            {
                HeaderBackgrounds = headerBackgrounds,
                ProductDetail = product,
                Advertisings = advertisings,
                RelatedProducts = relatedproducts
            };
            return View(model);
        }

        public IActionResult MainSearch(string searchText)
        {
            var products = _context.Products
                                .Include(m => m.Images)
                                .Include(m => m.ProductCategories)?
                                .OrderByDescending(m => m.Id)
                                .Where(m => !m.SofDelete && m.Name.ToLower().Trim().Contains(searchText.ToLower().Trim()))
                                .Take(6)
                                .ToList();

            return View(products);
        }

        public async Task<IActionResult> GetProductsByTag(int? id)
        {
            List<Product> products = await _context.ProductTags.Include(pc => pc.Product).ThenInclude(p => p.Images).Where(m => m.Tag.Id == id).Select(m=>m.Product).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }



    }
}
