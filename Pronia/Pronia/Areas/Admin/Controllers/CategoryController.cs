using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using System.Drawing;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService _categoryService;
        public CategoryController(AppDbContext context,
                             IWebHostEnvironment env,
                             ICategoryService categoryService)
        {

            _context = context;
            _env = env;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _categoryService.GetCategories();
            return View(categories);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        //CREATE ADVERTISING
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM category)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }

                Category newCategory = new()
                {
                    Name = category.Name,

                };
                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                throw;
            }

        }




        //DETAIL
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return BadRequest();

            Category category = await _categoryService.GetCategoryByIdAsync(id);    

            if (category == null) return NotFound();

            return View(category);
        }





        //-------DELETE-------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Category dbCategory = await _categoryService.GetCategoryByIdAsync(id);

                if (dbCategory == null) return NotFound();

                _context.Categories.Remove(dbCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                throw;
            }


        }




        //-----------UPDATE-----------

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            Category dbCategory = await _categoryService.GetCategoryByIdAsync(id);

            if (dbCategory == null) return NotFound();


            CategoryUpdateVM model = new()
            {
                Name = dbCategory.Name,

            };
            return View(model);

        }

        //-----------UPDATE-----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryUpdateVM categoryUpdate)
        {

            try
            {
                if (id == null) return BadRequest();

                Category dbCategory = await _categoryService.GetCategoryByIdAsync(id);

                if (dbCategory == null) return NotFound();


                CategoryUpdateVM model = new()
                {
                    Name = dbCategory.Name,

                };
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                dbCategory.Name = categoryUpdate.Name;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }

        }

    }
}
