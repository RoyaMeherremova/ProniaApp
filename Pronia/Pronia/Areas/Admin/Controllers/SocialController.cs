using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISocialService _socialService;
        public SocialController(AppDbContext context,
                             ISocialService socialService)
        {

            _context = context;
            _socialService = socialService;
        }
        public async Task<IActionResult> Index()
        {
            List<Social> socials = await _socialService.GetAllSocials();
            return View(socials);
        }

        //DETAIL
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return BadRequest();
            Social DbSocial = await _socialService.GetSocialById(id);
            if (DbSocial == null) return NotFound();

            return View(DbSocial);
        }

        //-------PHOTO DELETE FROM PROJECT AND DATABASE-------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                Social DbSocial = await _socialService.GetSocialById(id);
                if (DbSocial == null) return NotFound();

                _context.Socials.Remove(DbSocial);
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
            Social DbSocial = await _socialService.GetSocialById(id);
            if (DbSocial == null) return NotFound();

            SocialUpdateVM model = new()
            {
                Link = DbSocial.Link,
            };
            return View(model);


        }

        //-----------UPDATE-----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SocialUpdateVM social)
        {

            try
            {
                if (id == null) return BadRequest();
                Social DbSocial = await _socialService.GetSocialById(id);
                if (DbSocial == null) return NotFound();

                SocialUpdateVM model = new()
                {
                    Link = DbSocial.Link,
                };

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                

                DbSocial.Link = social.Link;
                

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
