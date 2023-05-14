using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IBlogService _blogService;
        private readonly IAuthorService _authorService;
        public BlogController(AppDbContext context,
                             IWebHostEnvironment env,
                             IAuthorService authorService,
                             IBlogService blogService)

        {

            _context = context;
            _env = env;
            _authorService= authorService;
            _blogService = blogService;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _blogService.GetBlogs();
            return View(blogs);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            @ViewBag.authors = await GetAuthorsAsync();
            return View();
        }

        private async Task<SelectList> GetAuthorsAsync()
        {
            List<Author> authors = await _authorService.GetAll();
            return new SelectList(authors, "Id", "Name");
        }



        //CREATE BANNER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blog)
        {
            @ViewBag.authors = await GetAuthorsAsync();

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(blog);
                }


                foreach (var photo in blog.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(blog);
                    }
                    if (!photo.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(blog);

                    }
                }

                List<BlogImage> blogImages = new();

                foreach (var photo in blog.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(path, photo);

                    BlogImage blogImage = new()
                    {
                        Image = fileName
                    };

                    blogImages.Add(blogImage);
                }

                Blog newBlog = new()
                {
                    Tittle = blog.Tittle,
                    Description = blog.Description,
                    AuthorId = blog.AuthorId,
                    Images = blogImages
                };

                await _context.BlogImages.AddRangeAsync(blogImages);
                await _context.Blogs.AddAsync(newBlog);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }






        }


        ////DETAIL
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return BadRequest();

            Blog dbBlog = await _blogService.GetBlogdById(id);

            if (dbBlog == null) return NotFound();
            return View(dbBlog);
        }

        ////-------DELETE-------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Blog dbBlog = await _blogService.GetBlogdById(id);

                if (dbBlog == null) return NotFound();

                foreach(var photo in dbBlog.Images)
                {

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", photo.Image);

                    FileHelper.DeleteFile(path);


                }

                _context.Blogs.Remove(dbBlog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                throw;
            }


        }


        ////-----------UPDATE-----------

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();


            @ViewBag.authors = await GetAuthorsAsync();

            Blog dbBlog = await _blogService.GetBlogdById(id);

            if (dbBlog == null) return NotFound();

            BlogUpdateVM model = new()
            {
                Id = dbBlog.Id,
                Tittle = dbBlog.Tittle,
                AuthorId = dbBlog.AuthorId,
                Images = dbBlog.Images.ToList(),
                Description = dbBlog.Description
            };


            return View(model);


        }

        //-----------UPDATE-----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogUpdateVM updatedBlog)
        {
            @ViewBag.authors = await GetAuthorsAsync();

            if (id == null) return BadRequest();

            Blog dbBlog = await _blogService.GetBlogdById(id);

            if (dbBlog == null) return NotFound();

            BlogUpdateVM model = new()
            {
                Tittle = dbBlog.Tittle,
                AuthorId = dbBlog.AuthorId,
                Images = dbBlog.Images.ToList(),
                Description = dbBlog.Description
            };
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (updatedBlog.Photos is not null)
            {
                foreach (var photo in updatedBlog.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        updatedBlog.Images = dbBlog.Images.ToList();
                        return View(updatedBlog);
                    }

                    if (!photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        updatedBlog.Images = dbBlog.Images.ToList();
                        return View(updatedBlog);
                    }
                }

              

                foreach (var dbBlogPhoto in dbBlog.Images)
                {
                    string deletePath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBlogPhoto.Image);
                    FileHelper.DeleteFile(deletePath);
                }

                   List<BlogImage> blogImages = new();

                foreach (var photo in updatedBlog.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(path, photo);
                    
                    BlogImage blogImage = new()
                    {
                        Image = fileName
                    };

                    blogImages.Add(blogImage);
                }

                await _context.BlogImages.AddRangeAsync(blogImages);
                dbBlog.Images = blogImages;
            }
            else
            {
                Blog blog = new()
                {
                    Images = dbBlog.Images
                };
            }


            dbBlog.Tittle = updatedBlog.Tittle;
            dbBlog.Description = updatedBlog.Description;
            dbBlog.AuthorId = updatedBlog.AuthorId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }




    
}

