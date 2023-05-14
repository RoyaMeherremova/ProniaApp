using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;
        public SliderController(AppDbContext context,
                                IWebHostEnvironment env,
                                ISliderService slider)
        {
            
            _context = context;
            _env = env;
            _sliderService = slider;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();
            return View(sliders);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //CREATE SLIDER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(slider);
                }

                
                
                    if (!slider.Photo.CheckFileType("image/"))   
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(slider);
                    }

                if (!slider.Photo.CheckFileSize(500))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View(slider);

                }



                string fileName = Guid.NewGuid().ToString() + " " + slider.Photo.FileName;
                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                    await FileHelper.SaveFileAsync(newPath, slider.Photo);

                    Slider newSlider = new()  
                    {
                        Image = fileName,
                        Tittle = slider.Tittle,
                        Description = slider.Description,
                        Offer = slider.Offer
                    };
                await  _context.Sliders.AddAsync(newSlider);   
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
            Slider slider = await _sliderService.GetSliderById(id);

            if (slider == null) return NotFound();
            return View(slider);
        }

        //-------PHOTO DELETE FROM PROJECT AND DATABASE-------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id) 
        {
            try
            {
                if (id == null) return BadRequest();

                Slider slider = await _sliderService.GetSliderById(id);  

                if (slider == null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", slider.Image);   

                FileHelper.DeleteFile(path);  


                _context.Sliders.Remove(slider);
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

            Slider dbSlider = await _sliderService.GetSliderById(id);


            if (dbSlider == null) return NotFound();

            SliderUpdateVM model = new()
            {
                Image = dbSlider.Image,
                Tittle = dbSlider.Tittle,   
                Description= dbSlider.Description,
                Offer= dbSlider.Offer,
            };
            return View(model);


        }

        //-----------UPDATE-----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderUpdateVM slider)
        {

            try
            {
                if (id == null) return BadRequest();

                Slider dbSlider = await _sliderService.GetSliderById(id);

                if (dbSlider == null) return NotFound();


                SliderUpdateVM model = new()
                {
                    Image = dbSlider.Image,
                    Tittle = dbSlider.Tittle,
                    Description = dbSlider.Description,
                    Offer = dbSlider.Offer,
                };
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                if (slider.Photo != null)
                {
                    if (!slider.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(model);
                    }
                    if (!slider.Photo.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }

                    string deletePath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbSlider.Image);
                    FileHelper.DeleteFile(deletePath);
                    string fileName = Guid.NewGuid().ToString() + " " + slider.Photo.FileName;
                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                    await FileHelper.SaveFileAsync(newPath, slider.Photo);
                    dbSlider.Image = fileName;
                }
                else
                {
                    Slider newSlider = new()   
                    {
                        Image = dbSlider.Image
                    };
                }

                dbSlider.Tittle = slider.Tittle;
                dbSlider.Description = slider.Description;
                dbSlider.Offer = slider.Offer;

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
