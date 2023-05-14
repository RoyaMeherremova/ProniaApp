using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;
using System.Reflection.Metadata;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISizeService _sizeService;
        private readonly ITagService _tagService;
        private readonly IColorService _colorService;
        public ProductController(AppDbContext context,
                            IWebHostEnvironment env,
                            IProductService productService,
                            ICategoryService categoryService,
                            ISizeService sizeService,
                            ITagService tagService,
                            IColorService colorService)

        {
            _context = context;
            _env = env;
            _productService = productService;  
            _categoryService = categoryService;
            _sizeService = sizeService;
            _tagService = tagService;
            _colorService = colorService;
        }
        //INDEX
        public async Task<IActionResult> Index(int page = 1,int take = 5,int? cateId = null, int? tagId = null)
        {
            List<Product> products = await _productService.GetPaginatedDatas(page, take, cateId,tagId);

            List<ProductListVM> mappedDatas =  GetMappedDatas(products);

            int pageCount = await GetPageCountAsync(take);

            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount);

            ViewBag.take = take;

            return View(paginatedDatas);
        }
        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new(); 

            foreach (var product in products)
            {
                ProductListVM productVM = new()  
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    MainImage = product.Images.FirstOrDefault(m=>m.IsMain).Image,
                    SKU= product.SKU,

                };
                mappedDatas.Add(productVM); 
            }
            return mappedDatas;
        }  
        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }

        //DETAIL
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Product dbProduct = await _productService.GetFullDataById(id);

                if (dbProduct == null) return NotFound();

                ProductDetailVM model = new()
                {
                    Name = dbProduct.Name,
                    Price = dbProduct.Price,
                    SKU = dbProduct.SKU,
                    Description = dbProduct.Description,
                    SaleCount = dbProduct.SaleCount,
                    StockCount = dbProduct.StockCount,
                    ProductCategories = dbProduct.ProductCategories,
                    ProductImages = dbProduct.Images,
                    ProductSizes = dbProduct.ProductSizes,
                    ProductTags = dbProduct.ProductTags,
                    ColorName = dbProduct.Color.Name,
                    Rate = dbProduct.Rate,
                };

                return View(model);

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private async Task<SelectList> GetSizesAsync()
        {
            List<Size> sizes = await _sizeService.GetAllSize();
            return new SelectList(sizes, "Id", "Name");
        }

        private async Task<SelectList> GetColorsAsync()
        {
            List<Color> colors = await _colorService.GetColors();
            return new SelectList(colors, "Id", "Name");
        }

        private async Task<SelectList> GetTagsAsync()
        {
            List<Tag> tags = await _tagService.GetAllAsync();
            return new SelectList(tags, "Id", "Name");
        }
        private async Task<SelectList> GetCategoriessAsync()
        {
            List<Category> categories = await _categoryService.GetCategories(); 
            return new SelectList(categories, "Id", "Name");
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            @ViewBag.colors = await GetColorsAsync();
            @ViewBag.tags = await GetTagsAsync();
            @ViewBag.sizes = await GetSizesAsync();
            @ViewBag.categories = await GetCategoriessAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
           
            try
            {
                @ViewBag.colors = await GetColorsAsync();
                @ViewBag.tags = await GetTagsAsync();
                @ViewBag.sizes = await GetSizesAsync();
                @ViewBag.categories = await GetCategoriessAsync();

                if (!ModelState.IsValid) return View(model);



                Product newProduct = new();
                List<ProductImage> productImages = new();
                List<ProductTag> productTags = new();
                List<ProductSize> productSizes = new();
                List<ProductCategory> productCategories = new();

                foreach (var photo in model.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(model);
                    }
                    if (!photo.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);

                    }
                }


                foreach (var photo in model.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(path, photo);

                    ProductImage productImage = new()
                    {
                        Image = fileName
                    };

                    productImages.Add(productImage);
                }
                newProduct.Images = productImages;
                newProduct.Images.FirstOrDefault().IsMain = true;
                newProduct.Images.Skip(1).FirstOrDefault().IsHover = true;

                if (model.CategoryIds.Count > 0)
                {
                    foreach(var item in model.CategoryIds)
                    {

                        ProductCategory productCategory = new()
                        {
                            CategoryId =item
                        };

                        productCategories.Add(productCategory);
                    }
                    newProduct.ProductCategories = productCategories;
                }
                else
                {
                    ModelState.AddModelError("CategoryIds", "Don't be empty");
                }


                if (model.TagIds.Count > 0)
                {
                    foreach (var item in model.TagIds)
                    {

                        ProductTag productTag = new()
                        {
                            TagId = item
                        };

                        productTags.Add(productTag);
                    }
                    newProduct.ProductTags = productTags;
                }
                else
                {
                    ModelState.AddModelError("TagIds", "Don't be empty");
                }

                if (model.SizeIds.Count > 0)
                {
                    foreach (var item in model.SizeIds)
                    {

                        ProductSize productSize = new()
                        {
                            SizeId = item
                        };

                        productSizes.Add(productSize);
                    }
                    newProduct.ProductSizes = productSizes;
                }
                else
                {
                    ModelState.AddModelError("SizeIds", "Don't be empty");
                }

                var convertedPrice = decimal.Parse(model.Price);

                Random random = new();

                newProduct.Name = model.Name;
                newProduct.Price = convertedPrice;
                newProduct.SKU = model.Name.Substring(0, 3) + "-" + random.Next();
                newProduct.Description = model.Description;
                newProduct.Rate = model.Rate;
                newProduct.SaleCount = model.SaleCount;
                newProduct.StockCount = model.StockCount;
                newProduct.ColorId = model.ColorId;
                newProduct.Rate = model.Rate;

                await _context.ProductImages.AddRangeAsync(productImages);
                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                @ViewBag.colors = await GetColorsAsync();
                @ViewBag.tags = await GetTagsAsync();
                @ViewBag.sizes = await GetSizesAsync();
                @ViewBag.categories = await GetCategoriessAsync();
                if (id == null) return BadRequest();

                Product dbProduct = await _productService.GetFullDataById(id);
                if (dbProduct == null) return NotFound();

                ProductUpdateVM model = new()
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Description = dbProduct.Description,
                    Rate = dbProduct.Rate,
                    SKU = dbProduct.SKU,
                    Images = dbProduct.Images.ToList(),
                    SaleCount = dbProduct.SaleCount,
                    StockCount = dbProduct.StockCount,
                    Price = dbProduct.Price.ToString(),
                    CategoryIds = dbProduct.ProductCategories.Select(m => m.Category.Id).ToList(),
                    SizeIds = dbProduct.ProductSizes.Select(m => m.Size.Id).ToList(),
                    TagIds = dbProduct.ProductTags.Select(m => m.Tag.Id).ToList(),
                    ColorId = dbProduct.Color.Id,

                };
                return View(model);

            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductUpdateVM updatedProduct)
        {
            try
            {
                @ViewBag.colors = await GetColorsAsync();
                @ViewBag.tags = await GetTagsAsync();
                @ViewBag.sizes = await GetSizesAsync();
                @ViewBag.categories = await GetCategoriessAsync();

                if (id == null) return BadRequest();

                Product dbProduct = await _productService.GetFullDataById(id);

                if (dbProduct == null) return NotFound();

                ProductUpdateVM model = new()
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    SKU = dbProduct.SKU,
                    Description = dbProduct.Description,
                    Price = dbProduct.Price.ToString(),
                    Rate = dbProduct.Rate,
                    Images = dbProduct.Images.ToList(),
                    SaleCount = dbProduct.SaleCount,
                    StockCount = dbProduct.StockCount,
                    CategoryIds = dbProduct.ProductCategories.Select(m => m.Category.Id).ToList(),
                    SizeIds = dbProduct.ProductSizes.Select(m => m.Size.Id).ToList(),
                    TagIds = dbProduct.ProductTags.Select(m => m.Tag.Id).ToList(),
                    ColorId = dbProduct.Color.Id,

                };

                if (!ModelState.IsValid) return View(model);


              
                if (updatedProduct.Photos != null)
                {
                    foreach (var photo in updatedProduct.Photos)
                    {
                        if (!photo.CheckFileType("image/"))
                        {
                            ModelState.AddModelError("Photo", "File type must be image");
                            return View(model);
                        }
                        if (!photo.CheckFileSize(200))
                        {
                            ModelState.AddModelError("Photo", "Image size must be max 200kb");
                            return View(model);

                        }
                    }

                    foreach (var photo in dbProduct.Images)
                    {

                        string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", photo.Image);

                        FileHelper.DeleteFile(path);


                    }
           
                    List<ProductImage> productImages = new();
                 
          
                    foreach (var photo in updatedProduct.Photos)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                        string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                        await FileHelper.SaveFileAsync(path, photo);

                        ProductImage productImage = new()
                        {
                            Image = fileName
                        };

                        productImages.Add(productImage);
                    }
                    dbProduct.Images = productImages;
                    dbProduct.Images.FirstOrDefault().IsMain = true;
                    dbProduct.Images.Skip(1).FirstOrDefault().IsHover = true;

                    await _context.ProductImages.AddRangeAsync(productImages);
                }
                else
                {
                    Product newProduct = new()
                    {
                       Images= dbProduct.Images
                    };
                }


                List<ProductCategory> productCategories = new();
                if (updatedProduct.CategoryIds.Count > 0)
                {
                    foreach (var item in updatedProduct.CategoryIds)
                    {

                        ProductCategory productCategory = new()
                        {
                            CategoryId = item
                        };

                        productCategories.Add(productCategory);
                    }
                    dbProduct.ProductCategories = productCategories;
                }
                else
                {
                    updatedProduct.CategoryIds = dbProduct.ProductCategories.Select(p=>p.CategoryId).ToList();
                }


                List<ProductTag> productTags = new();

                if (updatedProduct.TagIds.Count > 0)
                {
                    foreach (var item in updatedProduct.TagIds)
                    {

                        ProductTag productTag = new()
                        {
                            TagId = item
                        };

                        productTags.Add(productTag);
                    }
                    dbProduct.ProductTags = productTags;
                }
                else
                {

                    updatedProduct.TagIds = dbProduct.ProductTags.Select(p => p.Tag.Id).ToList();
                }

                List<ProductSize> productSizes = new();
                if (updatedProduct.SizeIds.Count > 0)
                {
                    foreach (var item in updatedProduct.SizeIds)
                    {

                        ProductSize productSize = new()
                        {
                            SizeId = item
                        };

                        productSizes.Add(productSize);
                    }
                    dbProduct.ProductSizes = productSizes;
                }
                else
                {
                    updatedProduct.SizeIds = dbProduct.ProductSizes.Select(p => p.Size.Id).ToList();
                }

                var convertedPrice = decimal.Parse(updatedProduct.Price);

                dbProduct.Name = updatedProduct.Name;
                dbProduct.Price = convertedPrice;
                dbProduct.SKU =updatedProduct.SKU;
                dbProduct.Description = updatedProduct.Description;
                dbProduct.Rate = updatedProduct.Rate;
                dbProduct.SaleCount = updatedProduct.SaleCount;
                dbProduct.StockCount = updatedProduct.StockCount;
                dbProduct.ColorId = updatedProduct.ColorId;

               
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
                  Product dbProduct = await _productService.GetFullDataById(id);
                 if (dbProduct == null) return NotFound();

                foreach (var photo in dbProduct.Images)
                {

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", photo.Image);

                    FileHelper.DeleteFile(path);
                }

                _context.Products.Remove(dbProduct);
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
