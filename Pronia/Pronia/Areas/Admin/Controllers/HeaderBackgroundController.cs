using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HeaderBackgroundController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IHeaderBackgroundService _headerBackground;
        public HeaderBackgroundController(AppDbContext context,
                                      IHeaderBackgroundService headerBackground)
        {

            _context = context;
            _headerBackground = headerBackground;
        }
        public IActionResult Index()
        {
            List<HeaderBackground> headerBackgrounds = _headerBackground.GetHeaderBackgroundsAsync();

            return View(headerBackgrounds);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            HeaderBackground dbheaderBackground = await _headerBackground.GetHeaderBackgroundByIdAsync(id);


            HeaderBackground model = new()
            {
                Value = dbheaderBackground.Value,
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, HeaderBackground updatedHeaderBackground)
        {
            try
            {
                if (id == null) return BadRequest();
                HeaderBackground dbheaderBackground = await _headerBackground.GetHeaderBackgroundByIdAsync(id);
                if (dbheaderBackground is null) return NotFound();

                HeaderBackground model = new()
                {
                    Value = dbheaderBackground.Value,
                };

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                dbheaderBackground.Value = updatedHeaderBackground.Value;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
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
                HeaderBackground dbColor = await _headerBackground.GetHeaderBackgroundByIdAsync(id);
                if (dbColor is null) return NotFound();

                _context.HeaderBackgrounds.Remove(dbColor);

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
