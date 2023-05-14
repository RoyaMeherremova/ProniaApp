using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
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


        //SEARCH
        public async Task<IActionResult> Search(string searchText)
        {
            List<Product> products = await _context.Products.Include(m => m.Images)
                                            .Include(m => m.ProductCategories)
                                            .Include(m => m.ProductSizes)
                                            .Include(m => m.ProductTags)
                                            .Where(m => m.Name.ToLower().Contains(searchText.ToLower()))
                                            .Take(5)
                                            .ToListAsync();
            return PartialView("_SearchPartial", products);
        }

        
            public async Task<IActionResult> BlogDetail(int? id)
            {
                Blog blog = await _blogService.GetBlogdById(id);
                Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
                List<Category> categories = await _categoryService.GetCategories();
                List<Tag> tags  = await _tagService.GetAllAsync();
                List<Product> newProducts = await _productService.GetNewProducts();
                List<Blog> blogs = await _blogService.GetBlogs();
                List<BlogComment> blogComments = await _context.BlogComments.Include(m => m.AppUser).Where(m => m.BlogId == id).ToListAsync();
                CommentVM commentVM = new();
            List<Blog> relatedBlogs = new();
                foreach (var blogRelated in blogs)
                {

                    Blog reLatBlog = await _context.Blogs.Where(m => m.Id == blogRelated.Id).FirstAsync();
                    relatedBlogs.Add(reLatBlog);

                }
            BlogDetailVM model = new()
                {
                    BlogDt = blog,
                    HeaderBackgrounds = headerBackgrounds,
                    Categories = categories,
                    Tags = tags,
                    NewProducts= newProducts,
                    Blogs = blogs,
                    RelatedBlogs = relatedBlogs,
                    BlogComments = blogComments,

            };

                return View(model);
            }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PostComment(BlogDetailVM blogDetailVM, string userId, int blogId)
        {
            if (blogDetailVM.CommentVM.Message == null)
            {
                ModelState.AddModelError("Message", "Don't empty");
                return RedirectToAction(nameof(BlogDetail), new { id = blogId });
            }

            BlogComment blogComment = new()
            {
                FullName = blogDetailVM.CommentVM?.FullName,
                Email = blogDetailVM.CommentVM?.Email,
                Subject = blogDetailVM.CommentVM?.Subject,
                Message = blogDetailVM.CommentVM?.Message,
                AppUserId = userId,
                BlogId = blogId
            };

            await _context.BlogComments.AddAsync(blogComment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BlogDetail), new { id = blogId });

        }

    }
}
