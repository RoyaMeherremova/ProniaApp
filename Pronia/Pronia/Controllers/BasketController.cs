using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;


        private readonly IBasketService _basketService;

        private readonly IProductService _productService;
        public BasketController(AppDbContext context,
                              IBasketService basketService,
                              IProductService productService)
        {
            _context = context;
            _basketService = basketService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {


            List<BasketVM> basketProducts = _basketService.GetBasketDatas();

            List<BasketDetailVM> basketDetails = new();

            foreach (var product in basketProducts)
            {
                Product dbProduct = await _productService.GetFullDataById(product.Id);

                basketDetails.Add(new BasketDetailVM
                {

                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Price = dbProduct.Price,
                    Image = dbProduct.Images.Where(m => m.IsMain).FirstOrDefault().Image,
                    Count = product.Count,
                    Total = dbProduct.Price * product.Count,
                });
            }

            return View(basketDetails);

        }

    


        [HttpPost]
        //[ActionName("Delete")]
        public IActionResult DeleteProductFromBasket(int? id)
        {
            if (id == null) return BadRequest();

            _basketService.DeleteProductFromBasket((int)id);
            int count = _basketService.GetBasketDatas().Sum(m => m.Count);
            return Ok(count);
        }
 


        public IActionResult DecreaseCountProductFromBasket(int? id)
        {

            List<BasketVM> basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            BasketVM decreaseProduct = basketProducts.Find(m => m.Id == id);

            if (decreaseProduct.Count > 1)
            {
                var countProduct = --decreaseProduct.Count;

                Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

                return Ok(countProduct);
            }

            return Ok();
        }

        public IActionResult IncreaseCountProductFromBasket(int? id)
        {

            List<BasketVM> basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            BasketVM? increaseProduct = basketProducts.Find(m => m.Id == id);


            var countProduct = ++increaseProduct.Count;

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));


            return Ok(countProduct);

        }

    }
}
