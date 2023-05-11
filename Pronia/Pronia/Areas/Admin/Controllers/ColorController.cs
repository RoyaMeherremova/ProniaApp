using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Areas.Helpers;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;
using System.Net;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IColorService _colorService;
        public ColorController(AppDbContext context,
                             IColorService colorService)

        {

            _context = context;
            _colorService = colorService
;
        }


        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _colorService.GetColors();
            return View(colors);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorCreateVM color)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(color);
                }
                Color newColor = new()
                {
                    Name = color.Name,
                };
                await _context.Colors.AddAsync(newColor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.error = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Color dbColor = await _colorService.GetColordById(id);

                if (dbColor is null) return NotFound();

                _context.Colors.Remove(dbColor);

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
