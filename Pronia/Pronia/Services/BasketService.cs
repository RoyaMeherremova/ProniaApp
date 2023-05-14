using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Services
{
    public class BasketService:IBasketService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;  

        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public void AddProductToBasket(BasketVM existProduct, Product dbProduct, List<BasketVM> basket)
        {

            if (existProduct == null)
            {
                basket?.Add(new BasketVM
                {   
                    Id = dbProduct.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }

            _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));  
        }

        public void DeleteProductFromBasket(int id)
        {
            List<BasketVM> basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(_httpContextAccessor.HttpContext.Request.Cookies["basket"]);

            BasketVM deletedProduct = basketProducts.FirstOrDefault(m => m.Id == id);

            basketProducts.Remove(deletedProduct);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));
        }

        public List<BasketVM> GetBasketDatas()
        {

            List<BasketVM> basket; 

            if (_httpContextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(_httpContextAccessor.HttpContext.Request.Cookies["basket"]); 
            }
            else
            {
                basket = new List<BasketVM>();  
            }
            return basket;
        }
    }
}
