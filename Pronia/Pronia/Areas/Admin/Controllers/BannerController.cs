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
    public class BannerController : Controller
    {
       
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IBannerService _bannerService;
        public BannerController(AppDbContext context,
                             IWebHostEnvironment env,
                             IBannerService bannerService)
                             
        {

            _context = context;
            _env = env;
            _bannerService = bannerService;  
        }
        public async Task<IActionResult> Index()
        {
            List<Bannner> banners = await _bannerService.GetAllAsync();
            return View(banners);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //CREATE BANNER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerCreateVM banner)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!banner.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                //if (!banner.Photo.CheckFileSize(200))
                //{
                //    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                //    return View(banner);

                //}



                string fileName = Guid.NewGuid().ToString() + " " + banner.Photo.FileName;
                string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                await FileHelper.SaveFileAsync(newPath, banner.Photo);

                Bannner newBanner = new()
                {
                    Image = fileName,
                    Name = banner.Name,
                    Title = banner.Tittle,

                };
                await _context.Bannners.AddAsync(newBanner);
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

            Bannner dbBanner = await _bannerService.GetBannerById(id);

            if (dbBanner == null) return NotFound();
            return View(dbBanner);
        }

        //-------DELETE-------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Bannner dbBanner = await _bannerService.GetBannerById(id);  

                if (dbBanner == null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBanner.Image);

                FileHelper.DeleteFile(path);


                _context.Bannners.Remove(dbBanner);
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

            Bannner dbBanner = await _bannerService.GetBannerById(id);

            if (dbBanner == null) return NotFound();


            BannerUpdateVM model = new()
            {
                Image = dbBanner.Image,
                Name = dbBanner.Name,
                Tittle = dbBanner.Title,

            };
            return View(model);


        }

        //-----------UPDATE-----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BannerUpdateVM banner)
        {

            try
            {
                if (id == null) return BadRequest();

                Bannner dbBanner = await _bannerService.GetBannerById(id);

                if (dbBanner == null) return NotFound();

                BannerUpdateVM model = new()
                {
                    Image = dbBanner.Image,
                    Name = dbBanner.Name,
                    Tittle = dbBanner.Title,
                };
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (banner.Photo != null)
                {
                    if (!banner.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(model);
                    }


                    //if (!banner.Photo.CheckFileSize(200))
                    //{
                    //    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    //    return View(model);
                    //}



                    string deletePath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBanner.Image);
                    FileHelper.DeleteFile(deletePath);
                    string fileName = Guid.NewGuid().ToString() + " " + banner.Photo.FileName;
                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                    await FileHelper.SaveFileAsync(newPath, banner.Photo);
                    dbBanner.Image = fileName;
                }
                else
                {
                    Bannner newBanner = new()
                    {
                        Image = dbBanner.Image
                    };
                }

                dbBanner.Name = banner.Name;
                dbBanner.Title = banner.Tittle;


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
