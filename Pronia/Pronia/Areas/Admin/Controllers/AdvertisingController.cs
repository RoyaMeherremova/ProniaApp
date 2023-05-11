using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Areas.Helpers;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertisingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IAdvertisingService _advertisingService;
        public AdvertisingController(AppDbContext context,
                             IWebHostEnvironment env,
                             IAdvertisingService advertisingService)
        {

            _context = context;
            _env = env;
            _advertisingService = advertisingService;
        }
        public async Task<IActionResult> Index()
        {
            List<Advertising> advertisings = await _advertisingService.GetAll();
            return View(advertisings);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        //CREATE ADVERTISING
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvertisingCreateVM advertising)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!advertising.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                //if (!advertising.Photo.CheckFileSize(200))
                //{
                //    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                //    return View(advertising);

                //}



                string fileName = Guid.NewGuid().ToString() + " " + advertising.Photo.FileName;
                string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                await FileHelper.SaveFileAsync(newPath, advertising.Photo);

                Advertising newAdvertising = new()
                {
                    Image = fileName,
                    Name = advertising.Name,
                    Description = advertising.Description,
                    
                };
                await _context.Advertisings.AddAsync(newAdvertising);
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
            Advertising advertising = await _advertisingService.GetAdvertisingById(id);

            if (advertising == null) return NotFound();
            return View(advertising);
        }





        //-------DELETE-------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Advertising dbAdvertising = await _advertisingService.GetAdvertisingById(id);

                if (dbAdvertising == null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbAdvertising.Image);

                FileHelper.DeleteFile(path);


                _context.Advertisings.Remove(dbAdvertising);
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

            Advertising dbAdvertising = await _advertisingService.GetAdvertisingById(id);

            if (dbAdvertising == null) return NotFound();


            AdvertisingUpdateVM model = new()
            {
                Image = dbAdvertising.Image,
                Name = dbAdvertising.Name,
                Description = dbAdvertising.Description,
               
            };
            return View(model);


        }

        //-----------UPDATE-----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AdvertisingUpdateVM advertising)
        {

            try
            {
                if (id == null) return BadRequest();

                Advertising dbAdvertising = await _advertisingService.GetAdvertisingById(id);

                if (dbAdvertising == null) return NotFound();


                AdvertisingUpdateVM model = new()
                {
                    Image = dbAdvertising.Image,
                    Name = dbAdvertising.Name,
                    Description = dbAdvertising.Description,

                };
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (advertising.Photo != null)
                {
                    if (!advertising.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(model);
                    }


                    //if (!advertising.Photo.CheckFileSize(200))
                    //{
                    //    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    //    return View(model);
                    //}



                    string deletePath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbAdvertising.Image);
                    FileHelper.DeleteFile(deletePath);
                    string fileName = Guid.NewGuid().ToString() + " " + advertising.Photo.FileName;
                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                    await FileHelper.SaveFileAsync(newPath, advertising.Photo);
                    dbAdvertising.Image = fileName;
                }
                else
                {
                    Advertising newAdvertising = new()
                    {
                        Image = dbAdvertising.Image
                    };
                }

                dbAdvertising.Name = advertising.Name;
                dbAdvertising.Description = advertising.Description;
           

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


