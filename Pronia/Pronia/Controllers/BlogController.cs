using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlogService _blogService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly IAdvertisingService _advertisingService;
        private readonly IProductService _productService;

        public BlogController(AppDbContext context,
                             IBlogService blogService,
                             ITagService tagService,
                             IAdvertisingService advertisingService,
                             ICategoryService categoryService,
                             IProductService productService)
        {
            _context = context;
            _blogService = blogService;
            _tagService = tagService;
            _advertisingService = advertisingService;
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(int page= 1,int take=2)
        {
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Blog> paginateBlogs = await _blogService.GetPaginatedDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Blog> paginatedDatas = new(paginateBlogs, page, pageCount);
            List<Blog> blogs = await _blogService.GetBlogs();
            List<Category> categories = await _categoryService.GetCategories();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Tag> tags = await _tagService.GetAllAsync();
            BlogVM model = new()
            {
                HeaderBackgrounds = headerBackgrounds,
                Blogs = blogs,
                PaginatedDatas = paginatedDatas,
                Categories = categories,
                Tags = tags,
                NewProducts = newProducts,
            };
           
            return View(model);
        }
        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _blogService.GetCountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }

    }
}
