using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
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

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Color color = await _colorService.GetColorById(id);
            if (color is null) return NotFound();
            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Color dbColor = await _colorService.GetColorById(id);

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


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            Color dbColor = await _colorService.GetColorById(id);
            if (dbColor is null) return NotFound();

            ColorUpdateVM model = new()
            {
                Name = dbColor.Name,
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ColorUpdateVM colorUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Color dbColor = await _colorService.GetColorById(id);

                if (dbColor is null) return NotFound();

                ColorUpdateVM model = new()
                {
                    Name = dbColor.Name,
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }



                dbColor.Name = colorUpdate.Name;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }
        }


    }
}
