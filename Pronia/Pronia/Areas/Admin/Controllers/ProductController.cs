using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISizeService _sizeService;
        private readonly ITagService _tagService;
        private readonly IColorService _colorService;
        public ProductController(AppDbContext context,
                            IWebHostEnvironment env,
                            IProductService productService,
                            ICategoryService categoryService,
                            ISizeService sizeService,
                            ITagService tagService,
                            IColorService colorService)

        {
            _context = context;
            _env = env;
            _productService = productService;  
            _categoryService = categoryService;
            _sizeService = sizeService;
            _tagService = tagService;
            _colorService = colorService;
        }
        
        public async Task<IActionResult> Index(int page = 1,int take = 5,int? cateId = null)
        {
            List<Product> products = await _productService.GetPaginatedDatas(page, take, cateId);

            List<ProductListVM> mappedDatas =  GetMappedDatas(products);

            int pageCount = await GetPageCountAsync(take);

            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount);

            ViewBag.take = take;

            return View(paginatedDatas);
        }
        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new(); 

            foreach (var product in products)
            {
                ProductListVM productVM = new()  
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    SKU= product.SKU,
                    MainImage = product.MainImage

                };
                mappedDatas.Add(productVM); 
            }
            return mappedDatas;
        }  
        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return BadRequest();

            Product dbProduct = await _productService.GetFullDataById(id);  

            if (dbProduct == null) return NotFound();

            ProductDetailVM model = new()
            {
                Name = dbProduct.Name,
                Price = dbProduct.Price,
                SKU= dbProduct.SKU,
                MainImage = dbProduct.MainImage,
                HoverImage = dbProduct.HoverImage,
                Description = dbProduct.Description,
                SaleCount= dbProduct.SaleCount,
                StockCount= dbProduct.StockCount,
                ProductCategories= dbProduct.ProductCategories,
                ProductImages = dbProduct.Images,
                ProductSizes= dbProduct.ProductSizes,
                ProductTags = dbProduct.ProductTags,
                ColorName = dbProduct.Color.Name,
                Rate= dbProduct.Rate,
            };

            return View(model);
        }


        private async Task<SelectList> GetSizesAsync()
        {
            List<Size> sizes = await _sizeService.GetAllSize();
            return new SelectList(sizes, "Id", "Name");
        }

        private async Task<SelectList> GetColorsAsync()
        {
            List<Color> colors = await _colorService.GetColors();
            return new SelectList(colors, "Id", "Name");
        }

        private async Task<SelectList> GetTagsAsync()
        {
            List<Tag> tags = await _tagService.GetAllAsync();
            return new SelectList(tags, "Id", "Name");
        }





        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



    }
}
